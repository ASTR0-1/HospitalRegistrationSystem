using System.Threading.Tasks;

namespace HospitalRegistrationSystem.Application.Interfaces.Data;

/// <summary>
///     Represents a repository manager interface.
/// </summary>
public interface IRepositoryManager
{
    /// <summary>
    ///     Gets the appointment repository.
    /// </summary>
    IAppointmentRepository Appointment { get; }

    /// <summary>
    ///     Gets the application user repository.
    /// </summary>
    IApplicationUserRepository ApplicationUser { get; }

    /// <summary>
    ///     Saves changes made in database asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous save operation.</returns>
    Task SaveAsync();
}
