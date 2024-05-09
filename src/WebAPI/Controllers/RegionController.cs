using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.DTOs.LocationDTOs;
using HospitalRegistrationSystem.Application.Interfaces;
using HospitalRegistrationSystem.Application.Interfaces.Services;
using HospitalRegistrationSystem.Application.Utility;
using HospitalRegistrationSystem.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalRegistrationSystem.WebAPI.Controllers;

/// <summary>
///     Controller for managing regions.
/// </summary>
[Authorize(Roles = RoleConstants.MasterSupervisor)]
[ApiController]
[Route("api/regions")]
public class RegionController : ControllerBase
{
    private readonly ILoggerManager _logger;
    private readonly IRegionService _regionService;

    /// <summary>
    ///     Initializes a new instance of the <see cref="RegionController"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="regionService">The region service.</param>
    public RegionController(ILoggerManager logger, IRegionService regionService)
    {
        _logger = logger;
        _regionService = regionService;
    }

    /// <summary>
    ///     Gets the region with the specified ID.
    /// </summary>
    /// <param name="regionId">The region ID.</param>
    /// <returns>The region.</returns>
    [HttpGet("{regionId:int}")]
    public async Task<ActionResult<RegionDto>> Get(int regionId)
    {
        var result = await _regionService.GetAsync(regionId);

        if (!result.IsSuccess)
        {
            _logger.LogInformation(result.Error.Description);

            return NotFound(result.Error.Message);
        }

        var regionDto = result.Value;

        return Ok(regionDto);
    }

    /// <summary>
    ///     Gets all regions.
    /// </summary>
    /// <param name="paging">The paging parameters.</param>
    /// <returns>The list of regions.</returns>
    [HttpGet]
    public async Task<ActionResult<PagedList<RegionDto>>> GetAll([FromQuery] PagingParameters paging)
    {
        var result = await _regionService.GetAllAsync(paging);

        if (!result.IsSuccess)
        {
            _logger.LogInformation(result.Error.Description);

            return NotFound(result.Error.Message);
        }

        var regionDtos = result.Value;

        return Ok(regionDtos);
    }

    /// <summary>
    ///     Adds a new region.
    /// </summary>
    /// <param name="regionCreationDto">The region creation DTO.</param>
    /// <returns>The result of the add operation.</returns>
    [HttpPost]
    public async Task<IActionResult> AddNew([FromBody] RegionDto regionCreationDto)
    {
        var result = await _regionService.AddNewAsync(regionCreationDto);

        if (!result.IsSuccess)
        {
            _logger.LogInformation(result.Error.Description);

            return BadRequest(result.Error.Message);
        }

        return Ok();
    }

    /// <summary>
    ///     Deletes the region with the specified ID.
    /// </summary>
    /// <param name="regionId">The region ID.</param>
    /// <returns>The result of the delete operation.</returns>
    [HttpDelete("{regionId:int}")]
    public async Task<IActionResult> Delete(int regionId)
    {
        var result = await _regionService.DeleteAsync(regionId);

        if (!result.IsSuccess)
        {
            _logger.LogInformation(result.Error.Description);

            return BadRequest(result.Error.Message);
        }

        return Ok();
    }
}
