using System;
using HospitalRegistrationSystem.Domain.Entities;
using HospitalRegistrationSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HospitalRegistrationSystem.Tests.Infrastructure.Repository.CityRepo;

public class CitySeedDataFixture : IDisposable
{
    public CitySeedDataFixture()
    {
        ApplicationContext.Cities.Add(new City
        {
            Id = 1,
            Name = "City1"
        });

        ApplicationContext.Cities.Add(new City
        {
            Id = 2,
            Name = "City2"
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
