using System.Collections.Generic;
using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalRegistrationSystem.Infrastructure.Persistence.DataSeeding;

public class CityDataSeed : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        var cities = new List<City>
        {
            new()
            {
                Id = 1,
                Name = "Київ",
                RegionId = 1
            },
            new()
            {
                Id = 2,
                Name = "Харків",
                RegionId = 2
            },
            new()
            {
                Id = 3,
                Name = "Львів",
                RegionId = 3
            },
            new()
            {
                Id = 4,
                Name = "Одеса",
                RegionId = 4
            },
            new()
            {
                Id = 5,
                Name = "Дніпро",
                RegionId = 5
            },
            new()
            {
                Id = 6,
                Name = "Запоріжжя",
                RegionId = 6
            },
            new()
            {
                Id = 7,
                Name = "Херсон",
                RegionId = 7
            },
            new()
            {
                Id = 8,
                Name = "Кривий ріг",
                RegionId = 5
            }
        };

        builder.HasData(cities);
    }
}