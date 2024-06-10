using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Application.Mappers;
using HospitalRegistrationSystem.Application.Services;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Entities;
using Moq;
using NUnit.Framework;

namespace HospitalRegistrationSystem.Tests.Application.Service.AppointmentServ;

[TestFixture]
public class AppointmentServiceTests
{
    [SetUp]
    public void SetUp()
    {
        _mock = new Mock<IRepositoryManager>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        _mapper = config.CreateMapper();

        _appointmentService = new AppointmentService(_mock.Object, _mapper, null, null);
    }

    private Mock<IRepositoryManager> _mock;
    private AppointmentService _appointmentService;
    private IMapper _mapper;

    [Test]
    public async Task GetIncomingByUserIdAsync_ExistingUserId_ReturnAppointments()
    {
        // Arrange
        var existingId = 1;
        var user = new ApplicationUser {Id = existingId};
        var appointments = new PagedList<Appointment>(new List<Appointment>(), 1, 1, 1);

        _mock.Setup(x => x.ApplicationUser.GetApplicationUserAsync(existingId)).ReturnsAsync(user);
        _mock.Setup(x =>
                x.Appointment.GetIncomingAppointmentsByUserIdAsync(It.IsAny<PagingParameters>(), existingId, false))
            .ReturnsAsync(appointments);

        // Act
        var result = await _appointmentService.GetIncomingByUserIdAsync(new PagingParameters(), existingId);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public async Task GetAllByUserIdAsync_ExistingUserId_ReturnAppointments()
    {
        // Arrange
        var existingId = 1;
        var user = new ApplicationUser {Id = existingId};
        var appointments = new PagedList<Appointment>(new List<Appointment>(), 1, 1, 1);

        _mock.Setup(x => x.ApplicationUser.GetApplicationUserAsync(existingId)).ReturnsAsync(user);
        _mock.Setup(x =>
                x.Appointment.GetAppointmentsByUserIdAsync(It.IsAny<PagingParameters>(), existingId, null, false))
            .ReturnsAsync(appointments);

        // Act
        var result = await _appointmentService.GetAllByUserIdAsync(new PagingParameters(), existingId);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public async Task GetMissedByUserIdAsync_ExistingUserId_ReturnAppointments()
    {
        // Arrange
        var existingId = 1;
        var user = new ApplicationUser {Id = existingId};
        var appointments = new PagedList<Appointment>(new List<Appointment>(), 1, 1, 1);

        _mock.Setup(x => x.ApplicationUser.GetApplicationUserAsync(existingId)).ReturnsAsync(user);
        _mock.Setup(x =>
                x.Appointment.GetAppointmentsByUserIdAsync(It.IsAny<PagingParameters>(), existingId, false, false))
            .ReturnsAsync(appointments);

        // Act
        var result = await _appointmentService.GetMissedByUserIdAsync(new PagingParameters(), existingId);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public async Task GetVisitedByUserIdAsync_ExistingUserId_ReturnAppointments()
    {
        // Arrange
        var existingId = 1;
        var user = new ApplicationUser {Id = existingId};
        var appointments = new PagedList<Appointment>(new List<Appointment>(), 1, 1, 1);

        _mock.Setup(x => x.ApplicationUser.GetApplicationUserAsync(existingId)).ReturnsAsync(user);
        _mock.Setup(x =>
                x.Appointment.GetAppointmentsByUserIdAsync(It.IsAny<PagingParameters>(), existingId, true, false))
            .ReturnsAsync(appointments);

        // Act
        var result = await _appointmentService.GetVisitedByUserIdAsync(new PagingParameters(), existingId);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public async Task MarkAsVisitedAsync_ExistingAppointmentId_MarkAppointmentAsVisited()
    {
        // Arrange
        var existingId = 1;
        var appointment = new Appointment {Id = existingId};

        _mock.Setup(x => x.Appointment.GetAppointmentAsync(existingId, true)).ReturnsAsync(appointment);
        _mock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _appointmentService.MarkAsVisitedAsync(existingId, "Diagnosis");

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }
}