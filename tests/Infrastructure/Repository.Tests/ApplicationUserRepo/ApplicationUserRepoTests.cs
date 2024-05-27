using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Entities;
using HospitalRegistrationSystem.Infrastructure.Persistence.Repositories;
using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Identity;

namespace HospitalRegistrationSystem.Tests.Infrastructure.Repository.ApplicationUserRepo;

[TestFixture]
public class ApplicationUserRepoTests
{
    private Mock<UserManager<ApplicationUser>> _userManager;
    private IApplicationUserRepository _applicationUserRepository;
    private List<ApplicationUser> _users;

    [SetUp]
    public void SetUpFixture()
    {
        var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
        _userManager = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
        _applicationUserRepository = new ApplicationUserRepository(_userManager.Object);

        _users =
        [
            new ApplicationUser {Id = 1, UserName = "User1"},
            new ApplicationUser {Id = 2, UserName = "User2"}
        ];

        _userManager.Setup(x => x.GetUsersInRoleAsync(It.IsAny<string>()))
            .ReturnsAsync(_users);
        _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .Returns<string>(id => Task.FromResult(_users.FirstOrDefault(x => x.Id.ToString() == id)));
        _userManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(IdentityResult.Success);
        _userManager.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
    }

    [Test]
    public async Task GetApplicationUsersAsync_GetValue()
    {
        // Arrange
        var expectedCount = 2;

        // Act
        var actualCount = (await _applicationUserRepository.GetApplicationUsersAsync(new PagingParameters(), null, "Role")).Count;

        // Assert
        Assert.That(actualCount, Is.EqualTo(expectedCount));
    }

    [Test]
    public async Task GetApplicationUserAsync_NotExistingId_GetNull()
    {
        // Arrange
        var notExistingId = 0;
        ApplicationUser expectedUser = null;

        // Act
        var actualUser = await _applicationUserRepository.GetApplicationUserAsync(notExistingId);

        // Assert
        Assert.That(actualUser, Is.EqualTo(expectedUser));
    }

    [Test]
    public async Task GetApplicationUserAsync_ExistingId_GetValue()
    {
        // Arrange
        var expectedId = 1;

        // Act
        var actualId = (await _applicationUserRepository.GetApplicationUserAsync(expectedId)).Id;

        // Assert
        Assert.That(actualId, Is.EqualTo(expectedId));
    }

    [Test]
    public async Task UpdateApplicationUserAsync_Success()
    {
        // Arrange
        var user = new ApplicationUser { Id = 1, UserName = "User1" };

        // Act
        var result = await _applicationUserRepository.UpdateApplicationUserAsync(user);

        // Assert
        Assert.That(result.Succeeded, Is.True);
    }

    [Test]
    public async Task AssignUserToRoleAsync_Success()
    {
        // Arrange
        var user = await _applicationUserRepository.GetApplicationUserAsync(1);
        var role = "Role";

        // Act
        var result = await _applicationUserRepository.AssignUserToRoleAsync(user, role);

        // Assert
        Assert.That(result.Succeeded, Is.True);
    }
}
