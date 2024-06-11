using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalRegistrationSystem.Infrastructure.Persistence.Configurations;

public class HospitalConfiguration : IEntityTypeConfiguration<Hospital>
{
    public void Configure(EntityTypeBuilder<Hospital> builder)
    {
        builder.HasKey(h => h.Id);

        builder.Property(h => h.Name)
            .IsRequired();

        builder.OwnsOne(h => h.Address, ha =>
        {
            ha.HasOne(a => a.City)
                .WithMany()
                .HasForeignKey(a => a.CityId);
        });
    }
}