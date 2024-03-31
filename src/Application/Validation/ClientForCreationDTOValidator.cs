using FluentValidation;
using HospitalRegistrationSystem.Application.DTOs;

namespace HospitalRegistrationSystem.Application.Validation;

public class ClientForCreationDTOValidator : AbstractValidator<ClientForCreationDTO>
{
    public ClientForCreationDTOValidator()
    {
        RuleFor(e => e.FirstName)
            .NotEmpty();
        RuleFor(e => e.MiddleName)
            .NotEmpty();
        RuleFor(e => e.LastName)
            .NotEmpty();
        RuleFor(e => e.Gender)
            .MaximumLength(10)
            .NotEmpty();
    }
}