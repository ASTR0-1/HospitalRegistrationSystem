using FluentValidation;
using HospitalRegistrationSystem.Application.DTOs.AuthenticationDTOs;

namespace HospitalRegistrationSystem.Application.Validation;

public class UserForAuthenticationDtoValidator : AbstractValidator<UserForAuthenticationDto>
{
    public UserForAuthenticationDtoValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .Matches(@"^\+?\d{10,13}")
            .WithMessage("Phone number must be 10 digits");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(5)
            .WithMessage("Password must be at least 5 characters");
    }
}
