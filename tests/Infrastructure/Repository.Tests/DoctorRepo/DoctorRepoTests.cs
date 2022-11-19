using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Domain.Entities;
using HospitalRegistrationSystem.Infrastructure.Persistence.Repositories;
using HospitalRegistrationSystem.Tests.Infrastructure.Repository.DoctorRepo;
using NUnit.Framework;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace HospitalRegistrationSystem.Tests.Infrastructure.Repository.DoctorRepo
{
    [TestFixture]
    public class DoctorRepoTests
    {
        private IDoctorRepository _doctorRepository;
        private DoctorSeedDataFixture _dataFixture;

        [SetUp]
        public void SetUpFixture()
        {
            _dataFixture = new DoctorSeedDataFixture();
            _doctorRepository = new DoctorRepository(_dataFixture.RepositoryContext);
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
            Doctor nullDoctor = null;
            Type expectedExceptionType = typeof(ArgumentNullException);

            // Act
            Type actualExceptionType = (Assert.Catch(() => _doctorRepository.CreateDoctor(nullDoctor))).GetType();

            // Assert
            Assert.That(actualExceptionType, Is.EqualTo(expectedExceptionType));
        }

        [Test]
        public async Task Create_NotNull_AddValue()
        {
            // Arrange
            string expectedFirstName = "f3";
            Doctor doctor = new()
            {
                FirstName = "f3",
                MiddleName = "m3",
                LastName = "l3",
                Gender = "g3",
                Specialty = "s3"
            };

            // Act
            _doctorRepository.CreateDoctor(doctor);
            await _dataFixture.RepositoryContext.SaveChangesAsync();
            string actualFirstName = (await _doctorRepository.GetDoctorsAsync(trackChanges: false)).Last().FirstName;

            // Assert
            Assert.That(actualFirstName, Is.EqualTo(expectedFirstName));
        }

        [Test]
        public void Delete_Null_ThrowArgumentNullException()
        {
            // Arrange
            Doctor nullDoctor = null;
            Type expectedExceptionType = typeof(ArgumentNullException);

            // Act
            Type actualExceptionType = (Assert.Catch(() => _doctorRepository.DeleteDoctor(nullDoctor))).GetType();

            // Assert
            Assert.That(actualExceptionType, Is.EqualTo(expectedExceptionType));
        }

        [Test]
        public async Task Delete_Existing_DeleteValue()
        {
            // Arrange
            string expectedFirstName = "f1";
            var doctorToDelete = await _doctorRepository.GetDoctorAsync(2, true);

            // Act
            _doctorRepository.DeleteDoctor(doctorToDelete);
            await _dataFixture.RepositoryContext.SaveChangesAsync();
            string actualDiagnosis = (await _doctorRepository.GetDoctorsAsync(false)).Last().FirstName;

            // Assert
            Assert.That(actualDiagnosis, Is.EqualTo(expectedFirstName));
        }

        [Test]
        public async Task GetDoctorsAsync_GetValue()
        {
            // Arrange
            int expectedCount = 2;

            // Act
            int actualCount = (await _doctorRepository.GetDoctorsAsync(false)).Count();

            // Assert
            Assert.That(actualCount, Is.EqualTo(expectedCount));
        }

        [Test]
        public async Task GetDoctorAsync_NotExistingId_GetNull()
        {
            // Arrange
            int notExistingId = -1;
            Appointment expectedAppointment = null;

            // Act
            var actualDoctor = await _doctorRepository.GetDoctorAsync(notExistingId, trackChanges: false);

            // Assert
            Assert.That(actualDoctor, Is.EqualTo(expectedAppointment));
        }

        [Test]
        public async Task GetDoctorAsync_ExistingId_GetValue()
        {
            // Arrange
            int expectedId = 1;

            // Act
            int actualId = (await _doctorRepository.GetDoctorAsync(expectedId, trackChanges: false)).Id;

            // Assert
            Assert.That(actualId, Is.EqualTo(expectedId));
        }
    }
}
