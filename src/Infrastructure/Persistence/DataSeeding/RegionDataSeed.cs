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
                Name = "Харківська",
                CountryId = 1
            },
            new()
            {
                Id = 3,
                Name = "Львівська",
                CountryId = 1
            },
            new()
            {
                Id = 4,
                Name = "Одеська",
                CountryId = 1
            },
            new()
            {
                Id = 5,
                Name = "Дніпропетровська",
                CountryId = 1
            },
            new()
            {
                Id = 6,
                Name = "Запорізька",
                CountryId = 1
            },
            new()
            {
                Id = 7,
                Name = "Херсонська",
                CountryId = 1
            }
        };

        builder.HasData(regions);
    }
}