using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Domain.Entities;
using HospitalRegistrationSystem.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;

namespace HospitalRegistrationSystem.Infrastructure.Persistence;

/// <summary>
///     Represents a repository manager for managing data access.
/// </summary>
public class RepositoryManager : IRepositoryManager
{
    private static readonly object Lock = new();

    private readonly ApplicationContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    private IAppointmentRepository? _appointmentRepository;
    private IApplicationUserRepository? _applicationUserRepository;

    /// <summary>
    ///     Initializes a new instance of the <see cref="RepositoryManager"/> class.
    /// </summary>
    /// <param name="context">The repository context.</param>
    /// <param name="userManager">The user manager.</param>
    public RepositoryManager(ApplicationContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    /// <summary>
    ///     Gets the appointment repository.
    /// </summary>
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

    /// <summary>
    ///     Gets the application user repository.
    /// </summary>
    public IApplicationUserRepository ApplicationUser
    {
        get
        {
            lock (Lock)
            {
                _applicationUserRepository ??= new ApplicationUserRepository(_userManager);

                return _applicationUserRepository;
            }
        }
    }
    
    /// <summary>
    ///     Saves the changes made in database asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous save operation.</returns>
    public Task SaveAsync() => _context.SaveChangesAsync();
}
