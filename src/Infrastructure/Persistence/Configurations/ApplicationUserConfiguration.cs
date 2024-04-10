using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace HospitalRegistrationSystem.Infrastructure.Persistence.Configurations;
public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.FirstName)
            .HasMaxLength(250)
            .IsRequired();
        builder.Property(u => u.MiddleName)
            .HasMaxLength(250)
            .IsRequired();
        builder.Property(u => u.LastName)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(u => u.PhoneNumber)
            .IsRequired();
        builder.HasIndex(u => u.PhoneNumber)
            .IsUnique();

        builder.HasMany(u => u.Appointments)
            .WithMany(a => a.ApplicationUsers);
    }
}
