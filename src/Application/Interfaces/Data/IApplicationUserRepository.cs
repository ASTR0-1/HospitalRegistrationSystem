using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Utility;
using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace HospitalRegistrationSystem.Application.Interfaces.Data;

/// <summary>
///     Represents the interface for accessing and manipulating application users in the data layer.
/// </summary>
public interface IApplicationUserRepository
{
    /// <summary>
    ///     Get application users by role asynchronously.
    /// </summary>
    /// <param name="paging">The paging parameters.</param>
    /// <param name="role">The role to filter by.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the paged list of application users.</returns>
    Task<PagedList<ApplicationUser>> GetApplicationUsersByRoleAsync(string role, PagingParameters paging);

    /// <summary>
    ///     Get an application user by user ID asynchronously.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the application user.</returns>
    Task<ApplicationUser?> GetApplicationUserAsync(int userId);

    /// <summary>
    ///     Update an application user.
    /// </summary>
    /// <param name="applicationUser">The application user to update.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the identity result of the update operation.</returns>
    Task<IdentityResult> UpdateApplicationUserAsync(ApplicationUser applicationUser);

    /// <summary>
    ///     Assign a user to a provided role asynchronously.
    /// </summary>
    /// <param name="user">The user to assign the role to.</param>
    /// <param name="role">The role to assign.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the identity result of the assignment operation.</returns>
    Task<IdentityResult> AssignUserToRoleAsync(ApplicationUser user, string role);
}
