using FluentValidation;
using HospitalRegistrationSystem.Application.Interfaces.DTOs;

namespace HospitalRegistrationSystem.Application.Validation
{
    public class DoctorForCreationDTOValidator : AbstractValidator<DoctorForCreationDTO>
    {
        public DoctorForCreationDTOValidator()
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
            RuleFor(e => e.Specialty)
                .MaximumLength(30)
                .NotEmpty();
        }
    }
}
