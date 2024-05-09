using System.IO;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Extensions;
using HospitalRegistrationSystem.Application.Interfaces;
using HospitalRegistrationSystem.Application.Mappers;
using HospitalRegistrationSystem.Infrastructure.Extensions;
using HospitalRegistrationSystem.WebAPI.ActionFilters;
using HospitalRegistrationSystem.WebAPI.Exstensions;
using HospitalRegistrationSystem.WebAPI.Extensions;
using HospitalRegistrationSystem.WebAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using NLog;

namespace HospitalRegistrationSystem.WebAPI;

/// <summary>
///     Main entry point of the application.
/// </summary>
public abstract class Program
{
    /// <summary>
    ///     Main method of the application.
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        LogManager.Setup()
            .LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

        var services = builder.Services;
        var configuration = builder.Configuration;

        services.ConfigureCors();

        services.ConfigureLoggerManager();

        services.AddAuthentication();

        services.ConfigureInfrastructure(configuration);
        services.ConfigureEntityServices();

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.ConfigureAuthenticationManager();

        services.AddAutoMapper(typeof(MappingProfile));

        services.ConfigureAzureBlob(configuration);

        services.AddControllers(options =>
        {
            options.RespectBrowserAcceptHeader = true;
            options.ReturnHttpNotAcceptable = true;

            options.Filters.Add<PaginationHeaderFilter>();
        }).AddNewtonsoftJson(opts =>
        {
            opts.SerializerSettings.ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
        });

        services.AddFluentValidation();

        services.ConfigureSwagger();

        var app = builder.Build();

        await app.MigrateDatabaseAsync();
        await app.ConfigureInitialSupervisorAsync();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI();

            // Prolong token expiration for development purposes
            configuration.GetSection("JwtSettings").GetSection("expires").Value = "60";
        }
        else
        {
            app.UseHsts();
            app.UseStaticFiles();
        }

        var logger = app.Services.GetRequiredService<ILoggerManager>();
        app.ConfigureExceptionHandler(logger);
        app.UseHttpsRedirection();

        app.UseCors("CorsPolicy");

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.All
        });

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        await app.RunAsync();
    }
}
