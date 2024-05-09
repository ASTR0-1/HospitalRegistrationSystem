using System.Collections.Generic;
using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalRegistrationSystem.Infrastructure.Persistence.DataSeeding;

public class RegionDataSeed : IEntityTypeConfiguration<Region>
{
    public void Configure(EntityTypeBuilder<Region> builder)
    {
        var regions = new List<Region>
        {
            new()
            {
                Id = 1,
                Name = "Київ",
                CountryId = 1
            },
            new()
            {
                Id = 2,
                Name = "Харківська область",
                CountryId = 1
            },
            new()
            {
                Id = 3,
                Name = "Львівська область",
                CountryId = 1
            },
            new()
            {
                Id = 4,
                Name = "Одеська область",
                CountryId = 1
            },
            new()
            {
                Id = 5,
                Name = "Дніпропетровська область",
                CountryId = 1
            },
            new()
            {
                Id = 6,
                Name = "Запорізька область",
                CountryId = 1
            },
            new()
            {
                Id = 7,
                Name = "Херсонська область",
                CountryId = 1
            }
        };

        builder.HasData(regions);
    }
}
