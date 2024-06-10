using System;
using HospitalRegistrationSystem.Domain.Entities;
using HospitalRegistrationSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HospitalRegistrationSystem.Tests.Infrastructure.Repository.RegionRepo;

public class RegionSeedDataFixture : IDisposable
{
    public RegionSeedDataFixture()
    {
        ApplicationContext.Regions.Add(new Region
        {
            Id = 1,
            Name = "Region1"
        });

        ApplicationContext.Regions.Add(new Region
        {
            Id = 2,
            Name = "Region2"
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