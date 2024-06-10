namespace HospitalRegistrationSystem.Application.Interfaces;

/// <summary>
///     Represents the current user service.
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    ///     Gets the application user ID.
    /// </summary>
    /// <returns>The application user ID.</returns>
    public int GetApplicationUserId();

    /// <summary>
    ///     Gets the application user's hospital ID.
    /// </summary>
    /// <returns>The application user's hospital ID</returns>
    public int? GetApplicationUserHospitalId();

    /// <summary>
    ///     Checks if the current user is in the specified role.
    /// </summary>
    /// <param name="role">The role to check.</param>
    /// <returns>True if the current user is in the specified role, otherwise false.</returns>
    public bool IsInRole(string role);
}