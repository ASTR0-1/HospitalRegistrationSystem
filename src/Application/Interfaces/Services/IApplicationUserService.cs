using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.DTOs.ApplicationUserDTOs;
using HospitalRegistrationSystem.Application.Utility;
using HospitalRegistrationSystem.Domain.Shared;

namespace HospitalRegistrationSystem.Application.Interfaces.Services;

/// <summary>
///     Represents the interface for the application user service.
/// </summary>
public interface IApplicationUserService
{
    /// <summary>
    ///     Retrieves an application user by their ID.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the application user as a <see cref="Result{T}"/> where T is <see cref="ApplicationUserDto"/>.</returns>
    Task<Result<ApplicationUserDto>> GetAsync(int userId);

    /// <summary>
    ///     Retrieves all application users by their role.
    /// </summary>
    /// <param name="paging">The paging parameters.</param>
    /// <param name="role">The role of the users to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a paged list of application users as a <see cref="Result{T}"/> where T is <see cref="PagedList{ApplicationUserDto}"/>.</returns>
    Task<Result<PagedList<ApplicationUserDto>>> GetAllByRoleAsync(PagingParameters paging, string role);

    /// <summary>
    ///     Updates an application user.
    /// </summary>
    /// <param name="applicationUserDto">The application user DTO containing the updated information.</param>
    /// <returns>A task that represents the asynchronous operation. The task result indicates the success or failure of the update operation as a <see cref="Result"/>.</returns>
    Task<Result> UpdateAsync(ApplicationUserDto applicationUserDto);

    /// <summary>
    ///     Assigns a role to an application user.
    /// </summary>
    /// <param name="userId">The ID of the user to assign the role to.</param>
    /// <param name="role">The role to assign.</param>
    /// <returns>A task that represents the asynchronous operation. The task result indicates the success or failure of the assignment operation as a <see cref="Result"/>.</returns>
    Task<Result> AssignUserToRoleAsync(int userId, string role);
}
