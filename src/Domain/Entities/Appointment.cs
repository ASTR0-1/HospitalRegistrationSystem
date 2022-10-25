using System;

namespace HospitalRegistrationSystem.Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }

        public DateTime VisitTime { get; set; }

        public string Diagnosis { get; set; }

        public bool IsVisited { get; set; } = false;

        public int DoctorId { get; set; }

        public Doctor Doctor { get; set; }

        public int ClientId { get; set; }

        public Client Client { get; set; }
    }
}
