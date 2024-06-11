using System.Text;
using HospitalRegistrationSystem.Application.ConfigurationModels;
using HospitalRegistrationSystem.Application.Interfaces;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Application.Interfaces.Identity;
using HospitalRegistrationSystem.Domain.Entities;
using HospitalRegistrationSystem.Infrastructure.Persistence;
using HospitalRegistrationSystem.Infrastructure.Persistence.Identity;
using HospitalRegistrationSystem.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace HospitalRegistrationSystem.Infrastructure.Extensions;

public static class ServiceExtensions
{
    /// <summary>
    ///     Configures the infrastructure layer.
    /// </summary>
    /// <param name="services">Service collection to be configured.</param>
    /// <param name="configuration">Application configuration to get configure settings.</param>
    public static void ConfigureInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.ConfigureSqlContext(configuration);
        services.ConfigureRepositoryManager();

        services.ConfigureIdentity();
        services.ConfigureJwtAuthentication(configuration);
    }

    /// <summary>
    ///     Configures the logger manager.
    /// </summary>
    /// <param name="services">Service collection to be configured.</param>
    public static void ConfigureLoggerManager(this IServiceCollection services)
    {
        services.AddSingleton<ILoggerManager, LoggerManager>();
    }

    /// <summary>
    ///     Configures the authentication manager.
    /// </summary>
    /// <param name="services">Service collection to be configured.</param>
    public static void ConfigureAuthenticationManager(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationManager, AuthenticationManager>();
    }

    /// <summary>
    ///     Configures azure blob for storing profile pictures.
    /// </summary>
    /// <param name="services">Service collection to be configured.</param>
    /// <param name="configuration">Application configuration to get connection string.</param>
    public static void ConfigureAzureBlob(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<AzureBlobOptions>()
            .BindConfiguration(AzureBlobOptions.SectionName);
        services.AddSingleton<IFileStorageManager, AzureBlobFileStorageManager>();

        services.AddAzureClients(builder =>
        {
            builder.AddBlobServiceClient(configuration.GetConnectionString("azureBlob"));
        });
    }

    private static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationContext>(opts =>
            opts.UseSqlServer(configuration.GetConnectionString("sqlConnection"),
                b => b.MigrationsAssembly("HospitalRegistrationSystem.Infrastructure")));
    }

    private static void ConfigureRepositoryManager(this IServiceCollection services)
    {
        services.AddScoped<IRepositoryManager, RepositoryManager>();
    }

    private static void ConfigureIdentity(this IServiceCollection services)
    {
        var builder = services.AddIdentityCore<ApplicationUser>(opts =>
        {
            opts.Password.RequireDigit = true;
            opts.Password.RequireLowercase = false;
            opts.Password.RequireUppercase = false;
            opts.Password.RequireNonAlphanumeric = false;
            opts.Password.RequiredLength = 5;
        });

        builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole<int>), builder.Services);
        builder.AddEntityFrameworkStores<ApplicationContext>()
            .AddRoleManager<RoleManager<IdentityRole<int>>>();
    }

    private static void ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<JwtSettings>()
            .BindConfiguration(JwtSettings.SectionName);

        var jwtSettings = configuration.GetSection("JwtSettings")
            .Get<JwtSettings>();

        services.AddAuthentication(opts =>
            {
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opts =>
            {
                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings?.ValidIssuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings?.ValidAudience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings!.Secret!))
                };
            });
    }
}