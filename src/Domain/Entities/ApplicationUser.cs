using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace HospitalRegistrationSystem.Domain.Entities;

public class ApplicationUser : IdentityUser<int>
{
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string LastName { get; set; }

    public string Gender { get; set; }

    public string? Specialty { get; set; }

    public string? ProfilePhotoUrl { get; set; }

    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public int? HospitalId { get; set; }
    public Hospital? Hospital { get; set; }

    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
}