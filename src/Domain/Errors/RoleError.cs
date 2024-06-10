using HospitalRegistrationSystem.Domain.Shared.ResultPattern;

namespace HospitalRegistrationSystem.Domain.Errors;

public static class RoleError
{
    public static Error RoleNotFound(string role)
    {
        return new Error("Role not found", $"The role '{role}' does not exist.");
    }
}