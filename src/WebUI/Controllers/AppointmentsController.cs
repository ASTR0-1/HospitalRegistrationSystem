using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using HospitalRegistrationSystem.Application.Interfaces;
using HospitalRegistrationSystem.Application.Interfaces.DTOs;
using HospitalRegistrationSystem.Application.Interfaces.Services;
using HospitalRegistrationSystem.Application.Utility;
using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HospitalRegistrationSystem.WebUI.Controllers;

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
    public async Task<IActionResult> GetClientAppointments(int clientId,
        [FromQuery] PagingParameters pagingParameters)
    {
        ClientCardDTO client = await _clientService.GetAsync(clientId);

        if (client == null)
        {
            _logger.LogInformation($"Client with id: {clientId} doesn't exist in the database.");

            return NotFound($"Client with id: {clientId} doesn't exist");
        }

        IEnumerable<ClientAppointmentDTO> clientAppointments = await _appointmentsService.GetByClientIdAsync(clientId);

        var pagedClientAppointments = PagedList<ClientAppointmentDTO>
            .ToPagedList(clientAppointments, pagingParameters.PageNumber, pagingParameters.PageSize);
        Response.Headers.Add("X-Pagination",
            JsonConvert.SerializeObject(pagedClientAppointments.MetaData));

        return Ok(pagedClientAppointments);
    }

    [HttpGet("client/{clientId}/visited")]
    public async Task<IActionResult> GetVisitedClientAppointments(int clientId,
        [FromQuery] PagingParameters pagingParameters)
    {
        ClientCardDTO client = await _clientService.GetAsync(clientId);

        if (client == null)
        {
            _logger.LogInformation($"Client with id: {clientId} doesn't exist in the database.");

            return NotFound($"Client with id: {clientId} doesn't exist");
        }

        IEnumerable<ClientAppointmentCardDTO> clientAppointmentCards = await _appointmentsService.GetVisitedByClientIdAsync(clientId);

        var pagedClientAppointments = PagedList<ClientAppointmentCardDTO>
            .ToPagedList(clientAppointmentCards, pagingParameters.PageNumber, pagingParameters.PageSize);
        Response.Headers.Add("X-Pagination",
            JsonConvert.SerializeObject(pagedClientAppointments.MetaData));

        return Ok(pagedClientAppointments);
    }

    [HttpGet("doctor/{doctorId}", Name = "DoctorAppointments")]
    public async Task<IActionResult> GetDoctorAppointments(int doctorId,
        [FromQuery] PagingParameters pagingParameters)
    {
        DoctorCardDTO doctor = await _doctorService.GetAsync(doctorId);

        if (doctor == null)
        {
            _logger.LogInformation($"Doctor with id: {doctorId} doesn't exist in the database.");

            return NotFound($"Doctor with id: {doctorId} doesn't exist");
        }

        IEnumerable<DoctorAppointmentDTO> doctorAppointments = await _appointmentsService.GetByDoctorIdAsync(doctorId);

        var pagedDoctorAppointments = PagedList<DoctorAppointmentDTO>
            .ToPagedList(doctorAppointments, pagingParameters.PageNumber, pagingParameters.PageSize);

        Response.Headers.Add("X-Pagination",
            JsonConvert.SerializeObject(pagedDoctorAppointments.MetaData));

        return Ok(pagedDoctorAppointments);
    }

    [HttpPost]
    public async Task<IActionResult> PostAppointment([FromBody] AppointmentForCreationDTO appointmentDto)
    {
        if (appointmentDto == null)
        {
            _logger.LogError("AppointmentForCreationDTO object sent from a client is null.");

            return BadRequest("AppointmentForCreationDTO object is null");
        }

        var client = await _clientService.GetAsync(appointmentDto.ClientId);
        var doctor = await _doctorService.GetAsync(appointmentDto.DoctorId);

        if (client == null || doctor == null)
        {
            _logger.LogError("Doctor or Client with given id doesn't exist in the database.");

            return BadRequest("Doctor or Client with given id doesn't exist");
        }

        ValidationResult result = await _validator.ValidateAsync(appointmentDto);

        if (!result.IsValid)
        {
            _logger.LogError("Invalid model state for the AppointmentForCreationDTO object.");

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
        ClientAppointmentDTO appointment = await _appointmentsService.GetAsync(appointmentId);

        if (appointment == null)
        {
            _logger.LogInformation($"Appointment with id: {appointmentId} doesn't exist in the database.");

            return NotFound($"Appointment with id: {appointmentId} doesn't exist");
        }

        ClientAppointmentDTO appointmnetDto = await _appointmentsService.MarkAsVisitedAsync(appointmentId, diagnosis);

        return Ok(appointmnetDto);
    }
}
