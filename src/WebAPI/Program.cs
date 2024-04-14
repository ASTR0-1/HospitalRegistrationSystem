using System.IO;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Extensions;
using HospitalRegistrationSystem.Application.Interfaces;
using HospitalRegistrationSystem.Application.Mappers;
using HospitalRegistrationSystem.Domain.Entities;
using HospitalRegistrationSystem.Infrastructure.Extensions;
using HospitalRegistrationSystem.Infrastructure.Persistence.Identity;
using HospitalRegistrationSystem.WebAPI.Exstensions;
using HospitalRegistrationSystem.WebAPI.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;

namespace HospitalRegistrationSystem.WebAPI;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(),
            "/nlog.config"));

        var services = builder.Services;
        IConfiguration configuration = builder.Configuration;

        services.ConfigureCors();

        services.ConfigureLoggerService();

        services.AddAuthentication();
        services.AddAuthorization();

        services.ConfigureInfrastructure(configuration);
        services.ConfigureEntityServices();

        services.ConfigureJwt(configuration);
        services.AddScoped<IAuthenticationManager, AuthenticationManager>();

        services.AddAutoMapper(typeof(MappingProfile));

        services.AddControllers(config =>
        {
            config.RespectBrowserAcceptHeader = true;
            config.ReturnHttpNotAcceptable = true;
        }).AddNewtonsoftJson();

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

        app.UseAuthorization();

        app.MapControllers();

        await app.RunAsync();
    }
}