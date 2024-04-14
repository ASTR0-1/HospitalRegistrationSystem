using FluentValidation;
using HospitalRegistrationSystem.Application.DTOs.AppointmentDTOs;
using HospitalRegistrationSystem.Application.DTOs.AuthenticationDTOs;
using HospitalRegistrationSystem.Application.Interfaces.Services;
using HospitalRegistrationSystem.Application.Services;
using HospitalRegistrationSystem.Application.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace HospitalRegistrationSystem.Application.Extensions;
public static class ServiceExtensions
{
    public static void ConfigureEntityServices(this IServiceCollection services)
    {
        services.AddScoped<IAppointmentService, AppointmentService>();
    }

    public static void AddFluentValidation(this IServiceCollection services)
    {
        services.AddScoped<IValidator<AppointmentForCreationDto>, AppointmentForCreationDtoValidator>();
    }
}
