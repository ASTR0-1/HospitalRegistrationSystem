using HospitalRegistrationSystem.Domain.Entities;
using System;

namespace HospitalRegistrationSystem.Application.Interfaces.DTOs
{
    public class AppointmentForCreationDTO
    {
        public DateTime VisitTime { get; set; }

        public string Diagnosis { get; set; }

        public int DoctorId { get; set; }

        public int ClientId { get; set; }
    }
}
