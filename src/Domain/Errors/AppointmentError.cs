using HospitalRegistrationSystem.Domain.Shared.ResultPattern;

namespace HospitalRegistrationSystem.Domain.Errors;

public static class AppointmentError
{
    public static Error AppointmentNotFound(int appointmentId) => new("Appointment not found.", $"Appointment with id {appointmentId} not found.");
}
