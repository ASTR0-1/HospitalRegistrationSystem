using System;
using System.Linq;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Domain.Entities;
using HospitalRegistrationSystem.Infrastructure.Persistence.Repositories;
using NUnit.Framework;

namespace HospitalRegistrationSystem.Tests.Infrastructure.Repository.ClientRepo;

[TestFixture]
public class ClientRepoTests
{
    [SetUp]
    public void SetUpFixture()
    {
        _dataFixture = new ClientSeedDataFixture();
        _clientRepository = new ClientRepository(_dataFixture.RepositoryContext);
    }

    [TearDown]
    public void TearDownFixture()
    {
        _dataFixture.Dispose();
    }

    private IClientRepository _clientRepository;
    private ClientSeedDataFixture _dataFixture;

    [Test]
    public void Create_Null_ThrowArgumentNullException()
    {
        // Arrange
        Client nullClient = null;
        var expectedExceptionType = typeof(ArgumentNullException);

        // Act
        var actualExceptionType = Assert.Catch(() => _clientRepository.CreateClient(nullClient)).GetType();

        // Assert
        Assert.That(actualExceptionType, Is.EqualTo(expectedExceptionType));
    }

    [Test]
    public async Task Create_NotNull_AddValue()
    {
        // Arrange
        var expectedFirstName = "f3";
        Client client = new()
        {
            FirstName = "f3",
            MiddleName = "m3",
            LastName = "l3",
            Gender = "g3"
        };

        // Act
        _clientRepository.CreateClient(client);
        await _dataFixture.RepositoryContext.SaveChangesAsync();
        var actualFirstName = (await _clientRepository.GetClientsAsync(false)).Last().FirstName;

        // Assert
        Assert.That(actualFirstName, Is.EqualTo(expectedFirstName));
    }

    [Test]
    public void Delete_Null_ThrowArgumentNullException()
    {
        // Arrange
        Client nullClient = null;
        var expectedExceptionType = typeof(ArgumentNullException);

        // Act
        var actualExceptionType = Assert.Catch(() => _clientRepository.DeleteClient(nullClient)).GetType();

        // Assert
        Assert.That(actualExceptionType, Is.EqualTo(expectedExceptionType));
    }

    [Test]
    public async Task Delete_Existing_DeleteValue()
    {
        // Arrange
        var expectedFirstName = "f1";
        var clientToDelete = await _clientRepository.GetClientAsync(2, true);

        // Act
        _clientRepository.DeleteClient(clientToDelete);
        await _dataFixture.RepositoryContext.SaveChangesAsync();
        var actualDiagnosis = (await _clientRepository.GetClientsAsync(false)).Last().FirstName;

        // Assert
        Assert.That(actualDiagnosis, Is.EqualTo(expectedFirstName));
    }

    [Test]
    public async Task GetClientsAsync_GetValue()
    {
        // Arrange
        var expectedCount = 2;

        // Act
        var actualCount = (await _clientRepository.GetClientsAsync(false)).Count();

        // Assert
        Assert.That(actualCount, Is.EqualTo(expectedCount));
    }

    [Test]
    public async Task GetClientAsync_NotExistingId_GetNull()
    {
        // Arrange
        var notExistingId = -1;
        Appointment expectedAppointment = null;

        // Act
        var actualClient = await _clientRepository.GetClientAsync(notExistingId, false);

        // Assert
        Assert.That(actualClient, Is.EqualTo(expectedAppointment));
    }

    [Test]
    public async Task GetClientAsync_ExistingId_GetValue()
    {
        // Arrange
        var expectedId = 1;

        // Act
        var actualId = (await _clientRepository.GetClientAsync(expectedId, false)).Id;

        // Assert
        Assert.That(actualId, Is.EqualTo(expectedId));
    }
}