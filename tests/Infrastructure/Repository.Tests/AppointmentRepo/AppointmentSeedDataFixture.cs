using System;
using HospitalRegistrationSystem.Domain.Entities;
using HospitalRegistrationSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HospitalRegistrationSystem.Tests.Infrastructure.Repository.AppointmentRepo;

public class AppointmentSeedDataFixture : IDisposable
{
    public AppointmentSeedDataFixture()
    {
        ApplicationContext.Appointments.Add(new Appointment
        {
            VisitTime = new DateTime(2022, 1, 1),
            Diagnosis = "D1"
        });

        ApplicationContext.Appointments.Add(new Appointment
        {
            VisitTime = new DateTime(2022, 2, 2),
            Diagnosis = "D2"
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