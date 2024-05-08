using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.DTOs.LocationDTOs;
using HospitalRegistrationSystem.Application.Interfaces;
using HospitalRegistrationSystem.Application.Interfaces.Services;
using HospitalRegistrationSystem.Application.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalRegistrationSystem.WebAPI.Controllers;

/// <summary>
///     Controller for managing cities.
/// </summary>
[Authorize]
[ApiController]
[Route("api/cities")]
public class CityController : ControllerBase
{
    private readonly ILoggerManager _logger;
    private readonly ICityService _cityService;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CityController"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="cityService">The city service.</param>
    public CityController(ILoggerManager logger, ICityService cityService)
    {
        _logger = logger;
        _cityService = cityService;
    }

    /// <summary>
    ///     Gets the city with the specified ID.
    /// </summary>
    /// <param name="cityId">The city ID.</param>
    /// <returns>The city.</returns>
    [HttpGet("{cityId:int}")]
    public async Task<ActionResult<CityDto>> Get(int cityId)
    {
        var result = await _cityService.GetAsync(cityId);

        if (!result.IsSuccess)
        {
            _logger.LogInformation(result.Error.Description);

            return NotFound(result.Error.Message);
        }

        var cityDto = result.Value;

        return Ok(cityDto);
    }

    /// <summary>
    ///     Gets all cities.
    /// </summary>
    /// <param name="paging">The paging parameters.</param>
    /// <returns>The list of cities.</returns>
    [HttpGet]
    public async Task<ActionResult<PagedList<CityDto>>> GetAll([FromQuery] PagingParameters paging)
    {
        var result = await _cityService.GetAllAsync(paging);

        if (!result.IsSuccess)
        {
            _logger.LogInformation(result.Error.Description);

            return NotFound(result.Error.Message);
        }

        var cityDtos = result.Value;

        return Ok(cityDtos);
    }

    /// <summary>
    ///     Adds a new city.
    /// </summary>
    /// <param name="cityCreationDto">The city creation DTO.</param>
    /// <returns>The result of the add operation.</returns>
    [HttpPost]
    public async Task<IActionResult> AddNew([FromBody] CityDto cityCreationDto)
    {
        var result = await _cityService.AddNewAsync(cityCreationDto);

        if (!result.IsSuccess)
        {
            _logger.LogInformation(result.Error.Description);

            return BadRequest(result.Error.Message);
        }

        return Ok();
    }

    /// <summary>
    ///     Deletes the city with the specified ID.
    /// </summary>
    /// <param name="cityId">The city ID.</param>
    /// <returns>The result of the delete operation.</returns>
    [HttpDelete("{cityId:int}")]
    public async Task<IActionResult> Delete(int cityId)
    {
        var result = await _cityService.DeleteAsync(cityId);

        if (!result.IsSuccess)
        {
            _logger.LogInformation(result.Error.Description);

            return BadRequest(result.Error.Message);
        }

        return Ok();
    }
}
