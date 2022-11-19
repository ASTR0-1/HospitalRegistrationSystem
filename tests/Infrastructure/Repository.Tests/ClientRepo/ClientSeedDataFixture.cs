using System;
using HospitalRegistrationSystem.Domain.Entities;
using HospitalRegistrationSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HospitalRegistrationSystem.Tests.Infrastructure.Repository.ClientRepo
{
    public class ClientSeedDataFixture : IDisposable
    {
        public RepositoryContext RepositoryContext { get; } = new RepositoryContext(
            new DbContextOptionsBuilder<RepositoryContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

        public ClientSeedDataFixture()
        {
            RepositoryContext.Clients.Add(new Client
            {
                FirstName = "f1",
                MiddleName = "m1",
                LastName = "l1",
                Gender = "g1",
            });

            RepositoryContext.Clients.Add(new Client
            {
                FirstName = "f2",
                MiddleName = "m2",
                LastName = "l2",
                Gender = "g2",
            });

            RepositoryContext.SaveChanges();
        }

        public void Dispose()
        {
            RepositoryContext.Database.EnsureDeleted();
            RepositoryContext.Dispose();
        }
    }
}
