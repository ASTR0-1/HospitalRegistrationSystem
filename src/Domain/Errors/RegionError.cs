using HospitalRegistrationSystem.Domain.Shared.ResultPattern;

namespace HospitalRegistrationSystem.Domain.Errors;

public class RegionError
{
    public static Error RegionIdNotFound(int regionId)
    {
        return new Error("Region id not found", $"The region with ID '{regionId}' does not exist.");
    }
}