using System;

namespace HospitalRegistrationSystem.Application.DTOs;

public class ClientAppointmentDTO
{
    public int Id { get; set; }

    public int DoctorId { get; set; }

    public DateTime VisitTime { get; set; }

    public string DoctorFirstName { get; set; }

    public string DoctorMiddleName { get; set; }

    public string DoctorLastName { get; set; }

    public string DoctorSpecialty { get; set; }

    public string DoctorGender { get; set; }
}
