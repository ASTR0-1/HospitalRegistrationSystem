using System;

namespace HospitalRegistrationSystem.Domain.Entities;

public class Feedback
{
    public int Id { get; set; }

    public decimal Rating { get; set; }
    public string Text { get; set; }

    public int ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; } = null!;

    public int AppointmentId { get; set; }
    public Appointment Appointment { get; set; } = null!;

    public DateTime FeedbackDate { get; set; }
}