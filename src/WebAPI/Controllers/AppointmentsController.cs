using System.Threading.Tasks;
using FluentValidation;
using HospitalRegistrationSystem.Application.DTOs.AppointmentDTOs;
using HospitalRegistrationSystem.Application.Interfaces;
using HospitalRegistrationSystem.Application.Interfaces.Services;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HospitalRegistrationSystem.WebAPI.Controllers;

/// <summary>
///     Represents a controller for managing appointments.
/// </summary>
[Route("api/appointments")]
[ApiController]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentsService;
    private readonly ILoggerManager _logger;

    private readonly IValidator<AppointmentForCreationDto> _validator;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AppointmentsController"/> class.
    /// </summary>
    /// <param name="appointmentService">The appointment service.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="validator">The validator.</param>
    public AppointmentsController(IAppointmentService appointmentService, ILoggerManager logger, IValidator<AppointmentForCreationDto> validator)
    {
        _appointmentsService = appointmentService;
        _logger = logger;
        _validator = validator;
    }

    /// <summary>
    ///     Gets the appointment with the specified ID.
    /// </summary>
    /// <param name="appointmentId">The appointment ID.</param>
    /// <returns>The appointment with the specified ID.</returns>
    [HttpGet("{appointmentId:int}")]
    public async Task<IActionResult> GetAppointment(int appointmentId)
    {
        var result = await _appointmentsService.GetAsync(appointmentId);
        if (result.IsFailure)
        {
            _logger.LogInformation(result.Error.Description);
            return NotFound(result.Error.Description);
        }

        return Ok(result.Value);
    }

    /// <summary>
    ///     Gets the incoming appointments for the specified user ID.
    /// </summary>
    /// <param name="paging">The paging parameters.</param>
    /// <param name="userId">The user ID.</param>
    /// <returns>The incoming appointments for the specified user ID.</returns>
    [HttpGet("incoming/{userId:int}")]
    public async Task<IActionResult> GetIncomingAppointmentsByUserId(PagingParameters paging, int userId)
    {
        var result = await _appointmentsService.GetIncomingByUserIdAsync(paging, userId);
        if (result.IsFailure)
        {
            _logger.LogInformation(result.Error.Description);
            return NotFound(result.Error.Description);
        }

        return Ok(result.Value);
    }

    /// <summary>
    ///     Gets all appointments for the specified user ID.
    /// </summary>
    /// <param name="paging">The paging parameters.</param>
    /// <param name="userId">The user ID.</param>
    /// <returns>All appointments for the specified user ID.</returns>
    [HttpGet("all/{userId:int}")]
    public async Task<IActionResult> GetAllAppointmentsByUserId(PagingParameters paging, int userId)
    {
        var result = await _appointmentsService.GetAllByUserIdAsync(paging, userId);
        if (result.IsFailure)
        {
            _logger.LogInformation(result.Error.Description);
            return NotFound(result.Error.Description);
        }

        return Ok(result.Value);
    }

    /// <summary>
    ///     Gets the missed appointments for the specified user ID.
    /// </summary>
    /// <param name="paging">The paging parameters.</param>
    /// <param name="userId">The user ID.</param>
    /// <returns>The missed appointments for the specified user ID.</returns>
    [HttpGet("missed/{userId:int}")]
    public async Task<IActionResult> GetMissedAppointmentsByUserId(PagingParameters paging, int userId)
    {
        var result = await _appointmentsService.GetMissedByUserIdAsync(paging, userId);
        if (result.IsFailure)
        {
            _logger.LogInformation(result.Error.Description);
            return NotFound(result.Error.Description);
        }

        return Ok(result.Value);
    }

    /// <summary>
    ///     Gets the visited appointments for the specified user ID.
    /// </summary>
    /// <param name="paging">The paging parameters.</param>
    /// <param name="userId">The user ID.</param>
    /// <returns>The visited appointments for the specified user ID.</returns>
    [HttpGet("visited/{userId}")]
    public async Task<IActionResult> GetVisitedAppointmentsByUserId(PagingParameters paging, int userId)
    {
        var result = await _appointmentsService.GetVisitedByUserIdAsync(paging, userId);
        if (result.IsFailure)
        {
            _logger.LogInformation(result.Error.Description);
            return NotFound(result.Error.Description);
        }

        return Ok(result.Value);
    }

    /// <summary>
    ///     Creates a new appointment.
    /// </summary>
    /// <param name="appointmentDto">The appointment DTO.</param>
    /// <returns>The created appointment.</returns>
    [HttpPost]
    public async Task<IActionResult> PostAppointment([FromBody] AppointmentForCreationDto appointmentDto)
    {
        var validateResult = await _validator.ValidateAsync(appointmentDto);
        if (!validateResult.IsValid)
        {
            _logger.LogInformation("Invalid model state for the AppointmentForCreationDTO object.");

            return UnprocessableEntity(validateResult.Errors);
        }

        var result = await _appointmentsService.AddNewAsync(appointmentDto);
        if (result.IsFailure)
        {
            _logger.LogInformation(result.Error.Description);

            return BadRequest(result.Error.Description);
        }

        return Created();
    }

    /// <summary>
    ///     Marks the appointment as visited.
    /// </summary>
    /// <param name="appointmentId">The appointment ID.</param>
    /// <param name="diagnosis">The diagnosis.</param>
    /// <returns>The result of marking the appointment as visited.</returns>
    [HttpPut("{appointmentId:int}/markAsVisited")]
    public async Task<IActionResult> PutVisitedAppointment(int appointmentId, [FromQuery] string diagnosis)
    {
        var result = await _appointmentsService.MarkAsVisitedAsync(appointmentId, diagnosis);
        if (result.IsFailure)
        {
            _logger.LogInformation(result.Error.Description);

            return BadRequest(result.Error.Description);
        }

        return Ok();
    }
}
