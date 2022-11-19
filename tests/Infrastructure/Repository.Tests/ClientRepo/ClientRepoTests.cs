using System.Threading.Tasks;
using System;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Domain.Entities;
using HospitalRegistrationSystem.Infrastructure.Persistence.Repositories;
using NUnit.Framework;
using System.Linq;

namespace HospitalRegistrationSystem.Tests.Infrastructure.Repository.ClientRepo
{
    [TestFixture]
    public class ClientRepoTests
    {
        private IClientRepository _clientRepository;
        private ClientSeedDataFixture _dataFixture;

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

        [Test]
        public void Create_Null_ThrowArgumentNullException()
        {
            // Arrange
            Client nullClient = null;
            Type expectedExceptionType = typeof(ArgumentNullException);

            // Act
            Type actualExceptionType = (Assert.Catch(() => _clientRepository.CreateClient(nullClient))).GetType();

            // Assert
            Assert.That(actualExceptionType, Is.EqualTo(expectedExceptionType));
        }

        [Test]
        public async Task Create_NotNull_AddValue()
        {
            // Arrange
            string expectedFirstName = "f3";
            Client client = new() {
                FirstName = "f3",
                MiddleName = "m3",
                LastName = "l3",
                Gender = "g3"
            };

            // Act
            _clientRepository.CreateClient(client);
            await _dataFixture.RepositoryContext.SaveChangesAsync();
            string actualFirstName = (await _clientRepository.GetClientsAsync(trackChanges: false)).Last().FirstName;

            // Assert
            Assert.That(actualFirstName, Is.EqualTo(expectedFirstName));
        }

        [Test]
        public void Delete_Null_ThrowArgumentNullException()
        {
            // Arrange
            Client nullClient = null;
            Type expectedExceptionType = typeof(ArgumentNullException);

            // Act
            Type actualExceptionType = (Assert.Catch(() => _clientRepository.DeleteClient(nullClient))).GetType();

            // Assert
            Assert.That(actualExceptionType, Is.EqualTo(expectedExceptionType));
        }

        [Test]
        public async Task Delete_Existing_DeleteValue()
        {
            // Arrange
            string expectedFirstName = "f1";
            var clientToDelete = await _clientRepository.GetClientAsync(2, true);

            // Act
            _clientRepository.DeleteClient(clientToDelete);
            await _dataFixture.RepositoryContext.SaveChangesAsync();
            string actualDiagnosis = (await _clientRepository.GetClientsAsync(false)).Last().FirstName;

            // Assert
            Assert.That(actualDiagnosis, Is.EqualTo(expectedFirstName));
        }

        [Test]
        public async Task GetClientsAsync_GetValue()
        {
            // Arrange
            int expectedCount = 2;

            // Act
            int actualCount = (await _clientRepository.GetClientsAsync(false)).Count();

            // Assert
            Assert.That(actualCount, Is.EqualTo(expectedCount));
        }

        [Test]
        public async Task GetClientAsync_NotExistingId_GetNull()
        {
            // Arrange
            int notExistingId = -1;
            Appointment expectedAppointment = null;

            // Act
            var actualClient = await _clientRepository.GetClientAsync(notExistingId, trackChanges: false);

            // Assert
            Assert.That(actualClient, Is.EqualTo(expectedAppointment));
        }

        [Test]
        public async Task GetClientAsync_ExistingId_GetValue()
        {
            // Arrange
            int expectedId = 1;

            // Act
            int actualId = (await _clientRepository.GetClientAsync(expectedId, trackChanges: false)).Id;

            // Assert
            Assert.That(actualId, Is.EqualTo(expectedId));
        }
    }
}
