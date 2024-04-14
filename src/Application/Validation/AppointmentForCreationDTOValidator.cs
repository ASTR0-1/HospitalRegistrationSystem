using System;
using FluentValidation;
using HospitalRegistrationSystem.Application.DTOs.AppointmentDTOs;

namespace HospitalRegistrationSystem.Application.Validation;

public class AppointmentForCreationDtoValidator : AbstractValidator<AppointmentForCreationDto>
{
    public AppointmentForCreationDtoValidator()
    {
        RuleFor(e => e.VisitTime)
            // Disallow past dates
            .GreaterThan(DateTime.Now.Date)
            .NotEmpty();
        RuleFor(e => e.DoctorId)
            .NotEmpty();
        RuleFor(e => e.ClientId)
            .NotEmpty();
    }
}