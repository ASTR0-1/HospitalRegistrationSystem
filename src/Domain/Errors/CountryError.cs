using HospitalRegistrationSystem.Domain.Shared.ResultPattern;

namespace HospitalRegistrationSystem.Domain.Errors;

public class CountryError
{
    public static Error CountryIdNotFound(int countryId) => new("Country id not found", $"The country with ID '{countryId}' does not exist.");
}