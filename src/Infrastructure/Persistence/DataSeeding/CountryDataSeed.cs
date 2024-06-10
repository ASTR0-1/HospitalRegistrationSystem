using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalRegistrationSystem.Infrastructure.Persistence.DataSeeding;

public class CountryDataSeed : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.HasData(new Country
        {
            Id = 1,
            Name = "Україна",
            ISO2 = "UA",
            ISO3 = "UKR"
        });
    }
}