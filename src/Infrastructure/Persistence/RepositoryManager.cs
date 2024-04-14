using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Infrastructure.Persistence.Repositories;

namespace HospitalRegistrationSystem.Infrastructure.Persistence;

public class RepositoryManager : IRepositoryManager
{
    private static readonly object Lock = new();
    private readonly RepositoryContext _context;

    private IAppointmentRepository _appointmentRepository;

    public RepositoryManager(RepositoryContext context)
    {
        _context = context;
    }

    public IAppointmentRepository Appointment
    {
        get
        {
            lock (Lock)
            {
                _appointmentRepository ??= new AppointmentRepository(_context);

                return _appointmentRepository;
            }
        }
    }

    public Task SaveAsync()
    {
        return _context.SaveChangesAsync();
    }
}