using System.Security.AccessControl;
using System.Threading.Tasks;
using AutoMapper;
using HospitalRegistrationSystem.Application.DTOs.DoctorScheduleDTOs;
using HospitalRegistrationSystem.Application.Interfaces;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Application.Interfaces.Services;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Entities;
using HospitalRegistrationSystem.Domain.Errors;
using HospitalRegistrationSystem.Domain.Shared.ResultPattern;

namespace HospitalRegistrationSystem.Application.Services;

/// <summary>
///     Service class for managing doctor schedules.
/// </summary>
public class DoctorScheduleService : IDoctorScheduleService
{
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _repository;
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DoctorScheduleService"/> class.
    /// </summary>
    /// <param name="mapper">The mapper.</param>
    /// <param name="repository">The repository.</param>
    /// <param name="currentUserService">The current user service.</param>
    public DoctorScheduleService(IMapper mapper, IRepositoryManager repository, ICurrentUserService currentUserService)
    {
        _mapper = mapper;
        _repository = repository;
        _currentUserService = currentUserService;
    }

    /// <inheritdoc />
    public async Task<Result<PagedList<DoctorScheduleDto>>> GetDoctorSchedulesAsync(DoctorScheduleParameters parameters, int doctorId)
    {
        var doctor = await _repository.ApplicationUser.GetApplicationUserAsync(doctorId);
        if (doctor is null)
            return Result<PagedList<DoctorScheduleDto>>.Failure(ApplicationUserError.UserIdNotFound(doctorId));

        var doctorSchedules = await _repository.DoctorSchedule.GetDoctorSchedulesAsync(parameters, doctorId);
        var doctorSchedulesDto = _mapper.Map<PagedList<DoctorScheduleDto>>(doctorSchedules);

        return Result<PagedList<DoctorScheduleDto>>.Success(doctorSchedulesDto);
    }

    /// <inheritdoc />
    public async Task<Result> CreateDoctorSchedule(DoctorScheduleForManipulationDto doctorScheduleDto)
    {
        var callerId = _currentUserService.GetApplicationUserId();
        var doctorSchedule = _mapper.Map<DoctorSchedule>(doctorScheduleDto);
        doctorSchedule.DoctorId = callerId;

        _repository.DoctorSchedule.CreateDoctorSchedule(doctorSchedule);
        await _repository.SaveAsync();

        return Result.Success();
    }

    /// <inheritdoc />
    public async Task<Result> UpdateDoctorSchedule(DoctorScheduleForManipulationDto doctorScheduleDto)
    {
        var doctorSchedule = await _repository.DoctorSchedule.GetDoctorScheduleByIdAsync(doctorScheduleDto.Id);
        if (doctorSchedule is null)
            return Result.Failure(DoctorScheduleError.DoctorScheduleIdNotFound(doctorScheduleDto.Id));

        var callerId = _currentUserService.GetApplicationUserId();
        if (callerId != doctorSchedule!.DoctorId)
            return Result.Failure(DoctorScheduleError.UnauthorizedAccessToDoctorScheduleManipulation(callerId, doctorSchedule.Id));

        doctorSchedule = _mapper.Map(doctorScheduleDto, doctorSchedule);

        _repository.DoctorSchedule.UpdateDoctorSchedule(doctorSchedule);
        await _repository.SaveAsync();

        return Result.Success();
    }

    /// <inheritdoc />
    public async Task<Result> DeleteDoctorSchedule(int doctorScheduleId)
    {
        var doctorSchedule = await _repository.DoctorSchedule.GetDoctorScheduleByIdAsync(doctorScheduleId);
        if (doctorSchedule is null)
            return Result.Failure(DoctorScheduleError.DoctorScheduleIdNotFound(doctorScheduleId));

        var callerId = _currentUserService.GetApplicationUserId();
        if (callerId != doctorSchedule.DoctorId)
            return Result.Failure(DoctorScheduleError.UnauthorizedAccessToDoctorScheduleManipulation(callerId, doctorScheduleId));

        _repository.DoctorSchedule.DeleteDoctorSchedule(doctorSchedule);
        await _repository.SaveAsync();

        return Result.Success();
    }
}
