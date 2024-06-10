using HospitalRegistrationSystem.Domain.Shared.ResultPattern;

namespace HospitalRegistrationSystem.Domain.Errors;

public static class FeedbackError
{
    public static Error FeedbackOnNotVisitedAppointmentError(int appointmentId)
    {
        return new Error("Feedback cannot be given for a not visited appointment.",
            $"Feedback cannot be given for a not visited appointment with ID '{appointmentId}'.");
    }
}