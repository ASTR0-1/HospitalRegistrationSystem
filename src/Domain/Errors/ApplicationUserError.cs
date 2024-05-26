using HospitalRegistrationSystem.Domain.Shared.ResultPattern;

namespace HospitalRegistrationSystem.Domain.Errors;

public static class ApplicationUserError
{
    public static Error UserIdNotFound(int userId) => new("User id not found", $"The user with ID '{userId}' does not exist.");

    public static Error UserUpdateFailed(string errors) => new("User update failed", $"The user could not be updated. Errors: {errors}");

    public static Error RoleAssignmentFailed(string role, string errors) => new("User role assignment failed", $"The user could not be assigned to the role '{role}'. Errors: {errors}");

    public static Error UnauthorizedEmployeeAssignment(int userId, string role, int hospitalId) => new("Unauthorized employee assignment", $"The user with ID '{userId}' is not authorized to assign employees. Assigning role: '{role}'. Assigning hospital id: {hospitalId}.");

    public static Error SpecialtyRequiredForDoctor(int userId) => new("Specialty required for Doctor role", $"To be assigned the Doctor role the user with ID '{userId}' must have a specialty.");

    public static Error DoctorPriceInvalid(int userId, decimal? doctorPrice) => new("Doctor price invalid", $"The doctor price for the user with ID '{userId}' is invalid. Price: {doctorPrice}");
}
