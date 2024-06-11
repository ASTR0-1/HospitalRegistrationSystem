using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HospitalRegistrationSystem.Application.DTOs.ApplicationUserDTOs;
using HospitalRegistrationSystem.Application.DTOs.AppointmentDTOs;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Application.Interfaces.Services;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Constants;
using HospitalRegistrationSystem.Domain.Entities;
using HospitalRegistrationSystem.Domain.Errors;
using HospitalRegistrationSystem.Domain.Shared.ResultPattern;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace HospitalRegistrationSystem.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _repository;
    private readonly UserManager<ApplicationUser> _userManager;

    public AppointmentService(IRepositoryManager repository, IMapper mapper, IConfiguration configuration,
        UserManager<ApplicationUser> userManager)
    {
        _repository = repository;
        _mapper = mapper;
        _configuration = configuration;
        _userManager = userManager;
    }

    /// <inheritdoc />
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

        var isDoctorInRole =
            await _repository.ApplicationUser.CheckUserInRoleAsync(appointmentDto.DoctorId, RoleConstants.Doctor);
        if (!isDoctorInRole)
            return Result.Failure(AppointmentError.DoctorNotInRole(appointmentDto.DoctorId));

        if (doctor.DoctorSchedules is null || doctor.DoctorSchedules.Count == 0)
            return Result.Failure(AppointmentError.DoctorHasNoSchedule(appointmentDto.DoctorId));

        var dateIsFree = doctor.DoctorSchedules.Any(ds => ds.Date == DateOnly.FromDateTime(appointmentDto.VisitTime));
        var timeIsFree = doctor.DoctorSchedules.Any(ds =>
            DecodeWorkingHours(ds.WorkingHours).Contains(appointmentDto.VisitTime.Hour));
        if (!dateIsFree || !timeIsFree)
            return Result.Failure(
                AppointmentError.AppointmentTimeIsNotAvailable(appointmentDto.DoctorId, appointmentDto.VisitTime));

        var doctorSchedule =
            doctor.DoctorSchedules.First(ds => ds.Date == DateOnly.FromDateTime(appointmentDto.VisitTime));
        _repository.DoctorSchedule.UpdateDoctorSchedule(doctorSchedule);

        doctorSchedule.WorkingHours &= ~(1 << appointmentDto.VisitTime.Hour);

        appointment.ApplicationUsers.Add(client);
        appointment.ApplicationUsers.Add(doctor);

        _repository.Appointment.CreateAppointment(appointment);
        await _repository.SaveAsync();

        return Result.Success();
    }

    /// <inheritdoc />
    public async Task<Result<AppointmentDto>> GetAsync(int appointmentId)
    {
        var appointment = await _repository.Appointment.GetAppointmentAsync(appointmentId);
        if (appointment is null)
            return Result<AppointmentDto>.Failure(AppointmentError.AppointmentNotFound(appointmentId));

        var appointmentDto = _mapper.Map<AppointmentDto>(appointment);
        await ManageUsers(appointment, appointmentDto);

        return Result<AppointmentDto>.Success(appointmentDto);
    }

    /// <inheritdoc />
    public async Task<Result<PagedList<AppointmentDto>>> GetIncomingByUserIdAsync(PagingParameters paging, int userId)
    {
        var user = await _repository.ApplicationUser.GetApplicationUserAsync(userId);
        if (user is null)
            return Result<PagedList<AppointmentDto>>.Failure(ApplicationUserError.UserIdNotFound(userId));

        var appointments = await _repository.Appointment.GetIncomingAppointmentsByUserIdAsync(paging, userId, false);
        List<AppointmentDto> appointmentsDto = [];

        foreach (var appointment in appointments)
        {
            var appointmentDto = _mapper.Map<AppointmentDto>(appointment);
            await ManageUsers(appointment, appointmentDto);
            appointmentsDto.Add(appointmentDto);
        }

        var pagedAppointmentsDto =
            new PagedList<AppointmentDto>(appointmentsDto, appointmentsDto.Count, paging.PageNumber, paging.PageSize);
        await FillTotalServiceCost(pagedAppointmentsDto);

        return Result<PagedList<AppointmentDto>>.Success(pagedAppointmentsDto);
    }

    /// <inheritdoc />
    public async Task<Result<PagedList<AppointmentDto>>> GetAllByUserIdAsync(PagingParameters paging, int userId)
    {
        var user = await _repository.ApplicationUser.GetApplicationUserAsync(userId);
        if (user is null)
            return Result<PagedList<AppointmentDto>>.Failure(ApplicationUserError.UserIdNotFound(userId));

        var appointments = await _repository.Appointment.GetAppointmentsByUserIdAsync(paging, userId, null, false);
        List<AppointmentDto> appointmentsDto = [];

        foreach (var appointment in appointments)
        {
            var appointmentDto = _mapper.Map<AppointmentDto>(appointment);
            await ManageUsers(appointment, appointmentDto);
            appointmentsDto.Add(appointmentDto);
        }

        var pagedAppointmentsDto =
            new PagedList<AppointmentDto>(appointmentsDto, appointmentsDto.Count, paging.PageNumber, paging.PageSize);
        await FillTotalServiceCost(pagedAppointmentsDto);

        return Result<PagedList<AppointmentDto>>.Success(pagedAppointmentsDto);
    }

    /// <inheritdoc />
    public async Task<Result<PagedList<AppointmentDto>>> GetMissedByUserIdAsync(PagingParameters paging, int userId)
    {
        var user = await _repository.ApplicationUser.GetApplicationUserAsync(userId);
        if (user is null)
            return Result<PagedList<AppointmentDto>>.Failure(ApplicationUserError.UserIdNotFound(userId));

        var appointments = await _repository.Appointment.GetAppointmentsByUserIdAsync(paging, userId, false, false);
        List<AppointmentDto> appointmentsDto = [];

        foreach (var appointment in appointments)
        {
            var appointmentDto = _mapper.Map<AppointmentDto>(appointment);
            await ManageUsers(appointment, appointmentDto);
            appointmentsDto.Add(appointmentDto);
        }

        var pagedAppointmentsDto =
            new PagedList<AppointmentDto>(appointmentsDto, appointmentsDto.Count, paging.PageNumber, paging.PageSize);
        await FillTotalServiceCost(pagedAppointmentsDto);

        return Result<PagedList<AppointmentDto>>.Success(pagedAppointmentsDto);
    }

    /// <inheritdoc />
    public async Task<Result<PagedList<AppointmentDto>>> GetVisitedByUserIdAsync(PagingParameters paging, int userId)
    {
        var user = await _repository.ApplicationUser.GetApplicationUserAsync(userId);
        if (user is null)
            return Result<PagedList<AppointmentDto>>.Failure(ApplicationUserError.UserIdNotFound(userId));

        var appointments = await _repository.Appointment.GetAppointmentsByUserIdAsync(paging, userId, true, false);
        List<AppointmentDto> appointmentsDto = [];

        foreach (var appointment in appointments)
        {
            var appointmentDto = _mapper.Map<AppointmentDto>(appointment);
            await ManageUsers(appointment, appointmentDto);
            appointmentsDto.Add(appointmentDto);
        }

        var pagedAppointmentsDto =
            new PagedList<AppointmentDto>(appointmentsDto, appointmentsDto.Count, paging.PageNumber, paging.PageSize);
        await FillTotalServiceCost(pagedAppointmentsDto);

        return Result<PagedList<AppointmentDto>>.Success(pagedAppointmentsDto);
    }

    /// <inheritdoc />
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

    private List<int> DecodeWorkingHours(int workingHours)
    {
        var hours = new List<int>();

        for (var hour = 0; hour < 24; hour++)
            if ((workingHours & (1 << hour)) != 0)
                hours.Add(hour);

        return hours;
    }

    private async Task FillTotalServiceCost(PagedList<AppointmentDto> appointmentsDto)
    {
        foreach (var appointment in appointmentsDto)
        {
            var hospital = await _repository.Hospital.GetHospitalAsync((int) appointment.Doctor.HospitalId!);
            appointment.Doctor.TotalServiceCost =
                CalculateTotalServiceCost(appointment.Doctor.VisitCost.GetValueOrDefault(),
                    hospital.HospitalFeePercent);
        }
    }

    private async Task ManageUsers(Appointment appointment, AppointmentDto appointmentDto)
    {
        var u1 = await _repository.ApplicationUser.GetApplicationUserAsync(
            appointment.ApplicationUsers.FirstOrDefault()!.Id)!;
        var u2 = await _repository.ApplicationUser.GetApplicationUserAsync(appointment.ApplicationUsers.LastOrDefault()!
            .Id)!;

        if (await _userManager.IsInRoleAsync(u1, RoleConstants.Doctor))
        {
            appointmentDto.Doctor = _mapper.Map<ApplicationUserDto>(u1);
            appointmentDto.Client = _mapper.Map<ApplicationUserDto>(u2);
        }
        else
        {
            appointmentDto.Doctor = _mapper.Map<ApplicationUserDto>(u2);
            appointmentDto.Client = _mapper.Map<ApplicationUserDto>(u1);
        }
    }

    private decimal CalculateTotalServiceCost(decimal visitCost, decimal hospitalFeePercent)
    {
        var systemFeeSection = _configuration.GetSection("SystemSettings:SystemFeePercent");
        var systemFeeString = systemFeeSection.Value;
        if (!decimal.TryParse(systemFeeString, NumberStyles.Float, CultureInfo.InvariantCulture,
                out var systemFeePercent))
            throw new Exception("Invalid system fee value");

        var totalServiceCost = visitCost + visitCost * hospitalFeePercent / 100 + visitCost * systemFeePercent / 100;

        return totalServiceCost;
    }
}