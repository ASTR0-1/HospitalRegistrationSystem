using System;

namespace HospitalRegistrationSystem.Application.DTOs.FeedbackDTOs;

public class FeedbackDto
{
    public decimal Rating { get; set; }
    public string Text { get; set; }

    public int AppointmentId { get; set; }
    public DateTime FeedbackDate { get; set; }
}