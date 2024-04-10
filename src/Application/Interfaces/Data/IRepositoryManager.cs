using System.Threading.Tasks;

namespace HospitalRegistrationSystem.Application.Interfaces.Data;

public interface IRepositoryManager
{
    IAppointmentRepository Appointment { get; }

    Task SaveAsync();
}