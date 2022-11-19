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

namespace HospitalRegistrationSystem.Tests.Application.Service.DoctorServ
{
    [TestFixture]
    public class DoctorServiceTests
    {
        private Mock<IRepositoryManager> _mock;
        private IDoctorService _doctorService;

        [OneTimeSetUp]
        public void SetUpMock()
        {
            _mock = new Mock<IRepositoryManager>();

            MappingProfile mappingProfile = new();
            IMapper mapper = new Mapper(new MapperConfiguration(x => x.AddProfile(mappingProfile)));

            _doctorService = new DoctorService(_mock.Object, mapper);
        }

        [Test]
        public void AddNewAsync_NullDoctor_ThrowNullReferenceException()
        {
            // Arrange
            _mock.Setup(x => x.Doctor.CreateDoctor(It.IsAny<Doctor>()))
                .Throws(new NullReferenceException());
            Doctor nullDoctor = null;
            var expectedExceptionType = typeof(NullReferenceException);

            // Act
            var actualExceptionType = Assert.CatchAsync<NullReferenceException>(() =>
                _doctorService.AddNewAsync(nullDoctor)).GetType();

            // Assert
            Assert.That(actualExceptionType, Is.EqualTo(expectedExceptionType));
        }

        [Test]
        public async Task AddNewAsync_NewDoctor_AddDoctorToDb()
        {
            // Arrange
            _mock.Setup(x => x.Doctor.CreateDoctor(It.IsAny<Doctor>()));
            string expectedGender = "G1";
            var doctor = new Doctor() { Gender = expectedGender };

            // Act
            await _doctorService.AddNewAsync(doctor);

            // Assert
            _mock.Verify(x => x.Doctor.CreateDoctor(
                It.Is<Doctor>(a => a.Gender == expectedGender)));
        }

        [Test]
        public async Task GetAllAsync_GetDoctorCardDTOs()
        {
            // Arrange
            string expectedFirstName = "cf1";
            _mock.Setup(x => x.Doctor.GetDoctorsAsync(false))
                .ReturnsAsync(new List<Doctor>()
                {
                    new Doctor() { FirstName = "cf1" }
                });

            // Act
            var doctorCard = (await _doctorService.GetAllAsync()).Last();
            string actualFirstName = doctorCard.FirstName;

            // Assert
            Assert.That(actualFirstName, Is.EqualTo(expectedFirstName));
        }

        [Test]
        public async Task GetAsync_NotExistingDoctorId_ReturnNull()
        {
            // Arrange
            int notExistingId = 1;
            Doctor nullDoctor = null;
            _mock.Setup(x => x.Doctor.GetDoctorAsync(notExistingId, false))
                .ReturnsAsync(nullDoctor);

            // Act
            var doctorCard = await _doctorService.GetAsync(notExistingId);

            // Assert
            Assert.That(doctorCard, Is.Null);
        }

        [Test]
        public async Task GetAsync_ExistingDoctorId_GetDoctorCardDTO()
        {
            // Arrange
            string expectedFirstName = "cf1";
            int id = 1;
            _mock.Setup(x => x.Doctor.GetDoctorAsync(id, false))
                .ReturnsAsync(new Doctor
                {
                    FirstName = expectedFirstName
                });

            // Act
            var doctorCard = await _doctorService.GetAsync(id);
            string actualFirstName = doctorCard.FirstName;

            // Assert
            Assert.That(actualFirstName, Is.EqualTo(expectedFirstName));
        }

        [Test]
        public async Task FindAsync_NotExistingDoctorSearchString_ReturnNull()
        {
            // Arrange
            string notExistingDoctorSearchString = "fjkadlsfjalksd";
            _mock.Setup(x => x.Doctor.GetDoctorsAsync(false))
                .ReturnsAsync(new List<Doctor>()
                {
                    new Doctor() { FirstName = "f1" },
                    new Doctor() { FirstName = "f2" }
                });

            // Act
            var doctorCard = (await _doctorService.FindAsync(notExistingDoctorSearchString)).FirstOrDefault();

            // Assert
            Assert.That(doctorCard, Is.Null);
        }

        [Test]
        public async Task FindAsync_ExistingDoctorSearchString_GetDoctorCard()
        {
            // Arrange
            string expectedFirstName = "cfirstname1";
            _mock.Setup(x => x.Doctor.GetDoctorsAsync(false))
                .ReturnsAsync(new List<Doctor>()
                {
                    new Doctor() { FirstName = expectedFirstName },
                    new Doctor() { FirstName = "" }
                });

            // Act
            var doctorCard = (await _doctorService.FindAsync("cfirst")).FirstOrDefault();
            var actualFirstName = doctorCard.FirstName;

            // Assert
            Assert.That(actualFirstName, Is.EqualTo(expectedFirstName));
        }
    }
}
