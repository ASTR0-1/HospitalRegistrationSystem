using System;
using HospitalRegistrationSystem.Domain.Entities;
using HospitalRegistrationSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HospitalRegistrationSystem.Tests.Infrastructure.Repository.AppointmentRepo;

public class AppointmentSeedDataFixture : IDisposable
{
    public AppointmentSeedDataFixture()
    {
        RepositoryContext.Appointments.Add(new Appointment
        {
            
            VisitTime = new DateTime(2022, 1, 1),
            Diagnosis = "D1"
        });

        RepositoryContext.Appointments.Add(new Appointment
        {
            
            VisitTime = new DateTime(2022, 2, 2),
            Diagnosis = "D2"
        });
        RepositoryContext.SaveChanges();
    }

    public RepositoryContext RepositoryContext { get; } = new(
        new DbContextOptionsBuilder<RepositoryContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    public void Dispose()
    {
        RepositoryContext.Database.EnsureDeleted();
        RepositoryContext.Dispose();
    }
}