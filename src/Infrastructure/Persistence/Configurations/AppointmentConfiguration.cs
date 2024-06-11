using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalRegistrationSystem.Infrastructure.Persistence.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.VisitTime)
            .IsRequired();
        builder.Property(a => a.Diagnosis)
            .IsRequired(false);

        builder.HasMany(a => a.ApplicationUsers)
            .WithMany(u => u.Appointments);

        builder.HasOne(a => a.Feedback)
            .WithOne(f => f.Appointment)
            .HasForeignKey<Appointment>(a => a.FeedbackId);
    }
}