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
            Id = 1,
            VisitTime = DateTime.Now.AddDays(1),
            IsVisited = false
        });

        ApplicationContext.Appointments.Add(new Appointment
        {
            Id = 2,
            VisitTime = DateTime.Now.AddDays(2),
            IsVisited = false
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