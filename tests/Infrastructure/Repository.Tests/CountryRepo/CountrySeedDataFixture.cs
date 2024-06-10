using System;
using HospitalRegistrationSystem.Domain.Entities;
using HospitalRegistrationSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HospitalRegistrationSystem.Tests.Infrastructure.Repository.CountryRepo;

public class CountrySeedDataFixture : IDisposable
{
    public CountrySeedDataFixture()
    {
        ApplicationContext.Countries.Add(new Country
        {
            Id = 1,
            Name = "Country1",
            ISO2 = "C1",
            ISO3 = "C1Y"
        });

        ApplicationContext.Countries.Add(new Country
        {
            Id = 2,
            Name = "Country2",
            ISO2 = "C2",
            ISO3 = "C2Y"
        });
        ApplicationContext.SaveChanges();
    }

    public ApplicationContext ApplicationContext { get; } = new(
        new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    public void Dispose()
    {
        ApplicationContext.Database.EnsureDeleted();
        ApplicationContext.Dispose();
    }
}