using System.Collections.Generic;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Domain.Entities;

namespace HospitalRegistrationSystem.Application.Interfaces.Data
{
    public interface IAppointmentRepository
    {
        Task<IEnumerable<Appointment>> GetAppointmentsAsync(bool trackChanges);
        Task<Appointment> GetAppointmentAsync(int id, bool trackChanges);
        void CreateAppointment(Appointment appointment);
        void DeleteAppointment(Appointment appointment);
    }
}