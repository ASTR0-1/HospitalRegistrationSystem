using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalRegistrationSystem.Infrastructure.Persistence.Configurations;

public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
{
    public void Configure(EntityTypeBuilder<Feedback> builder)
    {
        builder.Property(b => b.Text)
            .IsRequired(false);

        builder.Property(b => b.Rating)
            .HasPrecision(6)
            .IsRequired();

        builder.Property(b => b.FeedbackDate)
            .IsRequired();
    }
}