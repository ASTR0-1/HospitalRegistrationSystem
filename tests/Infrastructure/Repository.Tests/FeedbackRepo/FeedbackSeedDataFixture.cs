using System;
using System.Collections.Generic;
using HospitalRegistrationSystem.Domain.Entities;
using HospitalRegistrationSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HospitalRegistrationSystem.Tests.Infrastructure.Repository.FeedbackRepo;

public class FeedbackSeedDataFixture : IDisposable
{
    public FeedbackSeedDataFixture()
    {
        var user = new ApplicationUser
        {
            Id = 1,
            UserName = "User1",
            FirstName = "First",
            LastName = "Last",
            Gender = "Male",
            PhoneNumber = "1234567890",
            HospitalId = 1
        };
        ApplicationContext.ApplicationUsers.Add(user);

        var appointment1 = new Appointment { Id = 1, ApplicationUsers = new List<ApplicationUser> { user } };
        var appointment2 = new Appointment { Id = 2, ApplicationUsers = new List<ApplicationUser> { user } };
        ApplicationContext.Appointments.Add(appointment1);
        ApplicationContext.Appointments.Add(appointment2);

        var feedback1 = new Feedback
        {
            Id = 1,
            AppointmentId = 1,
            FeedbackDate = new DateTime(2022, 1, 1),
            Rating = 5,
            Appointment = appointment1
        };
        ApplicationContext.Feedbacks.Add(feedback1);

        var feedback2 = new Feedback
        {
            Id = 2,
            AppointmentId = 2,
            FeedbackDate = new DateTime(2022, 1, 2),
            Rating = 4,
            Appointment = appointment2
        };
        ApplicationContext.Feedbacks.Add(feedback2);

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

