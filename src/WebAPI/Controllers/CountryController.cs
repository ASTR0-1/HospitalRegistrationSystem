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
///     Controller for managing countries.
/// </summary>
[Authorize(Roles = RoleConstants.MasterSupervisor)]
[ApiController]
[Route("api/countries")]
public class CountryController : ControllerBase
{
    private readonly ILoggerManager _logger;
    private readonly ICountryService _countryService;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CountryController" /> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="countryService">The country service.</param>
    public CountryController(ILoggerManager logger, ICountryService countryService)
    {
        _logger = logger;
        _countryService = countryService;
    }

    /// <summary>
    ///     Gets the country with the specified ID.
    /// </summary>
    /// <param name="countryId">The country ID.</param>
    /// <returns>The country.</returns>
    [HttpGet("{countryId:int}")]
    public async Task<ActionResult<CountryDto>> Get(int countryId)
    {
        var result = await _countryService.GetAsync(countryId);

        if (!result.IsSuccess)
        {
            _logger.LogInformation(result.Error.Description);

            return NotFound(result.Error.Message);
        }

        var countryDto = result.Value;

        return Ok(countryDto);
    }

    /// <summary>
    ///     Gets all countries.
    /// </summary>
    /// <param name="paging">The paging parameters.</param>
    /// <returns>The list of countries.</returns>
    [HttpGet]
    public async Task<ActionResult<PagedList<CountryDto>>> GetAll([FromQuery] PagingParameters paging)
    {
        var result = await _countryService.GetAllAsync(paging);

        if (!result.IsSuccess)
        {
            _logger.LogInformation(result.Error.Description);

            return NotFound(result.Error.Message);
        }

        var countryDtos = result.Value;

        return Ok(countryDtos);
    }

    /// <summary>
    ///     Adds a new country.
    /// </summary>
    /// <param name="countryCreationDto">The country creation DTO.</param>
    /// <returns>The result of the add operation.</returns>
    [HttpPost]
    public async Task<IActionResult> AddNew([FromBody] CountryDto countryCreationDto)
    {
        var result = await _countryService.AddNewAsync(countryCreationDto);

        if (!result.IsSuccess)
        {
            _logger.LogInformation(result.Error.Description);

            return BadRequest(result.Error.Message);
        }

        return Ok();
    }

    /// <summary>
    ///     Deletes the country with the specified ID.
    /// </summary>
    /// <param name="countryId">The country ID.</param>
    /// <returns>The result of the delete operation.</returns>
    [HttpDelete("{countryId:int}")]
    public async Task<IActionResult> Delete(int countryId)
    {
        var result = await _countryService.DeleteAsync(countryId);

        if (!result.IsSuccess)
        {
            _logger.LogInformation(result.Error.Description);

            return BadRequest(result.Error.Message);
        }

        return Ok();
    }
}
