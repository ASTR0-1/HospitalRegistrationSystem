using System;

namespace HospitalRegistrationSystem.Application.Utility.PagedData;

/// <summary>
///     Represents the parameters for retrieving doctor schedules.
/// </summary>
public sealed class DoctorScheduleParameters : PagingParameters
{
    /// <summary>
    ///     Gets or sets the start date for the schedule.
    /// </summary>
    public DateOnly From { get; set; }

    /// <summary>
    ///     Gets or sets the end date for the schedule.
    /// </summary>
    public DateOnly To { get; set; }
}