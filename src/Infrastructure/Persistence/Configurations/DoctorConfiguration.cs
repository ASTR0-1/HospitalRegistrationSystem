using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalRegistrationSystem.Infrastructure.Persistence.Configurations
{
    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.HasKey(d => d.Id);
            builder.HasMany(d => d.Appointments)
                .WithOne(d => d.Doctor);

            builder.Property(d => d.FirstName)
                .IsRequired();
            builder.Property(d => d.MiddleName)
                .IsRequired();
            builder.Property(d => d.LastName)
                .IsRequired();
            builder.Property(d => d.Gender)
                .HasMaxLength(10)
                .IsRequired();
            builder.Property(d => d.Specialty)
                .HasMaxLength(30)
                .IsRequired();
        }
    }
}
