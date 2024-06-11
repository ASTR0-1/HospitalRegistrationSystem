using System;
using System.Collections.Generic;

namespace HospitalRegistrationSystem.Application.DTOs.DoctorScheduleDTOs;

/// <summary>
///     Represents a DTO for creating a doctor schedule.
/// </summary>
public class DoctorScheduleForManipulationDto
{
    /// <summary>
    ///     Gets or sets the ID of the doctors' schedule.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     Gets or sets the date.
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    ///     Gets or sets the working hours list.
    /// </summary>
    public List<int> WorkingHoursList { get; set; }
}