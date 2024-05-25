using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.DTOs.AuthenticationDTOs;
using HospitalRegistrationSystem.Application.DTOs.TokenDTOs;
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
    /// <param name="populateExpiration"></param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created token as a string.</returns>
    Task<TokenDto> CreateTokenAsync(ApplicationUser? user = null, bool populateExpiration = false);

    /// <summary>
    ///     Refreshes a token asynchronously.
    /// </summary>
    /// <param name="tokenDto">The token to be refreshed.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the refreshed token as a string.</returns>
    Task<TokenDto> RefreshTokenAsync(TokenDto tokenDto);
}
