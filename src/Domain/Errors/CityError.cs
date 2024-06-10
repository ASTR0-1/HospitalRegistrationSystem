using HospitalRegistrationSystem.Domain.Shared.ResultPattern;

namespace HospitalRegistrationSystem.Domain.Errors;

public class CityError
{
    public static Error CityIdNotFound(int cityId)
    {
        return new Error("City id not found", $"The city with ID '{cityId}' does not exist.");
    }
}