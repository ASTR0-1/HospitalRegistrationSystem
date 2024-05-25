using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        RuleFor(x => x.WorkingHoursList)
            .NotEmpty()
            .WithMessage("WorkingHoursList is required");
    }
}
