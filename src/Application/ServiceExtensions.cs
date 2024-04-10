using FluentValidation;
using HospitalRegistrationSystem.Application.DTOs;
using HospitalRegistrationSystem.Application.Interfaces.Services;
using HospitalRegistrationSystem.Application.Services;
using HospitalRegistrationSystem.Application.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace HospitalRegistrationSystem.Application;
public static class ServiceExtensions
{
    public static void ConfigureEntityServices(this IServiceCollection services)
    {
        services.AddScoped<IAppointmentService, AppointmentService>();
    }

    public static void AddFluentValidation(this IServiceCollection services)
    {
        services.AddScoped<IValidator<AppointmentForCreationDTO>, AppointmentForCreationDTOValidator>();
    }
}
