using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Identity;

namespace HospitalRegistrationSystem.Domain.Entities;

public class ApplicationUser : IdentityUser<int>
{
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }

    public string Gender { get; set; }

    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
