using System.Security.Claims;
using HospitalRegistrationSystem.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace HospitalRegistrationSystem.WebAPI.Services;

/// <summary>
/// Represents the application user service.
/// </summary>
public class CurrentUserService : ICurrentUserService
{
    private readonly ClaimsPrincipal _user;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CurrentUserService"/> class.
    /// </summary>
    /// <param name="httpContextAccessor">The HTTP context accessor.</param>
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _user = httpContextAccessor.HttpContext!.User;
    }

    /// <inheritdoc />
    public int GetApplicationUserId() => int.Parse(_user.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <inheritdoc />
    public bool IsInRole(string role) => _user.IsInRole(role);
}
