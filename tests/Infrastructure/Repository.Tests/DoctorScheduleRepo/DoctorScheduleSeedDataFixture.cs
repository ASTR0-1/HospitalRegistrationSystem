using System;
using HospitalRegistrationSystem.Domain.Entities;
using HospitalRegistrationSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HospitalRegistrationSystem.Tests.Infrastructure.Repository.DoctorScheduleRepo;

public class DoctorScheduleSeedDataFixture : IDisposable
{
    public DoctorScheduleSeedDataFixture()
    {
        ApplicationContext.DoctorSchedules.Add(new DoctorSchedule
        {
            Id = 1,
            DoctorId = 1,
            Date = new DateOnly(2022, 1, 1),
            WorkingHours = 1 << 9 | 1 << 10
        });

        ApplicationContext.DoctorSchedules.Add(new DoctorSchedule
        {
            Id = 2,
            DoctorId = 1,
            Date = new DateOnly(2022, 1, 2),
            WorkingHours = 1 << 10 | 1 << 11
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

