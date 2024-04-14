using System.Threading.Tasks;
using HospitalRegistrationSystem.Domain.Constants;
using HospitalRegistrationSystem.Domain.Entities;
using HospitalRegistrationSystem.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HospitalRegistrationSystem.WebAPI.Extensions;

public static class WebApplicationExtensions
{
    public static async Task MigrateDatabaseAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<RepositoryContext>();

        await dbContext.Database.MigrateAsync();
    }

    public static async Task ConfigureInitialSupervisorAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        
        var phoneNumber = configuration["MasterSupervisorCredentials:PhoneNumber"];

        var userExists = await userManager.Users.AnyAsync(u => u.PhoneNumber!.Equals(phoneNumber));
        if (userExists)
            return;

        var userSection = configuration.GetSection("MasterSupervisorCredentials");
        var user = new ApplicationUser();
        userSection.Bind(user);
        user.UserName = user.PhoneNumber;

        var password = configuration.GetSection("MasterSupervisorCredentials:Password").Value;

        await userManager.CreateAsync(user, password);
        await userManager.AddToRoleAsync(user, RoleConstants.MasterSupervisor);
    }
}
