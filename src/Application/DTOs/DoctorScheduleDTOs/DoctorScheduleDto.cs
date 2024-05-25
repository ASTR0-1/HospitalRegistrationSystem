using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalRegistrationSystem.Application.DTOs.DoctorScheduleDTOs;

/// <summary>
///     Represents a Doctor Schedule Data Transfer Object.
/// </summary>
public class DoctorScheduleDto
{
    /// <summary>
    ///     Gets or sets the ID of the doctor schedule.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     Gets or sets the ID of the doctor.
    /// </summary>
    public int DoctorId { get; set; }

    /// <summary>
    ///     Gets or sets the list of working hours.
    /// </summary>
    public required List<int> WorkingHoursList { get; set; }

    /// <summary>
    ///     Gets or sets the date of the doctor schedule.
    /// </summary>
    public DateOnly Date { get; set; }
}
