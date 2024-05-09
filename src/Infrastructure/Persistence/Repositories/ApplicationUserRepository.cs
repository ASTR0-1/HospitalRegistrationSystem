using System;
using System.Linq;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Application.Utility;
using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;

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

    /// <inheritdoc/>
    public async Task<PagedList<ApplicationUser>> GetApplicationUsersAsync(PagingParameters paging, string? searchQuery, string role, int? hospitalId = null)
    {
        var users = await _userManager.GetUsersInRoleAsync(role);
        if (hospitalId is not null)
        {
            users = users.Where(u => u.HospitalId == hospitalId)
                .ToList();
        }

        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            var searchTerms = searchQuery.Split(' ')
                .Select(term => term.ToLower().Trim())
                .ToList();

            users = users.Where(u => searchTerms.Any(term => 
                    u.FirstName.Equals(term, StringComparison.InvariantCultureIgnoreCase) ||
                    (u.MiddleName is not null && u.MiddleName.Equals(term, StringComparison.InvariantCultureIgnoreCase)) ||
                    u.LastName.Equals(term, StringComparison.InvariantCultureIgnoreCase) ||
                    (u.Specialty is not null && u.Specialty.Equals(term, StringComparison.InvariantCultureIgnoreCase))
                )).ToList();
        }

        var pagedUsers = PagedList<ApplicationUser>.ToPagedList(users, paging.PageNumber, paging.PageSize);

        return pagedUsers;
    }

    /// <inheritdoc/>
    public async Task<ApplicationUser?> GetApplicationUserAsync(int userId) => await _userManager.FindByIdAsync(userId.ToString());

    /// <inheritdoc/>
    public async Task<IdentityResult> UpdateApplicationUserAsync(ApplicationUser applicationUser) => await _userManager.UpdateAsync(applicationUser);

    /// <inheritdoc/>
    public async Task<IdentityResult> AssignUserToRoleAsync(ApplicationUser user, string role) => await _userManager.AddToRoleAsync(user, role);
}
