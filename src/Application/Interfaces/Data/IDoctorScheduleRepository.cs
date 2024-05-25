using System.Collections.Generic;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Entities;

namespace HospitalRegistrationSystem.Application.Interfaces.Data;

/// <summary>
///     Represents a repository for managing doctor schedules.
/// </summary>
public interface IDoctorScheduleRepository
{
    /// <summary>
    ///     Retrieves the doctor schedules asynchronously.
    /// </summary>
    /// <param name="parameters">The paging parameters with additional info to retrieve data.</param>
    /// <param name="doctorId">The ID of the doctor.</param>
    /// <param name="trackChanges">Indicates whether to track changes in the doctor schedules.</param>
    /// <returns>A collection of doctor schedules.</returns>
    Task<PagedList<DoctorSchedule>> GetDoctorSchedulesAsync(DoctorScheduleParameters parameters, int doctorId,
        bool trackChanges = false);

    /// <summary>
    ///     Retrieves a doctor schedule by ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the doctor schedule.</param>
    /// <param name="trackChanges">Indicates whether to track changes in the doctor schedule.</param>
    /// <returns>The doctor schedule with the specified ID.</returns>
    Task<DoctorSchedule?> GetDoctorScheduleByIdAsync(int id, bool trackChanges = false);

    /// <summary>
    ///     Creates a new doctor schedule asynchronously.
    /// </summary>
    /// <param name="doctorSchedule">The doctor schedule to create.</param>
    void CreateDoctorSchedule(DoctorSchedule doctorSchedule);

    /// <summary>
    ///     Updates an existing doctor schedule asynchronously.
    /// </summary>
    /// <param name="doctorSchedule">The doctor schedule to update.</param>
    void UpdateDoctorSchedule(DoctorSchedule doctorSchedule);

    /// <summary>
    ///     Deletes a doctor schedule asynchronously.
    /// </summary>
    /// <param name="doctorSchedule">The doctor schedule to delete.</param>
    void DeleteDoctorSchedule(DoctorSchedule doctorSchedule);
}
