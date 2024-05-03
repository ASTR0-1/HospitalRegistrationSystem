using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.DTOs.AuthenticationDTOs;
using HospitalRegistrationSystem.Domain.Entities;

namespace HospitalRegistrationSystem.Application.Interfaces.Identity;

/// <summary>
///     Represents an interface for managing authentication.
/// </summary>
public interface IAuthenticationManager
{
    /// <summary>
    ///     Validates a user asynchronously.
    /// </summary>
    /// <param name="userForAuthentication">The user to be validated.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean value indicating whether the user is valid or not.</returns>
    Task<bool> ValidateUserAsync(UserForAuthenticationDto userForAuthentication);

    /// <summary>
    ///     Creates a token asynchronously.
    /// </summary>
    /// <param name="user">The user for whom the token is created. If null, a token will be created without associating it with any user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created token as a string.</returns>
    Task<string> CreateTokenAsync(ApplicationUser? user = null);
}
