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
    [SetUp]
    public void SetUpFixture()
    {
        _dataFixture = new AppointmentSeedDataFixture();
        _appointmentRepository = new AppointmentRepository(_dataFixture.RepositoryContext);
    }

    [TearDown]
    public void TearDownFixture()
    {
        _dataFixture.Dispose();
    }

    private IAppointmentRepository _appointmentRepository;
    private AppointmentSeedDataFixture _dataFixture;

    [Test]
    public void Create_Null_ThrowArgumentNullException()
    {
        // Arrange
        Appointment nullAppointment = null;
        var expectedExceptionType = typeof(ArgumentNullException);

        // Act
        var actualExceptionType =
            Assert.Catch(() => _appointmentRepository.CreateAppointment(nullAppointment)).GetType();

        // Assert
        Assert.That(actualExceptionType, Is.EqualTo(expectedExceptionType));
    }

    [Test]
    public async Task Create_NotNull_AddValue()
    {
        // Arrange
        var expectedDiagnosis = "D3";
        var appointment = new Appointment
        {
            VisitTime = new DateTime(3033, 3, 3),
            Diagnosis = "D3"
        };

        // Act
        _appointmentRepository.CreateAppointment(appointment);
        await _dataFixture.RepositoryContext.SaveChangesAsync();
        var actualDiagnosis = (await _appointmentRepository.GetAppointmentsAsync(false)).Last().Diagnosis;

        // Assert
        Assert.That(actualDiagnosis, Is.EqualTo(expectedDiagnosis));
    }

    [Test]
    public void Delete_Null_ThrowArgumentNullException()
    {
        // Arrange
        Appointment nullAppointment = null;
        var expectedExceptionType = typeof(ArgumentNullException);

        // Act
        var actualExceptionType =
            Assert.Catch(() => _appointmentRepository.DeleteAppointment(nullAppointment)).GetType();

        // Assert
        Assert.That(actualExceptionType, Is.EqualTo(expectedExceptionType));
    }

    [Test]
    public async Task Delete_Existing_DeleteValue()
    {
        // Arrange
        var expectedDiagnosis = "D1";
        // TODO: Remake guid
        var appointmentToDelete = await _appointmentRepository.GetAppointmentAsync(Guid.Empty, true);

        // Act
        _appointmentRepository.DeleteAppointment(appointmentToDelete);
        await _dataFixture.RepositoryContext.SaveChangesAsync();
        var actualDiagnosis = (await _appointmentRepository.GetAppointmentsAsync(false)).Last().Diagnosis;

        // Assert
        Assert.That(actualDiagnosis, Is.EqualTo(expectedDiagnosis));
    }

    [Test]
    public async Task GetAppointmentsAsync_GetValue()
    {
        // Arrange
        var expectedCount = 2;

        // Act
        var actualCount = (await _appointmentRepository.GetAppointmentsAsync(false)).Count();

        // Assert
        Assert.That(actualCount, Is.EqualTo(expectedCount));
    }

    [Test]
    public async Task GetAppointmentAsync_NotExistingId_GetNull()
    {
        // Arrange
        var notExistingId = Guid.Empty;
        Appointment expectedAppointment = null;

        // Act
        var actualAppointment = await _appointmentRepository.GetAppointmentAsync(notExistingId, false);

        // Assert
        Assert.That(actualAppointment, Is.EqualTo(expectedAppointment));
    }

    [Test]
    public async Task GetAppointmentAsync_ExistingId_GetValue()
    {
        // Arrange
        var expectedId = 1;

        // Act
        // TODO: Remake guid
        var actualId = (await _appointmentRepository.GetAppointmentAsync(Guid.Empty, false)).Id;

        // Assert
        Assert.That(actualId, Is.EqualTo(expectedId));
    }
}