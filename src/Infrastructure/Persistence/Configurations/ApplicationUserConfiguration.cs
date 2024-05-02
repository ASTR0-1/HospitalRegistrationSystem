using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalRegistrationSystem.Infrastructure.Persistence.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.FirstName)
            .HasMaxLength(250)
            .IsRequired();
        builder.Property(u => u.MiddleName)
            .HasMaxLength(250);
        builder.Property(u => u.LastName)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(u => u.Gender)
            .IsRequired();

        builder.Property(u => u.PhoneNumber)
            .IsRequired();

        builder.HasMany(u => u.Appointments)
            .WithMany(a => a.ApplicationUsers);
    }
}