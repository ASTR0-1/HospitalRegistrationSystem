namespace HospitalRegistrationSystem.Application.DTOs.FeedbackDTOs;

public class FeedbackForCreationDto
{
    public decimal Rating { get; set; }
    public string Text { get; set; }
    public int AppointmentId { get; set; }
    public int ApplicationUserId { get; set; }
}