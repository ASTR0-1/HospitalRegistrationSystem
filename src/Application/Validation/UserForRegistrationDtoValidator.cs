using FluentValidation;
using HospitalRegistrationSystem.Application.DTOs.AuthenticationDTOs;

namespace HospitalRegistrationSystem.Application.Validation;

public class UserForRegistrationDtoValidator : AbstractValidator<UserForRegistrationDto>
{
    public UserForRegistrationDtoValidator()
    {
        RuleFor(e => e.FirstName)
            .NotEmpty()
            .WithMessage("FirstName is required");

        RuleFor(e => e.LastName)
            .NotEmpty()
            .WithMessage("LastName is required");

        RuleFor(e => e.PhoneNumber)
            .NotEmpty()
            .WithMessage("PhoneNumber is required")
            .Matches(@"^\+?\d{10,13}")
            .WithMessage("Phone number must be 10 digits");

        RuleFor(e => e.Gender)
            .NotEmpty()
            .WithMessage("Gender is required");

        RuleFor(e => e.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(5)
            .WithMessage("Password must be at least 5 characters");
    }
}