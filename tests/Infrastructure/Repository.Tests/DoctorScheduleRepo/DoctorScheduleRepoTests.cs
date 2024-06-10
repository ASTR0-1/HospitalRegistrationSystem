using System;
using System.Linq;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Entities;
using HospitalRegistrationSystem.Infrastructure.Persistence.Repositories;
using NUnit.Framework;

namespace HospitalRegistrationSystem.Tests.Infrastructure.Repository.DoctorScheduleRepo;

[TestFixture]
public class DoctorScheduleRepoTests
{
    [SetUp]
    public void SetUpFixture()
    {
        _dataFixture = new DoctorScheduleSeedDataFixture();
        _doctorScheduleRepository = new DoctorScheduleRepository(_dataFixture.ApplicationContext);
    }

    [TearDown]
    public void TearDownFixture()
    {
        _dataFixture.Dispose();
    }

    private IDoctorScheduleRepository _doctorScheduleRepository;
    private DoctorScheduleSeedDataFixture _dataFixture;

    [Test]
    public async Task GetDoctorSchedulesAsync_GetValue()
    {
        // Arrange
        var expectedCount = 2;
        var parameters = new DoctorScheduleParameters
        {
            From = new DateOnly(2022, 1, 1),
            To = new DateOnly(2022, 1, 2),
            PageNumber = 1,
            PageSize = 10
        };

        // Act
        var actualCount = (await _doctorScheduleRepository.GetDoctorSchedulesAsync(parameters, 1)).Count;

        // Assert
        Assert.That(actualCount, Is.EqualTo(expectedCount));
    }

    [Test]
    public async Task GetDoctorScheduleByIdAsync_NotExistingId_GetNull()
    {
        // Arrange
        var notExistingId = 0;
        DoctorSchedule expectedDoctorSchedule = null;

        // Act
        var actualDoctorSchedule = await _doctorScheduleRepository.GetDoctorScheduleByIdAsync(notExistingId);

        // Assert
        Assert.That(actualDoctorSchedule, Is.EqualTo(expectedDoctorSchedule));
    }

    [Test]
    public async Task GetDoctorScheduleByIdAsync_ExistingId_GetValue()
    {
        // Arrange
        var expectedId = 1;

        // Act
        var actualId = (await _doctorScheduleRepository.GetDoctorScheduleByIdAsync(expectedId)).Id;

        // Assert
        Assert.That(actualId, Is.EqualTo(expectedId));
    }

    [Test]
    public void CreateDoctorSchedule_Success()
    {
        // Arrange
        var doctorSchedule = new DoctorSchedule
            {Id = 3, DoctorId = 1, Date = new DateOnly(2022, 1, 3), WorkingHours = (1 << 11) | (1 << 12)};

        // Act
        _doctorScheduleRepository.CreateDoctorSchedule(doctorSchedule);
        _dataFixture.ApplicationContext.SaveChanges();

        // Assert
        Assert.That(_dataFixture.ApplicationContext.DoctorSchedules.Count(), Is.EqualTo(3));
    }

    [Test]
    public void DeleteDoctorSchedule_Success()
    {
        // Arrange
        var doctorSchedule = _dataFixture.ApplicationContext.DoctorSchedules.First();

        // Act
        _doctorScheduleRepository.DeleteDoctorSchedule(doctorSchedule);
        _dataFixture.ApplicationContext.SaveChanges();

        // Assert
        Assert.That(_dataFixture.ApplicationContext.DoctorSchedules.Count(), Is.EqualTo(1));
    }
}