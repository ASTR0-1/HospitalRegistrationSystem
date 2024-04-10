using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HospitalRegistrationSystem.Domain.Entities;

public class Appointment
{
    public Guid Id { get; set; }
    public DateTime VisitTime { get; set; }
    public string Diagnosis { get; set; }
    public bool IsVisited { get; set; } = false;

    public ICollection<ApplicationUser> ApplicationUsers { get; set; } = new List<ApplicationUser>();
}