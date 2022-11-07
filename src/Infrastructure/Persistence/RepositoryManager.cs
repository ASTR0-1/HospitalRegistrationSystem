using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Infrastructure.Persistence.Repositories;

namespace HospitalRegistrationSystem.Infrastructure.Persistence;

public class RepositoryManager : IRepositoryManager
{
    private readonly RepositoryContext _context;

    private IClientRepository _clientRepository;
    private IDoctorRepository _doctorRepository;
    private IAppointmentRepository _appointmentRepository;

    public RepositoryManager(RepositoryContext context)
    {
        _context = context;
    }

    public IAppointmentRepository Appointment
    {
        get
        {
            _appointmentRepository ??= new AppointmentRepository(_context);

            return _appointmentRepository;
        }
    }

    public IClientRepository Client
    {
        get
        {
            _clientRepository ??= new ClientRepository(_context);

            return _clientRepository;
        }
    }

    public IDoctorRepository Doctor
    {
        get
        {
            _doctorRepository ??= new DoctorRepository(_context);

            return _doctorRepository;
        }
    }

    public Task SaveAsync() =>
        _context.SaveChangesAsync();
}
