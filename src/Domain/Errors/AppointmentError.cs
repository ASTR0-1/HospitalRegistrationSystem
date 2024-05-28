using HospitalRegistrationSystem.Domain.Shared.ResultPattern;

namespace HospitalRegistrationSystem.Domain.Errors;

public static class AppointmentError
{
    public static Error AppointmentNotFound(int appointmentId) => new("Appointment not found.", $"Appointment with id {appointmentId} not found.");

    public static Error DoctorNotInRole(int appointmentDtoDoctorId) => new("Doctor not in role.", $"User with id {appointmentDtoDoctorId} is not a doctor.");

    public static Error DoctorAndClientAreSame(int appointmentDtoClientId) => new("Doctor and client are the same.", $"Doctor and client cannot be the same user with id {appointmentDtoClientId}.");
}
