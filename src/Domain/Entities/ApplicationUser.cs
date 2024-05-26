using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace HospitalRegistrationSystem.Domain.Entities;

/// <summary>
///     Represents an application user.
/// </summary>
public class ApplicationUser : IdentityUser<int>
{
    /// <summary>
    ///     Gets or sets the first name of the user.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    ///     Gets or sets the middle name of the user.
    /// </summary>
    public string? MiddleName { get; set; }

    /// <summary>
    ///     Gets or sets the last name of the user.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    ///     Gets or sets the gender of the user.
    /// </summary>
    public string Gender { get; set; }

    /// <summary>
    ///     Gets or sets the specialty of the user.
    /// </summary>
    /// <remarks>Applies only to doctor role.</remarks>
    public string? Specialty { get; set; }

    /// <summary>
    ///     Gets or sets the profile photo URL of the user.
    /// </summary>
    public string? ProfilePhotoUrl { get; set; }

    /// <summary>
    ///     Gets or sets the appointments of the user.
    /// </summary>
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    /// <summary>
    ///     Gets or sets the ID of the hospital associated with the user.
    /// </summary>
    /// <remarks>Applies only to doctor role.</remarks>
    public int? HospitalId { get; set; }

    /// <summary>
    ///     Gets or sets the hospital associated with the user.
    /// </summary>
    /// <remarks>Applies only to doctor role.</remarks>
    public Hospital? Hospital { get; set; }

    /// <summary>
    ///     Gets or sets the doctor visit cost.
    /// </summary>
    /// <remarks>Applies only to doctor role.</remarks>
    public decimal? VisitCost { get; set; }

    /// <summary>
    ///     Gets or sets the doctor schedules associated with the user.
    /// </summary>
    /// <remarks>Applies only to doctor role.</remarks>
    public ICollection<DoctorSchedule>? DoctorSchedules { get; set; }

    /// <summary>
    ///     Gets or sets the refresh token of the user.
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    ///     Gets or sets the expiry time of the refresh token.
    /// </summary>
    public DateTime? RefreshTokenExpiryTime { get; set; }
}
