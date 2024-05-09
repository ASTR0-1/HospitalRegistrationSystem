using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.DTOs.HospitalDTOs;
using HospitalRegistrationSystem.Application.Interfaces;
using HospitalRegistrationSystem.Application.Interfaces.Services;
using HospitalRegistrationSystem.Application.Utility;
using HospitalRegistrationSystem.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalRegistrationSystem.WebAPI.Controllers;

/// <summary>
///     Controller for managing hospitals.
/// </summary>
[ApiController]
[Route("api/hospitals")]
public class HospitalController : ControllerBase
{
    private readonly ILoggerManager _logger;
    private readonly IHospitalService _hospitalService;

    /// <summary>
    ///     Initializes a new instance of the <see cref="HospitalController" /> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="hospitalService">The hospital service.</param>
    public HospitalController(ILoggerManager logger, IHospitalService hospitalService)
    {
        _logger = logger;
        _hospitalService = hospitalService;
    }

    /// <summary>
    ///     Gets the hospital with the specified ID.
    /// </summary>
    /// <param name="hospitalId">The hospital ID.</param>
    /// <returns>The hospital.</returns>
    [HttpGet("{hospitalId:int}")]
    public async Task<ActionResult<HospitalDto>> Get(int hospitalId)
    {
        var result = await _hospitalService.GetHospitalAsync(hospitalId);

        if (!result.IsSuccess)
        {
            _logger.LogInformation(result.Error.Description);

            return NotFound(result.Error.Message);
        }

        var hospitalDto = result.Value;

        return Ok(hospitalDto);
    }

    /// <summary>
    ///     Gets all hospitals.
    /// </summary>
    /// <param name="paging">The paging parameters.</param>
    /// <param name="searchQuery">The search query.</param>
    /// <returns>The list of hospitals.</returns>
    [HttpGet]
    public async Task<ActionResult<PagedList<HospitalDto>>> GetAll([FromQuery] PagingParameters paging, [FromQuery] string? searchQuery)
    {
        var result = await _hospitalService.GetHospitalsAsync(paging, searchQuery);

        if (!result.IsSuccess)
        {
            _logger.LogInformation(result.Error.Description);

            return NotFound(result.Error.Message);
        }

        var hospitalDtos = result.Value;

        return Ok(hospitalDtos);
    }

    /// <summary>
    ///     Adds a new hospital.
    /// </summary>
    /// <param name="hospitalCreationDto">The hospital creation DTO.</param>
    /// <returns>The result of the add operation.</returns>
    [Authorize(Roles = RoleConstants.MasterSupervisor)]
    [HttpPost]
    public async Task<IActionResult> AddNew([FromBody] HospitalForCreationDto hospitalCreationDto)
    {
        var result = await _hospitalService.CreateHospitalAsync(hospitalCreationDto);

        if (!result.IsSuccess)
        {
            _logger.LogInformation(result.Error.Description);

            return BadRequest(result.Error.Message);
        }

        return Ok();
    }

    /// <summary>
    ///     Deletes the hospital with the specified ID.
    /// </summary>
    /// <param name="hospitalId">The hospital ID.</param>
    /// <returns>The result of the delete operation.</returns>
    [Authorize(Roles = RoleConstants.MasterSupervisor)]
    [HttpDelete("{hospitalId:int}")]
    public async Task<IActionResult> Delete(int hospitalId)
    {
        var result = await _hospitalService.DeleteHospitalAsync(hospitalId);

        if (!result.IsSuccess)
        {
            _logger.LogInformation(result.Error.Description);

            return BadRequest(result.Error.Message);
        }

        return Ok();
    }
}
