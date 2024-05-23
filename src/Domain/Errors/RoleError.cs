using HospitalRegistrationSystem.Domain.Shared.ResultPattern;

namespace HospitalRegistrationSystem.Domain.Errors;

public static class RoleError
{
    public static Error RoleNotFound(string role) => new("Role not found", $"The role '{role}' does not exist.");
}
