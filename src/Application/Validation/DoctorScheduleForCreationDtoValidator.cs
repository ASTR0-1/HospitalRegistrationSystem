using System;
using FluentValidation;
using HospitalRegistrationSystem.Application.DTOs.DoctorScheduleDTOs;

namespace HospitalRegistrationSystem.Application.Validation;

public class DoctorScheduleForCreationDtoValidator : AbstractValidator<DoctorScheduleForManipulationDto>
{
    public DoctorScheduleForCreationDtoValidator()
    {
        RuleFor(x => x.Date)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now.Date))
            .WithMessage("Date is required and must be greater than or equal to today");
    }
}