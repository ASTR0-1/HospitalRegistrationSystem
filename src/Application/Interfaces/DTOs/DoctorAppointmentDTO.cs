using System;

namespace HospitalRegistrationSystem.Application.Interfaces.DTOs
{
    public class DoctorAppointmentDTO
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public string ClientFirstName { get; set; }

        public string ClientMiddleName { get; set; }

        public string ClientLastName { get; set; }

        public string ClientGender { get; set; }

        public DateTime VisitTime { get; set; }
    }
}
