using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HospitalRegistrationSystem.Application.DTOs.ApplicationUserDTOs;
using HospitalRegistrationSystem.Application.Interfaces;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Application.Mappers;
using HospitalRegistrationSystem.Application.Services;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace HospitalRegistrationSystem.Tests.Application.Service.ApplicationUserServ;

[TestFixture]
public class ApplicationUserServiceTests
{
    private Mock<IRepositoryManager> _mock;
    private Mock<ICurrentUserService> _mockUserService;
    private Mock<IConfiguration> _mockConfiguration;
    private ApplicationUserService _applicationUserService;
    private IMapper _mapper;

    [SetUp]
    public void SetUp()
    {
        _mock = new Mock<IRepositoryManager>();
        _mockUserService = new Mock<ICurrentUserService>();
        _mockConfiguration = new Mock<IConfiguration>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
            cfg.CreateMap(typeof(PagedList<>), typeof(PagedList<>))
                .ConvertUsing(typeof(PagedListConverter<,>));
        });

        _mapper = config.CreateMapper();

        _applicationUserService = new ApplicationUserService(_mock.Object, _mapper, _mockUserService.Object, _mockConfiguration.Object);
    }

    [Test]
    public async Task GetAsync_ValidUserId_ReturnsUser()
    {
        // Arrange
        _mock.Setup(x => x.ApplicationUser.GetApplicationUserAsync(It.IsAny<int>())).ReturnsAsync(new ApplicationUser());

        // Act
        var result = await _applicationUserService.GetAsync(1);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public async Task GetAllAsync_ValidParameters_ReturnsUsers()
    {
        // Arrange
        var applicationUsers = new PagedList<ApplicationUser>(new List<ApplicationUser>(), 1, 1, 1);
        _mock.Setup(x => x.ApplicationUser.GetApplicationUsersAsync(It.IsAny<PagingParameters>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int?>())).ReturnsAsync(applicationUsers);

        // Act
        var result = await _applicationUserService.GetAllAsync(new PagingParameters(), null, "role");

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public async Task UpdateAsync_ValidUser_UpdatesUser()
    {
        // Arrange
        var applicationUserDto = new ApplicationUserDto { Id = 1 };
        _mockUserService.Setup(x => x.GetApplicationUserId()).Returns(1);
        _mock.Setup(x => x.ApplicationUser.GetApplicationUserAsync(It.IsAny<int>())).ReturnsAsync(new ApplicationUser());
        _mock.Setup(x => x.ApplicationUser.UpdateApplicationUserAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _applicationUserService.UpdateAsync(applicationUserDto);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public async Task AssignEmployeeAsync_ValidParameters_AssignsEmployee()
    {
        // Arrange
        _mockUserService.Setup(x => x.GetApplicationUserId()).Returns(1);
        _mockUserService.Setup(x => x.IsInRole(It.IsAny<string>())).Returns(true);
        _mock.Setup(x => x.ApplicationUser.GetApplicationUserAsync(It.IsAny<int>())).ReturnsAsync(new ApplicationUser());
        _mock.Setup(x => x.Hospital.GetHospitalAsync(It.IsAny<int>(), false)).ReturnsAsync(new Hospital());
        _mock.Setup(x => x.ApplicationUser.AssignUserToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
        _mock.Setup(x => x.ApplicationUser.UpdateApplicationUserAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _applicationUserService.AssignEmployeeAsync(1, "Doctor", 1, "specialty", 100);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }
}