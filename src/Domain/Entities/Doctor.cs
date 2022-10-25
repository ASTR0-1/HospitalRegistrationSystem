using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HospitalRegistrationSystem.Domain.Entities
{
    public class Doctor
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public string Specialty { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new Collection<Appointment>();
    }
}