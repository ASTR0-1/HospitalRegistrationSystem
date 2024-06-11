using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Entities;

namespace HospitalRegistrationSystem.Application.Interfaces.Data;

/// <summary>
///     Represents a repository for managing appointments.
/// </summary>
public interface IAppointmentRepository
{
    /// <summary>
    ///     Retrieves a paged list of appointments by user ID.
    /// </summary>
    /// <param name="paging">The paging parameters.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="isVisited">Indicates whether the appointments have been visited. (Optional, if not passed - retrieves all)</param>
    /// <param name="trackChanges">Indicates whether to track changes in the appointments.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the paged list of appointments.</returns>
    Task<PagedList<Appointment>> GetAppointmentsByUserIdAsync(PagingParameters paging, int userId,
        bool? isVisited = null, bool trackChanges = false);

    /// <summary>
    ///     Retrieves a paged list of incoming appointments by user ID.
    /// </summary>
    /// <param name="paging">The paging parameters.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="trackChanges">Indicates whether to track changes in the appointments.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the paged list of incoming
    ///     appointments.
    /// </returns>
    Task<PagedList<Appointment>> GetIncomingAppointmentsByUserIdAsync(PagingParameters paging, int userId,
        bool trackChanges = false);

    /// <summary>
    ///     Retrieves an appointment by its ID.
    /// </summary>
    /// <param name="id">The ID of the appointment.</param>
    /// <param name="trackChanges">Indicates whether to track changes in the appointment.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the appointment, or null if not
    ///     found.
    /// </returns>
    Task<Appointment?> GetAppointmentAsync(int id, bool trackChanges = false);

    /// <summary>
    ///     Creates a new appointment.
    /// </summary>
    /// <param name="appointment">The appointment to create.</param>
    void CreateAppointment(Appointment appointment);

    /// <summary>
    ///     Deletes an appointment.
    /// </summary>
    /// <param name="appointment">The appointment to delete.</param>
    void DeleteAppointment(Appointment appointment);
}