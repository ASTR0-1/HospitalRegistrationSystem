using HospitalRegistrationSystem.Domain.Shared.ResultPattern;

namespace HospitalRegistrationSystem.Domain.Errors;

public class HospitalError
{
    public static Error HospitalIdNotFound(int hospitalId) => new($"Hospital hospitalId not found.", $"The hospital with ID '{hospitalId}' does not exist.");
}
