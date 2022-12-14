using System.IO;
using HospitalRegistrationSystem.Application.Interfaces;
using HospitalRegistrationSystem.Application.Mappers;
using HospitalRegistrationSystem.WebAPI.Exstensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;

namespace HospitalRegistrationSystem.WebAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(),
            "/nlog.config"));

        IServiceCollection services = builder.Services;
        IConfiguration configuration = builder.Configuration;

        services.ConfigureCors();

        services.ConfigureLoggerService();

        services.ConfigureSqlContext(configuration);
        services.ConfigureRepositoryManager();
        services.ConfigureEntityServices();

        services.AddAutoMapper(typeof(MappingProfile));

        services.AddControllers(config =>
        {
            config.RespectBrowserAcceptHeader = true;
            config.ReturnHttpNotAcceptable = true;
        }).AddNewtonsoftJson();

        services.AddFluentValidation();

        services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseHsts();
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

        app.Run();
    }
}