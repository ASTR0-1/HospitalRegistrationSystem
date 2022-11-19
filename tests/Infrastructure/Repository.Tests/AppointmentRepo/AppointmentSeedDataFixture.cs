using System;
using System.Collections.Generic;
using HospitalRegistrationSystem.Domain.Entities;
using HospitalRegistrationSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using NLog.LayoutRenderers.Wrappers;

namespace HospitalRegistrationSystem.Tests.Infrastructure.Repository.AppointmentRepo
{
    public class AppointmentSeedDataFixture : IDisposable
    {
        public RepositoryContext RepositoryContext { get; } = new RepositoryContext(
            new DbContextOptionsBuilder<RepositoryContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

        public AppointmentSeedDataFixture()
        {
            RepositoryContext.Appointments.Add(new Appointment
            {
                Client = new Client
                {
                    FirstName = "f1",
                    MiddleName = "m1",
                    LastName = "l1",
                    Gender = "g1",
                },
                Doctor = new Doctor
                {
                    FirstName = "f1",
                    MiddleName = "m1",
                    LastName = "l1",
                    Gender = "g1",
                    Specialty = "s1"
                },
                VisitTime = new DateTime(2022, 1, 1),
                Diagnosis = "D1"
            });

            RepositoryContext.Appointments.Add(new Appointment
            {
                Client = new Client
                {
                    FirstName = "f2",
                    MiddleName = "m2",
                    LastName = "l2",
                    Gender = "g2",
                },
                Doctor = new Doctor
                {
                    FirstName = "f2",
                    MiddleName = "m2",
                    LastName = "l2",
                    Gender = "g2",
                    Specialty = "s2"
                },
                VisitTime = new DateTime(2022, 2, 2),
                Diagnosis = "D2"
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
