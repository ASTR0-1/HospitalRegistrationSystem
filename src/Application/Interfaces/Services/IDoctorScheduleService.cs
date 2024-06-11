using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.DTOs.DoctorScheduleDTOs;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Shared.ResultPattern;

namespace HospitalRegistrationSystem.Application.Interfaces.Services;

/// <summary>
///     Represents a service for managing doctor schedules.
/// </summary>
public interface IDoctorScheduleService
{
    /// <summary>
    ///     Retrieves a list of doctor schedules asynchronously.
    /// </summary>
    /// <param name="parameters">The parameters for filtering and pagination.</param>
    /// <param name="doctorId">The ID of the doctor.</param>
    /// <returns>A paged list of doctor schedules.</returns>
    Task<Result<PagedList<DoctorScheduleDto>>> GetDoctorSchedulesAsync(DoctorScheduleParameters parameters,
        int doctorId);

    /// <summary>
    ///     Creates a new doctor schedule.
    /// </summary>
    /// <param name="doctorScheduleDto">The doctor schedule to create.</param>
    /// <returns>A result indicating the success or failure of the operation.</returns>
    Task<Result> CreateDoctorSchedule(DoctorScheduleForManipulationDto doctorScheduleDto);

    /// <summary>
    ///     Updates an existing doctor schedule.
    /// </summary>
    /// <param name="doctorScheduleDto">The doctor schedule to update.</param>
    /// <returns>A result indicating the success or failure of the operation.</returns>
    Task<Result> UpdateDoctorSchedule(DoctorScheduleForManipulationDto doctorScheduleDto);

    /// <summary>
    ///     Deletes a doctor schedule by ID.
    /// </summary>
    /// <param name="doctorScheduleId">The ID of the doctor schedule to delete.</param>
    /// <returns>A result indicating the success or failure of the operation.</returns>
    Task<Result> DeleteDoctorSchedule(int doctorScheduleId);
}