using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Application.Utility;
using HospitalRegistrationSystem.Domain.Constants;
using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HospitalRegistrationSystem.Infrastructure.Persistence.Repositories;

/// <summary>
///     Represents a repository for accessing and manipulating application users in the data layer.
/// </summary>
public class ApplicationUserRepository : IApplicationUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ApplicationUserRepository" /> class.
    /// </summary>
    /// <param name="userManager">The user manager.</param>
    public ApplicationUserRepository(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    /// <inheritdoc />
    public async Task<PagedList<ApplicationUser>> GetApplicationUsersByRoleAsync(string role, PagingParameters paging)
    {
        var users = await _userManager.GetUsersInRoleAsync(role);
        var pagedUsers = PagedList<ApplicationUser>.ToPagedList(users, paging.PageNumber, paging.PageSize);

        return pagedUsers;
    }

    /// <inheritdoc />
    public async Task<ApplicationUser?> GetApplicationUserAsync(int userId) => await _userManager.FindByIdAsync(userId.ToString());

    /// <inheritdoc />
    public async Task<IdentityResult> UpdateApplicationUserAsync(ApplicationUser applicationUser) => await _userManager.UpdateAsync(applicationUser);

    /// <inheritdoc />
    public async Task<IdentityResult> AssignUserToRoleAsync(ApplicationUser user, string role) => await _userManager.AddToRoleAsync(user, role);
}
