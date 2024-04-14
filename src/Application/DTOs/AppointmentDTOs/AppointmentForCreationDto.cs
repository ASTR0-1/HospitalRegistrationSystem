using System;

namespace HospitalRegistrationSystem.Application.DTOs.AppointmentDTOs;

public class AppointmentForCreationDto
{
    public DateTime VisitTime { get; set; }

    public int DoctorId { get; set; }
    public int ClientId { get; set; }
}