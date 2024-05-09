using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HospitalRegistrationSystem.Application.DTOs.ApplicationUserDTOs;
using HospitalRegistrationSystem.Application.Interfaces;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Application.Interfaces.Services;
using HospitalRegistrationSystem.Application.Utility;
using HospitalRegistrationSystem.Domain.Constants;
using HospitalRegistrationSystem.Domain.Entities;
using HospitalRegistrationSystem.Domain.Errors;
using HospitalRegistrationSystem.Domain.Shared;

namespace HospitalRegistrationSystem.Application.Services;

/// <summary>
///     Represents a service for managing application users.
/// </summary>
public class ApplicationUserService : IApplicationUserService
{
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _repository;
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ApplicationUserService"/> class.
    /// </summary>
    /// <param name="repository">The repository manager.</param>
    /// <param name="mapper">The mapper.</param>
    /// <param name="currentUserService">The current user service.</param>
    public ApplicationUserService(IRepositoryManager repository, IMapper mapper, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    /// <inheritdoc />
    public async Task<Result<ApplicationUserDto>> GetAsync(int userId)
    {
        var applicationUser = await _repository.ApplicationUser.GetApplicationUserAsync(userId);
        if (applicationUser is null)
            return Result<ApplicationUserDto>.Failure(ApplicationUserError.UserIdNotFound(userId));

        var applicationUserDto = _mapper.Map<ApplicationUserDto>(applicationUser);

        return Result<ApplicationUserDto>.Success(applicationUserDto);
    }

    /// <inheritdoc />
    public async Task<Result<PagedList<ApplicationUserDto>>> GetAllAsync(PagingParameters paging, string? searchQuery, string role, int? hospitalId = null)
    {
        if (hospitalId is not null && 
            await _repository.Hospital.GetHospitalAsync(hospitalId.Value) is null)
        {
            return Result<PagedList<ApplicationUserDto>>.Failure(HospitalError.HospitalIdNotFound(hospitalId.Value));
        }

        var applicationUsers = await _repository.ApplicationUser.GetApplicationUsersAsync(paging, searchQuery, role, hospitalId);

        var applicationUsersDto = _mapper.Map<PagedList<ApplicationUserDto>>(applicationUsers);

        return Result<PagedList<ApplicationUserDto>>.Success(applicationUsersDto);
    }

    /// <inheritdoc />
    public async Task<Result> UpdateAsync(ApplicationUserDto applicationUserDto)
    {
        var callerId = _currentUserService.GetApplicationUserId();
        var isUserMasterSupervisor = _currentUserService.IsInRole(RoleConstants.MasterSupervisor);
        var isUserSupervisor = _currentUserService.IsInRole(RoleConstants.Supervisor);

        if (!isUserSupervisor
            && !isUserMasterSupervisor
            && applicationUserDto.Id != callerId)
        {
            return Result.Failure(ApplicationUserError.UserUpdateFailed($"Caller with id '{callerId}' doesn't have rights to update other users."));
        }

        var existingUser = await _repository.ApplicationUser.GetApplicationUserAsync(applicationUserDto.Id);
        if (existingUser is null)
            return Result.Failure(ApplicationUserError.UserIdNotFound(applicationUserDto.Id));

        _mapper.Map(applicationUserDto, existingUser);

        var result = await _repository.ApplicationUser.UpdateApplicationUserAsync(existingUser);

        if (!result.Succeeded)
        {
            var sb = new StringBuilder();
            foreach (var error in result.Errors)
                sb.AppendLine(error.Description);

            return Result.Failure(ApplicationUserError.UserUpdateFailed(sb.ToString()));
        }

        return Result.Success();
    }

    /// <inheritdoc />
    public async Task<Result> AssignEmployeeAsync(int userId, string? specialty, string role, int hospitalId)
    {
        var applicationUser = await _repository.ApplicationUser.GetApplicationUserAsync(userId);
        if (applicationUser is null)
            return Result.Failure(ApplicationUserError.UserIdNotFound(userId));

        var roleExists = RoleConstants.RoleExists(role);
        if (!roleExists)
            return Result.Failure(RoleError.RoleNotFound(role));

        if (!CanCallerAssignEmployee(role, hospitalId))
            return Result.Failure(ApplicationUserError.UnauthorizedEmployeeAssignment(_currentUserService.GetApplicationUserId(), role, hospitalId));

        var result = await _repository.ApplicationUser.AssignUserToRoleAsync(applicationUser, role);

        if (!result.Succeeded)
        {
            var sb = new StringBuilder();
            foreach (var error in result.Errors)
                sb.AppendLine(error.Description);

            return Result.Failure(ApplicationUserError.RoleAssignmentFailed(role, sb.ToString()));
        }

        applicationUser.Specialty = specialty;
        await _repository.ApplicationUser.UpdateApplicationUserAsync(applicationUser);

        return Result.Success();
    }

    private bool CanCallerAssignEmployee(string role, int hospitalId)
    {
        if (_currentUserService.IsInRole(RoleConstants.MasterSupervisor))
            return true;

        if (!_currentUserService.IsInRole(RoleConstants.Supervisor)) 
            return false;

        var canAssignRole = role switch
        {
            RoleConstants.Doctor or RoleConstants.Receptionist => true,
            _ => false
        };

        var canAssignHospital = _currentUserService.GetApplicationUserHospitalId() == hospitalId;

        return canAssignRole && canAssignHospital;
    }
}