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
    ///     Gets the country repository.
    /// </summary>
    ICountryRepository Country { get; }

    /// <summary>
    ///     Gets the region repository.
    /// </summary>
    IRegionRepository Region { get; }

    /// <summary>
    ///     Gets the city repository.
    /// </summary>
    ICityRepository City { get; }

    /// <summary>
    ///     Gets the hospital repository.
    /// </summary>
    IHospitalRepository Hospital { get; }

    /// <summary>
    ///     Gets the doctor schedule repository.
    /// </summary>
    IDoctorScheduleRepository DoctorSchedule { get; }

    /// <summary>
    ///     Gets the feedback repository.
    /// </summary>
    IFeedbackRepository Feedback { get; }

    /// <summary>
    ///     Saves changes made in database asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous save operation.</returns>
    Task SaveAsync();
}