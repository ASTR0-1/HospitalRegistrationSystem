using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HospitalRegistrationSystem.Application.DTOs.DoctorScheduleDTOs;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Application.Interfaces;
using HospitalRegistrationSystem.Application.Mappers;
using HospitalRegistrationSystem.Application.Services;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Entities;
using Moq;
using NUnit.Framework;

namespace HospitalRegistrationSystem.Tests.Application.Service.DoctorScheduleServ;

[TestFixture]
public class DoctorScheduleServiceTests
{
    private Mock<IRepositoryManager> _mock;
    private Mock<ICurrentUserService> _mockUserService;
    private DoctorScheduleService _doctorScheduleService;
    private IMapper _mapper;

    [SetUp]
    public void SetUp()
    {
        _mock = new Mock<IRepositoryManager>();
        _mockUserService = new Mock<ICurrentUserService>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
            cfg.CreateMap(typeof(PagedList<>), typeof(PagedList<>))
                .ConvertUsing(typeof(PagedListConverter<,>));
        });

        _mapper = config.CreateMapper();

        _doctorScheduleService = new DoctorScheduleService(_mapper, _mock.Object, _mockUserService.Object);
    }

    [Test]
    public async Task GetDoctorSchedulesAsync_WhenCalled_ReturnsDoctorSchedules()
    {
        // Arrange
        var doctorSchedules = new PagedList<DoctorSchedule>(new List<DoctorSchedule>(), 1, 1, 1);
        _mock.Setup(x => x.ApplicationUser.GetApplicationUserAsync(It.IsAny<int>())).ReturnsAsync(new ApplicationUser());
        _mock.Setup(x => x.DoctorSchedule.GetDoctorSchedulesAsync(It.IsAny<DoctorScheduleParameters>(), It.IsAny<int>(), false)).ReturnsAsync(doctorSchedules);

        // Act
        var result = await _doctorScheduleService.GetDoctorSchedulesAsync(new DoctorScheduleParameters(), 1);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public async Task CreateDoctorSchedule_ValidDoctorSchedule_CreatesDoctorSchedule()
    {
        // Arrange
        var doctorScheduleDto = new DoctorScheduleForManipulationDto { Id = 1 };
        _mockUserService.Setup(x => x.GetApplicationUserId()).Returns(1);
        _mock.Setup(x => x.DoctorSchedule.CreateDoctorSchedule(It.IsAny<DoctorSchedule>()));
        _mock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _doctorScheduleService.CreateDoctorSchedule(doctorScheduleDto);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public async Task UpdateDoctorSchedule_ValidDoctorSchedule_UpdatesDoctorSchedule()
    {
        // Arrange
        var doctorScheduleDto = new DoctorScheduleForManipulationDto { Id = 1 };
        var doctorSchedule = new DoctorSchedule { DoctorId = 1 };
        _mockUserService.Setup(x => x.GetApplicationUserId()).Returns(1);
        _mock.Setup(x => x.DoctorSchedule.GetDoctorScheduleByIdAsync(It.IsAny<int>(), false)).ReturnsAsync(doctorSchedule);
        _mock.Setup(x => x.DoctorSchedule.UpdateDoctorSchedule(It.IsAny<DoctorSchedule>()));
        _mock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _doctorScheduleService.UpdateDoctorSchedule(doctorScheduleDto);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public async Task DeleteDoctorSchedule_ValidDoctorSchedule_DeletesDoctorSchedule()
    {
        // Arrange
        var doctorSchedule = new DoctorSchedule { DoctorId = 1 };
        _mockUserService.Setup(x => x.GetApplicationUserId()).Returns(1);
        _mock.Setup(x => x.DoctorSchedule.GetDoctorScheduleByIdAsync(It.IsAny<int>(), false)).ReturnsAsync(doctorSchedule);
        _mock.Setup(x => x.DoctorSchedule.DeleteDoctorSchedule(It.IsAny<DoctorSchedule>()));
        _mock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _doctorScheduleService.DeleteDoctorSchedule(1);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }
}
