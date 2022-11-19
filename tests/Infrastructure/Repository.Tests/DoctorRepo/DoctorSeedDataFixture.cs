using HospitalRegistrationSystem.Domain.Entities;
using HospitalRegistrationSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;

namespace HospitalRegistrationSystem.Tests.Infrastructure.Repository.DoctorRepo
{
    public class DoctorSeedDataFixture : IDisposable
    {
        public RepositoryContext RepositoryContext { get; } = new RepositoryContext(
            new DbContextOptionsBuilder<RepositoryContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

        public DoctorSeedDataFixture()
        {
            RepositoryContext.Doctors.Add(new Doctor
            {
                FirstName = "f1",
                MiddleName = "m1",
                LastName = "l1",
                Gender = "g1",
                Specialty = "s1"
            });

            RepositoryContext.Doctors.Add(new Doctor
            {
                FirstName = "f2",
                MiddleName = "m2",
                LastName = "l2",
                Gender = "g2",
                Specialty = "s2"
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
