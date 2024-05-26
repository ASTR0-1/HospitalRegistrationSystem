using System;
using System.Collections.Generic;

namespace HospitalRegistrationSystem.Domain.Entities;

public class Appointment
{
    public int Id { get; set; }
    public DateTime VisitTime { get; set; }
    public string Diagnosis { get; set; }
    public bool IsVisited { get; set; }

    public ICollection<ApplicationUser> ApplicationUsers { get; set; } = [];

    public int? FeedbackId { get; set; }
    public Feedback? Feedback { get; set; }
}