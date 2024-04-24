using System.Linq;
using System.Reflection;

namespace HospitalRegistrationSystem.Domain.Constants;

/// <summary>
///     Represents a static class that contains constants for different roles in the system.
/// </summary>
public static class RoleConstants
{
    /// <summary>
    ///     Represents the role of a doctor.
    /// </summary>
    public const string Doctor = "Doctor";

    /// <summary>
    ///     Represents the role of a client.
    /// </summary>
    public const string Client = "Client";

    /// <summary>
    ///     Represents the role of a receptionist.
    /// </summary>
    public const string Receptionist = "Receptionist";

    /// <summary>
    ///     Represents the role of a supervisor.
    /// </summary>
    public const string Supervisor = "Supervisor";

    /// <summary>
    ///     Represents the role of a master supervisor.
    /// </summary>
    public const string MasterSupervisor = "Master-Supervisor";

    /// <summary>
    ///     Checks if a given role exists in the system.
    /// </summary>
    /// <param name="role">The role to check.</param>
    /// <returns>True if the role exists, otherwise false.</returns>
    public static bool RoleExists(string role)
    {
        var fields = typeof(RoleConstants).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

        return fields.Any(f => f is { IsLiteral: true, IsInitOnly: false } && f?.GetValue(null)?.ToString() == role);
    }
}
