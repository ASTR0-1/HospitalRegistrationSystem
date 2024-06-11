using System;
using HospitalRegistrationSystem.Domain.Entities;
using HospitalRegistrationSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HospitalRegistrationSystem.Tests.Infrastructure.Repository.HospitalRepo;

public class HospitalSeedDataFixture : IDisposable
{
    public HospitalSeedDataFixture()
    {
        ApplicationContext.Hospitals.Add(new Hospital
        {
            Id = 1,
            Name = "Hospital1",
            Address = new Address
            {
                CityId = 1,
                City = new City
                {
                    Id = 1,
                    Name = "City1"
                },
                Street = "Street1"
            }
        });

        ApplicationContext.Hospitals.Add(new Hospital
        {
            Id = 2,
            Name = "Hospital2",
            Address = new Address
            {
                CityId = 2,
                City = new City
                {
                    Id = 2,
                    Name = "City2"
                },
                Street = "Street2"
            }
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