using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HospitalRegistrationSystem.Application.DTOs.AppointmentDTOs;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Application.Interfaces.Services;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Constants;
using HospitalRegistrationSystem.Domain.Entities;
using HospitalRegistrationSystem.Domain.Errors;
using HospitalRegistrationSystem.Domain.Shared.ResultPattern;

namespace HospitalRegistrationSystem.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _repository;

    public AppointmentService(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<Result> AddNewAsync(AppointmentForCreationDto appointmentDto)
    {
        if (appointmentDto.DoctorId == appointmentDto.ClientId)
            return Result.Failure(AppointmentError.DoctorAndClientAreSame(appointmentDto.ClientId));

        var appointment = _mapper.Map<Appointment>(appointmentDto);

        var client = await _repository.ApplicationUser.GetApplicationUserAsync(appointmentDto.ClientId);
        if (client is null)
            return Result.Failure(ApplicationUserError.UserIdNotFound(appointmentDto.ClientId));

        var doctor = await _repository.ApplicationUser.GetApplicationUserAsync(appointmentDto.DoctorId);
        if (doctor is null)
            return Result.Failure(ApplicationUserError.UserIdNotFound(appointmentDto.DoctorId));

        var isDoctorInRole = await _repository.ApplicationUser.CheckUserInRoleAsync(appointmentDto.DoctorId, RoleConstants.Doctor);
        if (!isDoctorInRole)
            return Result.Failure(AppointmentError.DoctorNotInRole(appointmentDto.DoctorId));

        appointment.ApplicationUsers.Add(client);
        appointment.ApplicationUsers.Add(doctor);

        _repository.Appointment.CreateAppointment(appointment);
        await _repository.SaveAsync();

        return Result.Success();
    }

    /// <inheritdoc/>
    public async Task<Result<AppointmentDto>> GetAsync(int appointmentId)
    {
        var appointment = await _repository.Appointment.GetAppointmentAsync(appointmentId);
        if (appointment is null)
            return Result<AppointmentDto>.Failure(AppointmentError.AppointmentNotFound(appointmentId));

        var appointmentDto = _mapper.Map<AppointmentDto>(appointment);

        return Result<AppointmentDto>.Success(appointmentDto);
    }

    /// <inheritdoc/>
    public async Task<Result<PagedList<AppointmentDto>>> GetIncomingByUserIdAsync(PagingParameters paging, int userId)
    {
        var user = await _repository.ApplicationUser.GetApplicationUserAsync(userId);
        if (user is null)
            return Result<PagedList<AppointmentDto>>.Failure(ApplicationUserError.UserIdNotFound(userId));

        var appointments = await _repository.Appointment.GetIncomingAppointmentsByUserIdAsync(paging, userId, trackChanges: false);
        var appointmentsDto = _mapper.Map<PagedList<AppointmentDto>>(appointments);

        return Result<PagedList<AppointmentDto>>.Success(appointmentsDto);
    }

    /// <inheritdoc/>
    public async Task<Result<PagedList<AppointmentDto>>> GetAllByUserIdAsync(PagingParameters paging, int userId)
    {
        var user = await _repository.ApplicationUser.GetApplicationUserAsync(userId);
        if (user is null)
            return Result<PagedList<AppointmentDto>>.Failure(ApplicationUserError.UserIdNotFound(userId));

        var appointments = await _repository.Appointment.GetAppointmentsByUserIdAsync(paging, userId, isVisited: null, trackChanges: false);
        var appointmentsDto = _mapper.Map<PagedList<AppointmentDto>>(appointments);

        return Result<PagedList<AppointmentDto>>.Success(appointmentsDto);
    }

    /// <inheritdoc/>
    public async Task<Result<PagedList<AppointmentDto>>> GetMissedByUserIdAsync(PagingParameters paging, int userId)
    {
        var user = await _repository.ApplicationUser.GetApplicationUserAsync(userId);
        if (user is null)
            return Result<PagedList<AppointmentDto>>.Failure(ApplicationUserError.UserIdNotFound(userId));

        var appointments = await _repository.Appointment.GetAppointmentsByUserIdAsync(paging, userId, isVisited: false, trackChanges: false);
        var appointmentsDto = _mapper.Map<PagedList<AppointmentDto>>(appointments);

        return Result<PagedList<AppointmentDto>>.Success(appointmentsDto);
    }

    /// <inheritdoc/>
    public async Task<Result<PagedList<AppointmentDto>>> GetVisitedByUserIdAsync(PagingParameters paging, int userId)
    {
        var user = await _repository.ApplicationUser.GetApplicationUserAsync(userId);
        if (user is null)
            return Result<PagedList<AppointmentDto>>.Failure(ApplicationUserError.UserIdNotFound(userId));

        var appointments = await _repository.Appointment.GetAppointmentsByUserIdAsync(paging, userId, isVisited: true, trackChanges: false);
        var appointmentsDto = _mapper.Map<PagedList<AppointmentDto>>(appointments);

        return Result<PagedList<AppointmentDto>>.Success(appointmentsDto);
    }

    /// <inheritdoc/>
    public async Task<Result> MarkAsVisitedAsync(int appointmentId, string diagnosis)
    {
        var appointment = await _repository.Appointment.GetAppointmentAsync(appointmentId, true);
        if (appointment is null)
            return Result.Failure(AppointmentError.AppointmentNotFound(appointmentId));

        appointment.Diagnosis = diagnosis;
        appointment.IsVisited = true;
        await _repository.SaveAsync();

        return Result.Success();
    }
}