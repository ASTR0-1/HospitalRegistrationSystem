using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using HospitalRegistrationSystem.Application.Interfaces;
using HospitalRegistrationSystem.Application.Interfaces.DTOs;
using HospitalRegistrationSystem.Application.Interfaces.Services;
using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace HospitalRegistrationSystem.WebUI.Controllers
{
    [Route("api/appointments")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        private readonly IClientService _clientService;
        private readonly IAppointmentService _appointmentsService;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IValidator<AppointmentForCreationDTO> _validator;

        public AppointmentsController(IDoctorService doctorService, IClientService clientService, IAppointmentService appointmentService,
            ILoggerManager logger, IMapper mapper, IValidator<AppointmentForCreationDTO> validator)
        {
            _doctorService = doctorService;
            _clientService = clientService;
            _appointmentsService = appointmentService;
            _logger = logger;
            _mapper = mapper;
            _validator = validator;
        }

        [HttpGet("client/{clientId}", Name = "ClientAppointments")]
        public async Task<IActionResult> GetClientAppointments(int clientId)
        {
            ClientCardDTO client = (await _clientService.GetAllAsync())
                .Where(e => e.Id == clientId)
                .FirstOrDefault();
            
            if (client == null)
            {
                _logger.LogInformation($"Client with id: {clientId} doesn't exist in the database.");

                return NotFound();
            }

            IEnumerable<ClientAppointmentDTO> clientAppointments = await _appointmentsService.GetByClientIdAsync(clientId);

            if (!clientAppointments.Any())
            {
                _logger.LogInformation($"Appointments with given clientId: '{clientId}' doesn't exist in the database.");

                return NotFound();
            }

            return Ok(clientAppointments);
        }

        [HttpGet("doctor/{doctorId}", Name = "DoctorAppointments")]
        public async Task<IActionResult> GetDoctorAppointments(int doctorId)
        {
            DoctorCardDTO doctor = (await _doctorService.GetAllAsync())
                .Where(e => e.Id == doctorId)
                .FirstOrDefault();

            if (doctor == null)
            {
                _logger.LogInformation($"Doctor with id: {doctorId} doesn't exist in the database.");

                return NotFound();
            }

            IEnumerable<DoctorAppointmentDTO> doctorAppointments = await _appointmentsService.GetByDoctorIdAsync(doctorId);

            if (!doctorAppointments.Any())
            {
                _logger.LogInformation($"Appointments with given clientId: '{doctorId}' doesn't exist in the database.");

                return NotFound();
            }

            return Ok(doctorAppointments);
        }

        [HttpPost]
        public async Task<IActionResult> PostAppointment([FromBody] AppointmentForCreationDTO appointmentDto)
        {
            if (appointmentDto == null)
            {
                _logger.LogError("AppointmentForCreationDTO object sent from a client is null");

                return BadRequest("AppointmentForCreationDTO object is null");
            }

            ValidationResult result = await _validator.ValidateAsync(appointmentDto);

            if (!result.IsValid)
            {
                _logger.LogError("Invalid model state for the AppointmentForCreationDTO object");

                return UnprocessableEntity(result.Errors);
            }

            var appointmentEntity = _mapper.Map<Appointment>(appointmentDto);

            await _appointmentsService.AddNewAsync(appointmentEntity);

            return CreatedAtRoute("ClientAppointments", new { clientId = appointmentDto.ClientId },
                appointmentDto);
        }

        [HttpPut("{appointmentId}/markAsVisited")]
        public async Task<IActionResult> PutVisitedAppointment(int appointmentId, [FromQuery] string diagnosis)
        {
            ClientAppointmentDTO appointment = (await _appointmentsService.GetAllAsync())
                .Where(e => e.Id == appointmentId)
                .FirstOrDefault();

            if (appointment == null)
            {
                _logger.LogInformation($"Appointment with id: {appointmentId} doesn't exist in the database.");

                return NotFound();
            }

            ClientAppointmentDTO appointmnetDto = await _appointmentsService.MarkAsVisitedAsync(appointmentId, diagnosis);

            return Ok(appointmnetDto);
        }
    }
}
