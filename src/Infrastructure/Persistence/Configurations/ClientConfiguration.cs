using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalRegistrationSystem.Infrastructure.Persistence.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasKey(c => c.Id);
            builder.HasMany(c => c.Appointments)
                .WithOne(a => a.Client);

            builder.Property(c => c.FirstName)
                .IsRequired();
            builder.Property(c => c.MiddleName)
                .IsRequired();
            builder.Property(c => c.LastName)
                .IsRequired();
            builder.Property(c => c.Gender)
                .HasMaxLength(10)
                .IsRequired();
        }
    }
}
