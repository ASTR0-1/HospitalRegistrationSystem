using FluentValidation;
using HospitalRegistrationSystem.Application.DTOs;
using HospitalRegistrationSystem.Application.Interfaces;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Application.Interfaces.Services;
using HospitalRegistrationSystem.Application.Services;
using HospitalRegistrationSystem.Application.Validation;
using HospitalRegistrationSystem.Infrastructure.Persistence;
using HospitalRegistrationSystem.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HospitalRegistrationSystem.WebAPI.Exstensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services) =>
        services.AddCors(opts =>
        {
            opts.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
        });

    public static void ConfigureLoggerService(this IServiceCollection services) =>
        services.AddSingleton<ILoggerManager, LoggerManager>();

    public static void ConfigureSqlContext(this IServiceCollection services,
        IConfiguration configuration) =>
        services.AddDbContext<RepositoryContext>(opts =>
            opts.UseSqlServer(configuration.GetConnectionString("sqlConnection"),
                b => b.MigrationsAssembly("Infrastructure")));

    public static void ConfigureRepositoryManager(this IServiceCollection services) =>
        services.AddScoped<IRepositoryManager, RepositoryManager>();

    public static void ConfigureEntityServices(this IServiceCollection services)
    {
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IDoctorService, DoctorService>();
    }

    public static void AddFluentValidation(this IServiceCollection services)
    {
        services.AddScoped<IValidator<ClientForCreationDTO>, ClientForCreationDTOValidator>();
        services.AddScoped<IValidator<AppointmentForCreationDTO>, AppointmentForCreationDTOValidator>();
        services.AddScoped<IValidator<DoctorForCreationDTO>, DoctorForCreationDTOValidator>();
    }
}
