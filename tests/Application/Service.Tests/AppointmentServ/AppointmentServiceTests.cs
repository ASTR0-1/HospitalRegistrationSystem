using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration.Conventions;
using HospitalRegistrationSystem.Application.DTOs;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Application.Interfaces.Services;
using HospitalRegistrationSystem.Application.Mappers;
using HospitalRegistrationSystem.Application.Services;
using HospitalRegistrationSystem.Domain.Entities;
using Moq;
using NUnit.Framework;
using static NUnit.Framework.Constraints.Tolerance;

namespace HospitalRegistrationSystem.Tests.Application.Service.AppointmentServ
{
    [TestFixture]
    public class AppointmentServiceTests
    {
        private Mock<IRepositoryManager> _mock;
        private IAppointmentService _appointmentService;

        [OneTimeSetUp]
        public void SetUpMock()
        {
            _mock = new Mock<IRepositoryManager>();

            MappingProfile mappingProfile = new();
            IMapper mapper = new Mapper(new MapperConfiguration(x => x.AddProfile(mappingProfile)));

            _appointmentService = new AppointmentService(_mock.Object, mapper);
        }

        [Test]
        public void AddNewAsync_NullAppointment_ThrowNullReferenceException()
        {
            // Arrange
            _mock.Setup(x => x.Appointment.CreateAppointment(It.IsAny<Appointment>()))
                .Throws(new NullReferenceException());
            Appointment nullAppointment = null;
            var expectedExceptionType = typeof(NullReferenceException);

            // Act
            var actualExceptionType = Assert.CatchAsync<NullReferenceException>(() =>
                _appointmentService.AddNewAsync(nullAppointment)).GetType();

            // Assert
            Assert.That(actualExceptionType, Is.EqualTo(expectedExceptionType));
        }

        [Test]
        public async Task AddNewAsync_NewAppointment_AddClientToDb()
        {
            // Arrange
            _mock.Setup(x => x.Appointment.CreateAppointment(It.IsAny<Appointment>()));
            string expectedDiagnosis = "D1";
            var appointment = new Appointment() { Diagnosis = expectedDiagnosis };

            // Act
            await _appointmentService.AddNewAsync(appointment);

            // Assert
            _mock.Verify(x => x.Appointment.CreateAppointment(
                It.Is<Appointment>(a => a.Diagnosis == expectedDiagnosis)));
        }

        [Test]
        public async Task GetAllAsync_GetClientAppointmentDTOs()
        {
            // Arrange
            string expectedFirstName = "cf1";
            _mock.Setup(x => x.Appointment.GetAppointmentsAsync(false))
                .ReturnsAsync(new List<Appointment>()
                {
                    new Appointment() { Doctor = new Doctor { FirstName = expectedFirstName } }
                });

            // Act
            var clientAppointment = (await _appointmentService.GetAllAsync()).Last();
            string actualFirstName = clientAppointment.DoctorFirstName;

            // Assert
            Assert.That(actualFirstName, Is.EqualTo(expectedFirstName));
        }

        [Test]
        public async Task GetAsync_NotExistingAppointmentId_ReturnNull()
        {
            // Arrange
            int notExistingId = 1;
            Appointment nullAppointment = null;
            _mock.Setup(x => x.Appointment.GetAppointmentAsync(notExistingId, false))
                .ReturnsAsync(nullAppointment);

            // Act
            var appointment = await _appointmentService.GetAsync(notExistingId);

            // Assert
            Assert.That(appointment, Is.Null);
        }

        [Test]
        public async Task GetAsync_ExistingAppointmentId_GetClientAppointmentDTO()
        {
            // Arrange
            string expectedFirstName = "cf1";
            int id = 1;
            _mock.Setup(x => x.Appointment.GetAppointmentAsync(id, false))
                .ReturnsAsync(new Appointment
                {
                    Doctor = new Doctor { FirstName = expectedFirstName }
                });

            // Act
            var appointment = await _appointmentService.GetAsync(id);
            string actualFirstName = appointment.DoctorFirstName;

            // Assert
            Assert.That(actualFirstName, Is.EqualTo(expectedFirstName));
        }

        [Test]
        public async Task GetByClientIdAsync_NotExistingId_ReturnEmptyList()
        {
            // Arrange
            int notExistingId = -1;
            _mock.Setup(x => x.Appointment.GetAppointmentsAsync(false))
                .ReturnsAsync(new List<Appointment>()
                {
                    new Appointment(),
                    new Appointment()
                });

            // Act
            var clientAppointments = await _appointmentService.GetByClientIdAsync(notExistingId);

            // Assert
            Assert.That(clientAppointments, Is.Empty);
        }

        [Test]
        public async Task GetByClientIdAsync_ExistingId_ReturnClientAppointments()
        {
            // Arrange
            var expectedDoctorIds = (1, 2);
            _mock.Setup(x => x.Appointment.GetAppointmentsAsync(false))
                .ReturnsAsync(new List<Appointment>()
                {
                    new Appointment { ClientId = 1, DoctorId = 1},
                    new Appointment { ClientId = 1, DoctorId = 2}
                });

            // Act
            var clientAppointments = (await _appointmentService.GetByClientIdAsync(1)).ToList();
            (int, int) actualDoctorIds = (clientAppointments[0].DoctorId, clientAppointments[1].DoctorId);

            // Assert
            Assert.That(expectedDoctorIds, Is.EqualTo(actualDoctorIds));
        }

        [Test]
        public async Task GetByDoctorIdAsync_NotExistingId_ReturnEmptyList()
        {
            // Arrange
            int notExistingId = -1;
            _mock.Setup(x => x.Appointment.GetAppointmentsAsync(false))
                .ReturnsAsync(new List<Appointment>()
                {
                    new Appointment(),
                    new Appointment()
                });

            // Act
            var clientAppointments = await _appointmentService.GetByDoctorIdAsync(notExistingId);

            // Assert
            Assert.That(clientAppointments, Is.Empty);
        }

        [Test]
        public async Task GetByDoctorIdAsync_ExistingId_ReturnDoctorAppointments()
        {
            // Arrange
            var expectedDoctorIds = (1, 2);
            _mock.Setup(x => x.Appointment.GetAppointmentsAsync(false))
                .ReturnsAsync(new List<Appointment>()
                {
                    new Appointment { ClientId = 1, DoctorId = 1},
                    new Appointment { ClientId = 2, DoctorId = 1}
                });

            // Act
            var clientAppointments = (await _appointmentService.GetByDoctorIdAsync(1)).ToList();
            (int, int) actualDoctorIds = (clientAppointments[0].ClientId, clientAppointments[1].ClientId);

            // Assert
            Assert.That(expectedDoctorIds, Is.EqualTo(actualDoctorIds));
        }

        [Test]
        public async Task GetVisitedByClientIdAsync_NotExistingId_ReturnEmptyList()
        {
            // Arrange
            int notExistingId = -1;
            _mock.Setup(x => x.Appointment.GetAppointmentsAsync(false))
                .ReturnsAsync(new List<Appointment>()
                {
                    new Appointment(),
                    new Appointment()
                });

            // Act
            var clientAppointments = await _appointmentService.GetVisitedByClientIdAsync(notExistingId);

            // Assert
            Assert.That(clientAppointments, Is.Empty);
        }

        [Test]
        public async Task GetVisitedByClientIdAsync_ExistingId_ReturnVisitedClientAppointments()
        {
            // Arrange
            var expectedDoctorIds = (1, 2);
            _mock.Setup(x => x.Appointment.GetAppointmentsAsync(false))
                .ReturnsAsync(new List<Appointment>()
                {
                    new Appointment { ClientId = 1, DoctorId = 1, IsVisited = true },
                    new Appointment { ClientId = 1, DoctorId = 2, IsVisited = true }
                });

            // Act
            var clientAppointments = (await _appointmentService.GetVisitedByClientIdAsync(1)).ToList();
            (int, int) actualDoctorIds = (clientAppointments[0].DoctorId, clientAppointments[1].DoctorId);

            // Assert
            Assert.That(expectedDoctorIds, Is.EqualTo(actualDoctorIds));
        }

        [Test]
        public async Task MarkAsVisitedAsync_NotExistingId_ReturnNull()
        {
            // Arrange
            int notExistingId = -1;
            Appointment nullAppointment = null;
            _mock.Setup(x => x.Appointment.GetAppointmentAsync(notExistingId, false))
                .ReturnsAsync(nullAppointment);

            // Act
            var result = await _appointmentService.MarkAsVisitedAsync(notExistingId, "");

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task MarkAsVisitedAsync_ExistingId_ReturnClientAppointment()
        {
            // Arrange
            int expectedDoctorId = 1;
            string expectedDiagnosis = "Diagnosis";
            Appointment appointment = new Appointment() { Id = expectedDoctorId, DoctorId = 1 };
            _mock.Setup(x => x.Appointment.GetAppointmentAsync(expectedDoctorId, true))
                .ReturnsAsync(appointment);

            // Act
            var clientAppointment = await _appointmentService.MarkAsVisitedAsync(expectedDoctorId, expectedDiagnosis);
            int actualDoctorId = clientAppointment.DoctorId;
            string actualDiagnosis = clientAppointment.Diagnosis;

            // Assert
            Assert.That(actualDoctorId, Is.EqualTo(expectedDoctorId));
            Assert.That(actualDiagnosis, Is.EqualTo(expectedDiagnosis));
        }
    }
}
