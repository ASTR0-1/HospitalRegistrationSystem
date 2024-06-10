using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.DTOs.AppointmentDTOs;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Shared.ResultPattern;

namespace HospitalRegistrationSystem.Application.Interfaces.Services;

/// <summary>
///     Represents the interface for appointment service.
/// </summary>
public interface IAppointmentService
{
    /// <summary>
    ///     Adds a new appointment asynchronously.
    /// </summary>
    /// <param name="appointmentDto"></param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<Result> AddNewAsync(AppointmentForCreationDto appointmentDto);

    /// <summary>
    ///     Marks an appointment as visited asynchronously.
    /// </summary>
    /// <param name="appointmentId">The ID of the appointment.</param>
    /// <param name="diagnosis">The diagnosis for the appointment.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<Result> MarkAsVisitedAsync(int appointmentId, string diagnosis);

    /// <summary>
    ///     Gets all appointments by user ID asynchronously.
    /// </summary>
    /// <param name="paging">The paging parameters.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<Result<PagedList<AppointmentDto>>> GetAllByUserIdAsync(PagingParameters paging, int userId);

    /// <summary>
    ///     Gets an appointment by ID asynchronously.
    /// </summary>
    /// <param name="appointmentId">The ID of the appointment.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<Result<AppointmentDto>> GetAsync(int appointmentId);

    /// <summary>
    ///     Gets incoming appointments by user ID asynchronously.
    /// </summary>
    /// <param name="paging">The paging parameters.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<Result<PagedList<AppointmentDto>>> GetIncomingByUserIdAsync(PagingParameters paging, int userId);

    /// <summary>
    ///     Gets missed appointments by user ID asynchronously.
    /// </summary>
    /// <param name="paging">The paging parameters.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<Result<PagedList<AppointmentDto>>> GetMissedByUserIdAsync(PagingParameters paging, int userId);

    /// <summary>
    ///     Gets visited appointments by user ID asynchronously.
    /// </summary>
    /// <param name="paging">The paging parameters.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<Result<PagedList<AppointmentDto>>> GetVisitedByUserIdAsync(PagingParameters paging, int userId);
}