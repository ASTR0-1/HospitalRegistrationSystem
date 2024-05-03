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
    /// <param name="_currentUserService">The current user service.</param>
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
    public async Task<Result<PagedList<ApplicationUserDto>>> GetAllByRoleAsync(PagingParameters paging, string role)
    {
        var applicationUsers = await _repository.ApplicationUser.GetApplicationUsersByRoleAsync(role, paging);

        var applicationUsersDto = _mapper.Map<IEnumerable<ApplicationUserDto>>(applicationUsers);
        var pagedApplicationUserDtos = PagedList<ApplicationUserDto>.ToPagedList(applicationUsersDto, paging.PageNumber, paging.PageSize);

        return Result<PagedList<ApplicationUserDto>>.Success(pagedApplicationUserDtos);
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
    public async Task<Result> AssignUserToRoleAsync(int userId, string role)
    {
        var applicationUser = await _repository.ApplicationUser.GetApplicationUserAsync(userId);
        if (applicationUser is null)
            return Result.Failure(ApplicationUserError.UserIdNotFound(userId));

        var roleExists = RoleConstants.RoleExists(role);
        if (!roleExists)
            return Result.Failure(RoleError.RoleNotFound(role));

        if (!CanCallerAssignRole(role))
            return Result.Failure(ApplicationUserError.UnauthorizedRoleAssignment(_currentUserService.GetApplicationUserId(), role));

        var result = await _repository.ApplicationUser.AssignUserToRoleAsync(applicationUser, role);

        if (!result.Succeeded)
        {
            var sb = new StringBuilder();
            foreach (var error in result.Errors)
                sb.AppendLine(error.Description);

            return Result.Failure(ApplicationUserError.RoleAssignmentFailed(role, sb.ToString()));
        }

        return Result.Success();
    }

    private bool CanCallerAssignRole(string role)
    {
        if (_currentUserService.IsInRole(RoleConstants.MasterSupervisor))
            return true;

        if (_currentUserService.IsInRole(RoleConstants.Supervisor))
        {
            return role switch
            {
                RoleConstants.Doctor or RoleConstants.Receptionist => true,
                _ => false
            };
        }

        return false;
    }
}