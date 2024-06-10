using HospitalRegistrationSystem.Domain.Shared.ResultPattern;

namespace HospitalRegistrationSystem.Domain.Errors;

public class DoctorScheduleError
{
    public static Error DoctorScheduleIdNotFound(int doctorScheduleId)
    {
        return new Error("Doctor schedule ID not found.",
            $"The doctor schedule with ID '{doctorScheduleId}' does not exist.");
    }

    public static Error UnauthorizedAccessToDoctorScheduleManipulation(int callerId, int manipulationId)
    {
        return new Error("Unauthorized access to doctor schedule manipulation.",
            $"The caller with ID '{callerId}' is not authorized to manipulate schedule of the doctor with ID '{manipulationId}'.");
    }
}