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
        _appointmentRepository = new AppointmentRepository(_dataFixture.RepositoryContext);
    }

    [TearDown]
    public void TearDownFixture()
    {
        _dataFixture.Dispose();
    }

    [Test]
    public void Create_Null_ThrowArgumentNullException()
    {
        // Arrange
        Appointment nullAppointment = null;
        Type expectedExceptionType = typeof(ArgumentNullException);

        // Act
        Type actualExceptionType = (Assert.Catch(() => _appointmentRepository.CreateAppointment(nullAppointment))).GetType();

        // Assert
        Assert.That(actualExceptionType, Is.EqualTo(expectedExceptionType));
    }

    [Test]
    public async Task Create_NotNull_AddValue()
    {
        // Arrange
        string expectedDiagnosis = "D3";
        Appointment appointment = new Appointment {
            ClientId = 2,
            DoctorId = 2,
            VisitTime = new DateTime(3033, 3, 3),
            Diagnosis = "D3"
        };

        // Act
        _appointmentRepository.CreateAppointment(appointment);
        await _dataFixture.RepositoryContext.SaveChangesAsync();
        string actualDiagnosis = (await _appointmentRepository.GetAppointmentsAsync(trackChanges: false)).Last().Diagnosis;

        // Assert
        Assert.That(actualDiagnosis, Is.EqualTo(expectedDiagnosis));
    }

    [Test]
    public void Delete_Null_ThrowArgumentNullException()
    {
        // Arrange
        Appointment nullAppointment = null;
        Type expectedExceptionType = typeof(ArgumentNullException);

        // Act
        Type actualExceptionType = (Assert.Catch(() => _appointmentRepository.DeleteAppointment(nullAppointment))).GetType();

        // Assert
        Assert.That(actualExceptionType, Is.EqualTo(expectedExceptionType));
    }

    [Test]
    public async Task Delete_Existing_DeleteValue()
    {
        // Arrange
        string expectedDiagnosis = "D1";
        var appointmentToDelete = await _appointmentRepository.GetAppointmentAsync(2, true);

        // Act
        _appointmentRepository.DeleteAppointment(appointmentToDelete);
        await _dataFixture.RepositoryContext.SaveChangesAsync();
        string actualDiagnosis = (await _appointmentRepository.GetAppointmentsAsync(false)).Last().Diagnosis;

        // Assert
        Assert.That(actualDiagnosis, Is.EqualTo(expectedDiagnosis));
    }

    [Test]
    public async Task GetAppointmentsAsync_GetValue()
    {
        // Arrange
        int expectedCount = 2;

        // Act
        int actualCount = (await _appointmentRepository.GetAppointmentsAsync(false)).Count();

        // Assert
        Assert.That(actualCount, Is.EqualTo(expectedCount));
    }

    [Test]
    public async Task GetAppointmentAsync_NotExistingId_GetNull()
    {
        // Arrange
        int notExistingId = -1;
        Appointment expectedAppointment = null;

        // Act
        var actualAppointment = await _appointmentRepository.GetAppointmentAsync(notExistingId, trackChanges: false);

        // Assert
        Assert.That(actualAppointment, Is.EqualTo(expectedAppointment));
    }

    [Test]
    public async Task GetAppointmentAsync_ExistingId_GetValue()
    {
        // Arrange
        int expectedId = 1;

        // Act
        int actualId = (await _appointmentRepository.GetAppointmentAsync(expectedId, trackChanges: false)).Id;

        // Assert
        Assert.That(actualId, Is.EqualTo(expectedId));
    }
}
