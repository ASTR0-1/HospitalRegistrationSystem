using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using HospitalRegistrationSystem.Application.DTOs.DoctorScheduleDTOs;
using HospitalRegistrationSystem.Application.Interfaces;
using HospitalRegistrationSystem.Application.Interfaces.Services;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HospitalRegistrationSystem.WebAPI.Controllers;

/// <summary>
///     Controller for managing doctor schedules.
/// </summary>
[ApiController]
[Route("api/doctorSchedules")]
public class DoctorScheduleController : ControllerBase
{
    private readonly IValidator<DoctorScheduleForManipulationDto> _doctorScheduleForCreationDtoValidator;
    private readonly IDoctorScheduleService _doctorScheduleService;
    private readonly ILoggerManager _logger;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DoctorScheduleController" /> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="doctorScheduleService">The doctor schedule service.</param>
    /// <param name="doctorScheduleForCreationDtoValidator">The doctor schedule DTO validator.</param>
    public DoctorScheduleController(ILoggerManager logger, IDoctorScheduleService doctorScheduleService,
        IValidator<DoctorScheduleForManipulationDto> doctorScheduleForCreationDtoValidator)
    {
        _logger = logger;
        _doctorScheduleService = doctorScheduleService;
        _doctorScheduleForCreationDtoValidator = doctorScheduleForCreationDtoValidator;
    }

    /// <summary>
    ///     Gets all doctor schedules.
    /// </summary>
    /// <param name="parameters">The parameters for pagination and filtering.</param>
    /// <param name="doctorId">The doctor ID.</param>
    /// <returns>The list of doctor schedules.</returns>
    [HttpGet]
    public async Task<ActionResult<PagedList<DoctorScheduleDto>>> GetAll(
        [FromQuery] DoctorScheduleParameters parameters, [FromQuery] int doctorId)
    {
        var result = await _doctorScheduleService.GetDoctorSchedulesAsync(parameters, doctorId);
        if (!result.IsSuccess)
        {
            _logger.LogInformation(result.Error.Description);

            return NotFound(result.Error.Message);
        }

        var doctorScheduleDtos = result.Value;

        return Ok(doctorScheduleDtos);
    }

    /// <summary>
    ///     Adds a new doctor schedule.
    /// </summary>
    /// <param name="doctorScheduleDto">The doctor schedule DTO.</param>
    /// <returns>The result of the operation.</returns>
    [Authorize(Roles = RoleConstants.Doctor)]
    [HttpPost]
    public async Task<IActionResult> AddNew([FromBody] DoctorScheduleForManipulationDto doctorScheduleDto)
    {
        var validationResult = await _doctorScheduleForCreationDtoValidator.ValidateAsync(doctorScheduleDto);
        if (!validationResult.IsValid)
        {
            _logger.LogInformation($"Doctor schedule creation failed with un processable entity." +
                                   $"\nErrors: {JsonConvert.SerializeObject(validationResult.Errors)}");

            var validationErrors = validationResult.Errors.Select(error => new
            {
                error.PropertyName,
                error.ErrorMessage
            });

            return UnprocessableEntity(validationErrors);
        }

        var result = await _doctorScheduleService.CreateDoctorSchedule(doctorScheduleDto);
        if (!result.IsSuccess)
        {
            _logger.LogInformation(result.Error.Description);

            return BadRequest(result.Error.Message);
        }

        return Ok();
    }

    /// <summary>
    ///     Updates a doctor schedule.
    /// </summary>
    /// <param name="doctorScheduleForManipulationDto">The doctor schedule DTO.</param>
    /// <returns>The result of the operation.</returns>
    [Authorize(Roles = RoleConstants.Doctor)]
    [HttpPut]
    public async Task<IActionResult> Update(
        [FromBody] DoctorScheduleForManipulationDto doctorScheduleForManipulationDto)
    {
        var validationResult =
            await _doctorScheduleForCreationDtoValidator.ValidateAsync(doctorScheduleForManipulationDto);
        if (!validationResult.IsValid)
        {
            _logger.LogInformation($"Doctor schedule update failed with un processable entity." +
                                   $"\nErrors: {JsonConvert.SerializeObject(validationResult.Errors)}");

            var validationErrors = validationResult.Errors.Select(error => new
            {
                error.PropertyName,
                error.ErrorMessage
            });

            return UnprocessableEntity(validationErrors);
        }

        var result = await _doctorScheduleService.UpdateDoctorSchedule(doctorScheduleForManipulationDto);
        if (!result.IsSuccess)
        {
            _logger.LogInformation(result.Error.Description);

            return BadRequest(result.Error.Message);
        }

        return NoContent();
    }

    /// <summary>
    ///     Deletes a doctor schedule.
    /// </summary>
    /// <param name="doctorScheduleId">The ID of the doctor schedule to delete.</param>
    /// <returns>The result of the operation.</returns>
    [Authorize(Roles = RoleConstants.Doctor)]
    [HttpDelete("{doctorScheduleId:int}")]
    public async Task<IActionResult> Delete(int doctorScheduleId)
    {
        var result = await _doctorScheduleService.DeleteDoctorSchedule(doctorScheduleId);
        if (!result.IsSuccess)
        {
            _logger.LogInformation(result.Error.Description);

            return BadRequest(result.Error.Message);
        }

        return Ok();
    }
}