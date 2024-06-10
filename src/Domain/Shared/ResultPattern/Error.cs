namespace HospitalRegistrationSystem.Domain.Shared.ResultPattern;

/// <summary>
///     Represents an error with a message and description.
/// </summary>
public sealed record Error(string Message, string Description)
{
    /// <summary>
    ///     Represents an empty error.
    /// </summary>
    public static readonly Error None = new(string.Empty, string.Empty);
}