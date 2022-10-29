﻿using FluentValidation;
using HospitalRegistrationSystem.Application.Interfaces.DTOs;

namespace HospitalRegistrationSystem.Application.Validation
{
    public class AppointmentForCreationDTOValidator : AbstractValidator<AppointmentForCreationDTO>
    {
        public AppointmentForCreationDTOValidator()
        {
            RuleFor(e => e.VisitTime)
                .NotEmpty();
            RuleFor(e => e.Diagnosis)
                .NotEmpty();
            RuleFor(e => e.DoctorId)
                .NotEmpty();
            RuleFor(e => e.ClientId)
                .NotEmpty();
        }
    }
}
