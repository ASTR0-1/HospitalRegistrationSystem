using AutoMapper;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Application.Interfaces.Services;
using HospitalRegistrationSystem.Application.Mappers;
using HospitalRegistrationSystem.Application.Services;
using HospitalRegistrationSystem.Domain.Entities;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace HospitalRegistrationSystem.Tests.Application.Service.ClientServ
{
    [TestFixture]
    public class ClientServiceTests
    {
        private Mock<IRepositoryManager> _mock;
        private IClientService _clientService;

        [OneTimeSetUp]
        public void SetUpMock()
        {
            _mock = new Mock<IRepositoryManager>();

            MappingProfile mappingProfile = new();
            IMapper mapper = new Mapper(new MapperConfiguration(x => x.AddProfile(mappingProfile)));

            _clientService = new ClientService(_mock.Object, mapper);
        }

        [Test]
        public void AddNewAsync_NullClient_ThrowNullReferenceException()
        {
            // Arrange
            _mock.Setup(x => x.Client.CreateClient(It.IsAny<Client>()))
                .Throws(new NullReferenceException());
            Client nullClient = null;
            var expectedExceptionType = typeof(NullReferenceException);

            // Act
            var actualExceptionType = Assert.CatchAsync<NullReferenceException>(() =>
                _clientService.AddNewAsync(nullClient)).GetType();

            // Assert
            Assert.That(actualExceptionType, Is.EqualTo(expectedExceptionType));
        }

        [Test]
        public async Task AddNewAsync_NewClient_AddClientToDb()
        {
            // Arrange
            _mock.Setup(x => x.Client.CreateClient(It.IsAny<Client>()));
            string expectedGender = "G1";
            var client = new Client() { Gender = expectedGender };

            // Act
            await _clientService.AddNewAsync(client);

            // Assert
            _mock.Verify(x => x.Client.CreateClient(
                It.Is<Client>(a => a.Gender == expectedGender)));
        }

        [Test]
        public async Task GetAllAsync_GetClientCardDTOs()
        {
            // Arrange
            string expectedFirstName = "cf1";
            _mock.Setup(x => x.Client.GetClientsAsync(false))
                .ReturnsAsync(new List<Client>()
                {
                    new Client() { FirstName = "cf1" }
                });

            // Act
            var clientCard = (await _clientService.GetAllAsync()).Last();
            string actualFirstName = clientCard.FirstName;

            // Assert
            Assert.That(actualFirstName, Is.EqualTo(expectedFirstName));
        }

        [Test]
        public async Task GetAsync_NotExistingClientId_ReturnNull()
        {
            // Arrange
            int notExistingId = 1;
            Client nullClient = null;
            _mock.Setup(x => x.Client.GetClientAsync(notExistingId, false))
                .ReturnsAsync(nullClient);

            // Act
            var clientCard = await _clientService.GetAsync(notExistingId);

            // Assert
            Assert.That(clientCard, Is.Null);
        }

        [Test]
        public async Task GetAsync_ExistingClientId_GetClientCardDTO()
        {
            // Arrange
            string expectedFirstName = "cf1";
            int id = 1;
            _mock.Setup(x => x.Client.GetClientAsync(id, false))
                .ReturnsAsync(new Client
                {
                    FirstName = expectedFirstName
                });

            // Act
            var clientCard = await _clientService.GetAsync(id);
            string actualFirstName = clientCard.FirstName;

            // Assert
            Assert.That(actualFirstName, Is.EqualTo(expectedFirstName));
        }

        [Test]
        public async Task FindAsync_NotExistingClientSearchString_ReturnNull()
        {
            // Arrange
            string notExistingClientSearchString = "fjkadlsfjalksd";
            _mock.Setup(x => x.Client.GetClientsAsync(false))
                .ReturnsAsync(new List<Client>()
                {
                    new Client() { FirstName = "f1" },
                    new Client() { FirstName = "f2" }
                });

            // Act
            var clientCard = (await _clientService.FindAsync(notExistingClientSearchString)).FirstOrDefault();

            // Assert
            Assert.That(clientCard, Is.Null);
        }

        [Test]
        public async Task FindAsync_ExistingClientSearchString_GetClientCard()
        {
            // Arrange
            string expectedFirstName = "cfirstname1";
            _mock.Setup(x => x.Client.GetClientsAsync(false))
                .ReturnsAsync(new List<Client>()
                {
                    new Client() { FirstName = expectedFirstName },
                    new Client() { FirstName = "" }
                });

            // Act
            var clientCard = (await _clientService.FindAsync("cfirst")).FirstOrDefault();
            var actualFirstName = clientCard.FirstName;

            // Assert
            Assert.That(actualFirstName, Is.EqualTo(expectedFirstName));
        }
    }
}
