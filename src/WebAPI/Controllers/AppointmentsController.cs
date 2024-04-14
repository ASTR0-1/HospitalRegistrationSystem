using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using HospitalRegistrationSystem.Application.DTOs.AppointmentDTOs;
using HospitalRegistrationSystem.Application.Interfaces;
using HospitalRegistrationSystem.Application.Interfaces.Services;
using HospitalRegistrationSystem.Application.Utility;
using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HospitalRegistrationSystem.WebAPI.Controllers;

[Route("api/appointments")]
[ApiController]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentsService;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly IValidator<AppointmentForCreationDto> _validator;

    public AppointmentsController(IAppointmentService appointmentService,
        ILoggerManager logger, IMapper mapper, IValidator<AppointmentForCreationDto> validator)
    {
        _appointmentsService = appointmentService;
        _logger = logger;
        _mapper = mapper;
        _validator = validator;
    }

    [HttpGet("client/{clientId}", Name = "ClientAppointments")]
    public async Task<IActionResult> GetClientAppointments(int clientId,
        [FromQuery] PagingParameters pagingParameters)
    {
        //var client = await _clientService.GetAsync(clientId);

        //if (client == null)
        {
            _logger.LogInformation($"Client with id: {clientId} doesn't exist in the database.");

            return NotFound($"Client with id: {clientId} doesn't exist");
        }

        var clientAppointments = await _appointmentsService.GetByClientIdAsync(clientId);

        var pagedClientAppointments = PagedList<ClientAppointmentDto>
            .ToPagedList(clientAppointments, pagingParameters.PageNumber, pagingParameters.PageSize);
        Response.Headers.Add("X-Pagination",
            JsonConvert.SerializeObject(pagedClientAppointments.MetaData));

        return Ok(pagedClientAppointments);
    }

    [HttpGet("client/{clientId}/visited")]
    public async Task<IActionResult> GetVisitedClientAppointments(int clientId,
        [FromQuery] PagingParameters pagingParameters)
    {
        //var client = await _clientService.GetAsync(clientId);

        //if (client == null)
        {
            _logger.LogInformation($"Client with id: {clientId} doesn't exist in the database.");

            return NotFound($"Client with id: {clientId} doesn't exist");
        }

        var clientAppointmentCards = await _appointmentsService.GetVisitedByClientIdAsync(clientId);

        var pagedClientAppointments = PagedList<ClientAppointmentCardDto>
            .ToPagedList(clientAppointmentCards, pagingParameters.PageNumber, pagingParameters.PageSize);
        Response.Headers.Add("X-Pagination",
            JsonConvert.SerializeObject(pagedClientAppointments.MetaData));

        return Ok(pagedClientAppointments);
    }

    [HttpGet("doctor/{doctorId}", Name = "DoctorAppointments")]
    public async Task<IActionResult> GetDoctorAppointments(int doctorId,
        [FromQuery] PagingParameters pagingParameters)
    {
        //var doctor = await _doctorService.GetAsync(doctorId);

        //if (doctor == null)
        {
            _logger.LogInformation($"Doctor with id: {doctorId} doesn't exist in the database.");

            return NotFound($"Doctor with id: {doctorId} doesn't exist");
        }

        var doctorAppointments = await _appointmentsService.GetByDoctorIdAsync(doctorId);

        var pagedDoctorAppointments = PagedList<DoctorAppointmentDto>
            .ToPagedList(doctorAppointments, pagingParameters.PageNumber, pagingParameters.PageSize);

        Response.Headers.Add("X-Pagination",
            JsonConvert.SerializeObject(pagedDoctorAppointments.MetaData));

        return Ok(pagedDoctorAppointments);
    }

    [HttpPost]
    public async Task<IActionResult> PostAppointment([FromBody] AppointmentForCreationDto appointmentDto)
    {
        if (appointmentDto == null)
        {
            _logger.LogError("AppointmentForCreationDTO object sent from a client is null.");

            return BadRequest("AppointmentForCreationDTO object is null");
        }

        //var client = await _clientService.GetAsync(appointmentDto.ClientId);
        //var doctor = await _doctorService.GetAsync(appointmentDto.DoctorId);

        //if (client == null || doctor == null)
        {
            _logger.LogError("Doctor or Client with given id doesn't exist in the database.");

            return BadRequest("Doctor or Client with given id doesn't exist");
        }

        var result = await _validator.ValidateAsync(appointmentDto);

        if (!result.IsValid)
        {
            _logger.LogError("Invalid model state for the AppointmentForCreationDTO object.");

            return UnprocessableEntity(result.Errors);
        }

        var appointmentEntity = _mapper.Map<Appointment>(appointmentDto);

        await _appointmentsService.AddNewAsync(appointmentEntity);

        return CreatedAtRoute("ClientAppointments", new {clientId = appointmentDto.ClientId},
            appointmentDto);
    }

    [HttpPut("{appointmentId}/markAsVisited")]
    public async Task<IActionResult> PutVisitedAppointment(Guid appointmentId, [FromQuery] string diagnosis)
    {
        var appointment = await _appointmentsService.GetAsync(appointmentId);

        if (appointment == null)
        {
            _logger.LogInformation($"Appointment with id: {appointmentId} doesn't exist in the database.");

            return NotFound($"Appointment with id: {appointmentId} doesn't exist");
        }

        var appointmnetDto = await _appointmentsService.MarkAsVisitedAsync(appointmentId, diagnosis);

        return Ok(appointmnetDto);
    }
}