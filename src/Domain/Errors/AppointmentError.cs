using System;
using HospitalRegistrationSystem.Domain.Shared.ResultPattern;

namespace HospitalRegistrationSystem.Domain.Errors;

public static class AppointmentError
{
    public static Error AppointmentNotFound(int appointmentId) => new("Appointment not found.", $"Appointment with ID {appointmentId} not found.");

    public static Error DoctorNotInRole(int appointmentDtoDoctorId) => new("Doctor not in role.", $"User with ID {appointmentDtoDoctorId} is not a doctor.");

    public static Error DoctorAndClientAreSame(int appointmentDtoClientId) => new("Doctor and client are the same.", $"Doctor and client cannot be the same user with ID {appointmentDtoClientId}.");

    public static Error DoctorHasNoSchedule(int appointmentDtoDoctorId) => new("Doctor has no schedule.", $"Doctor with ID {appointmentDtoDoctorId} has no schedule.");

    public static Error AppointmentTimeIsNotAvailable(int appointmentDtoDoctorId, DateTime appointmentDtoVisitTime) => new("Appointment time is not available.", $"Appointment time '{appointmentDtoVisitTime}' is not available for doctor with ID '{appointmentDtoDoctorId}'.");
}
