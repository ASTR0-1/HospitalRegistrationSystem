using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Infrastructure.Persistence.Repositories;

namespace HospitalRegistrationSystem.Infrastructure.Persistence;

public class RepositoryManager : IRepositoryManager
{
    private static readonly object _lock = new();
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
            lock (_lock)
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