using FluentValidation;
using HospitalRegistrationSystem.Application.DTOs.AppointmentDTOs;
using HospitalRegistrationSystem.Application.Interfaces.Services;
using HospitalRegistrationSystem.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HospitalRegistrationSystem.Application.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureEntityServices(this IServiceCollection services)
    {
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IApplicationUserService, ApplicationUserService>();
    }

    public static void AddFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<AppointmentForCreationDto>();
    }
}