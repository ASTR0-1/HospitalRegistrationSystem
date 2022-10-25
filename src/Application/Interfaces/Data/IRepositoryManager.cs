using System.Threading.Tasks;

namespace HospitalRegistrationSystem.Application.Interfaces.Data
{
    public interface IRepositoryManager
    {
        IAppointmentRepository Appointment { get; }
        IClientRepository Client { get; }
        IDoctorRepository Doctor { get; }

        Task SaveAsync();
    }
}
