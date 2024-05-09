using System;
using HospitalRegistrationSystem.Domain.Shared;

namespace HospitalRegistrationSystem.Domain.Errors;

public static class ApplicationUserError
{
    public static Error UserIdNotFound(int userId) => new("User id not found", $"The user with ID '{userId}' does not exist.");

    public static Error UserUpdateFailed(string errors) => new("User update failed", $"The user could not be updated. Errors: {errors}");

    public static Error RoleAssignmentFailed(string role, string errors) => new("User role assignment failed", $"The user could not be assigned to the role '{role}'. Errors: {errors}");

    public static Error UnauthorizedEmployeeAssignment(int userId, string role, int hospitalId) => new("Unauthorized employee assignment", $"The user with ID '{userId}' is not authorized to assign employees. Assigning role: '{role}'. Assigning hospital id: {hospitalId}.");
}
