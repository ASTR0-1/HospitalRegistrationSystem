using System;
using System.Linq;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Domain.Entities;
using HospitalRegistrationSystem.Infrastructure.Persistence.Repositories;
using NUnit.Framework;

namespace HospitalRegistrationSystem.Tests.Infrastructure.Repository.AppointmentRepo;

[TestFixture]
public class AppointmentRepoTests
{
    private IAppointmentRepository _appointmentRepository;
    private AppointmentSeedDataFixture _dataFixture;

    [SetUp]
    public void SetUpFixture()
    {
        _dataFixture = new AppointmentSeedDataFixture();
        _appointmentRepository = new AppointmentRepository(_dataFixture.ApplicationContext);
    }

    [TearDown]
    public void TearDownFixture()
    {
        _dataFixture.Dispose();
    }

    [Test]
    public async Task GetAppointmentAsync_NotExistingId_GetNull()
    {
        // Arrange
        var notExistingId = 0;
        Appointment expectedAppointment = null;

        // Act
        var actualAppointment = await _appointmentRepository.GetAppointmentAsync(notExistingId);

        // Assert
        Assert.That(actualAppointment, Is.EqualTo(expectedAppointment));
    }

    [Test]
    public async Task GetAppointmentAsync_ExistingId_GetValue()
    {
        // Arrange
        var expectedId = 1;

        // Act
        var actualId = (await _appointmentRepository.GetAppointmentAsync(expectedId)).Id;

        // Assert
        Assert.That(actualId, Is.EqualTo(expectedId));
    }

    [Test]
    public void CreateAppointment_Success()
    {
        // Arrange
        var appointment = new Appointment { Id = 3, VisitTime = DateTime.Now.AddDays(3), IsVisited = false };

        // Act
        _appointmentRepository.CreateAppointment(appointment);
        _dataFixture.ApplicationContext.SaveChanges();

        // Assert
        Assert.That(_dataFixture.ApplicationContext.Appointments.Count(), Is.EqualTo(3));
    }

    [Test]
    public void DeleteAppointment_Success()
    {
        // Arrange
        var appointment = _dataFixture.ApplicationContext.Appointments.First();

        // Act
        _appointmentRepository.DeleteAppointment(appointment);
        _dataFixture.ApplicationContext.SaveChanges();

        // Assert
        Assert.That(_dataFixture.ApplicationContext.Appointments.Count(), Is.EqualTo(1));
    }
}
