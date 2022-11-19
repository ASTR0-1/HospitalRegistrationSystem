using FluentValidation;
using HospitalRegistrationSystem.Application.DTOs;

namespace HospitalRegistrationSystem.Application.Validation;

public class AppointmentForCreationDTOValidator : AbstractValidator<AppointmentForCreationDTO>
{
    public AppointmentForCreationDTOValidator()
    {
        RuleFor(e => e.VisitTime)
            .NotEmpty();
        RuleFor(e => e.DoctorId)
            .NotEmpty();
        RuleFor(e => e.ClientId)
            .NotEmpty();
    }
}
