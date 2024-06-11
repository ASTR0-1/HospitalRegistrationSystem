using HospitalRegistrationSystem.Domain.Shared.ResultPattern;

namespace HospitalRegistrationSystem.Domain.Errors;

public static class ApplicationUserError
{
    public static Error UserIdNotFound(int userId)
    {
        return new Error("User id not found", $"The user with ID '{userId}' does not exist.");
    }

    public static Error UserUpdateFailed(string errors)
    {
        return new Error("User update failed", $"The user could not be updated. Errors: {errors}");
    }

    public static Error RoleAssignmentFailed(string role, string errors)
    {
        return new Error("User role assignment failed",
            $"The user could not be assigned to the role '{role}'. Errors: {errors}");
    }

    public static Error UnauthorizedEmployeeAssignment(int userId, string role, int hospitalId)
    {
        return new Error("Unauthorized employee assignment",
            $"The user with ID '{userId}' is not authorized to assign employees. Assigning role: '{role}'. Assigning hospital id: {hospitalId}.");
    }

    public static Error SpecialtyRequiredForDoctor(int userId)
    {
        return new Error("Specialty required for Doctor role",
            $"To be assigned the Doctor role the user with ID '{userId}' must have a specialty.");
    }

    public static Error DoctorPriceInvalid(int userId, decimal? doctorPrice)
    {
        return new Error("Doctor price invalid",
            $"The doctor price for the user with ID '{userId}' is invalid. Price: {doctorPrice}");
    }
}