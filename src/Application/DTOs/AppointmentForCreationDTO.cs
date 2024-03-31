using System;

namespace HospitalRegistrationSystem.Application.DTOs;

public class AppointmentForCreationDTO
{
    public DateTime VisitTime { get; set; }

    public int DoctorId { get; set; }

    public int ClientId { get; set; }
}