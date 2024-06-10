using System.Collections.Generic;
using HospitalRegistrationSystem.Domain.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalRegistrationSystem.Infrastructure.Persistence.DataSeeding;

public class RoleDataSeed : IEntityTypeConfiguration<IdentityRole<int>>
{
    public void Configure(EntityTypeBuilder<IdentityRole<int>> builder)
    {
        var defaultRoles = new List<IdentityRole<int>>
        {
            new()
            {
                Id = 1,
                Name = RoleConstants.Doctor,
                NormalizedName = RoleConstants.Doctor.ToUpper()
            },
            new()
            {
                Id = 2,
                Name = RoleConstants.Client,
                NormalizedName = RoleConstants.Client.ToUpper()
            },
            new()
            {
                Id = 3,
                Name = RoleConstants.Receptionist,
                NormalizedName = RoleConstants.Receptionist.ToUpper()
            },
            new()
            {
                Id = 4,
                Name = RoleConstants.Supervisor,
                NormalizedName = RoleConstants.Supervisor.ToUpper()
            },
            new()
            {
                Id = 5,
                Name = RoleConstants.MasterSupervisor,
                NormalizedName = RoleConstants.MasterSupervisor.ToUpper()
            }
        };

        builder.HasData(defaultRoles);
    }
}