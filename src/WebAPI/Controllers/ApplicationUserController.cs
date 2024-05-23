using System.Collections.Generic;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.DTOs.ApplicationUserDTOs;
using HospitalRegistrationSystem.Application.Interfaces;
using HospitalRegistrationSystem.Application.Interfaces.Services;
using HospitalRegistrationSystem.Application.Utility;
using HospitalRegistrationSystem.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalRegistrationSystem.WebAPI.Controllers;

/// <summary>
///     Controller for managing application users.
/// </summary>
[Authorize]
[ApiController]
[Route("api/users")]
public class ApplicationUserController : ControllerBase
{
    private readonly ILoggerManager _logger;
    private readonly IApplicationUserService _userService;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ApplicationUserController"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="userService">The user service.</param>
    public ApplicationUserController(ILoggerManager logger, IApplicationUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    /// <summary>
    ///     Gets the application user with the specified ID.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>The application user.</returns>
    [HttpGet("{userId:int}")]
    public async Task<ActionResult<ApplicationUserDto>> Get(int userId)
    {
        var result = await _userService.GetAsync(userId);

        if (!result.IsSuccess)
        {
            _logger.LogInformation(result.Error.Description);

            return NotFound(result.Error.Message);
        }

        var userDto = result.Value;

        return Ok(userDto);
    }

    /// <summary>
    ///     Gets all application users with the specified role.
    /// </summary>
    /// <param name="paging">The paging parameters.</param>
    /// <param name="role">The role.</param>
    /// <param name="searchQuery">The search query.(optional)</param>
    /// <returns>The list of application users.</returns>
    [HttpGet("role/{role}")]
    public async Task<ActionResult<PagedList<ApplicationUserDto>>> GetAllByRole([FromQuery] PagingParameters paging, string role, string? searchQuery = null)
    {
        var result = await _userService.GetAllAsync(paging, searchQuery, role);

        var userDtos = result.Value;

        return Ok(userDtos);
    }

    /// <summary>
    ///     Gets all application users with the specified role for a specific hospital.
    /// </summary>
    /// <param name="paging">The paging parameters.</param>
    /// <param name="hospitalId">The hospital ID.</param>
    /// <param name="role">The role.</param>
    /// <param name="searchQuery">The search query.(optional)</param>
    /// <returns>The list of application users.</returns>
    [HttpGet("hospital/{hospitalId:int}/role/{role}")]
    public async Task<ActionResult<List<ApplicationUserDto>>> GetAllByHospitalAndRole([FromQuery] PagingParameters paging, int hospitalId, string role, string? searchQuery = null)
    {
        var result = await _userService.GetAllAsync(paging, searchQuery, role, hospitalId);

        if (!result.IsSuccess)
        {
            _logger.LogInformation(result.Error.Description);

            return BadRequest(result.Error.Message);
        }

        var userDtos = result.Value;

        return Ok(userDtos);
    }

    /// <summary>
    ///     Updates the specified application user.
    /// </summary>
    /// <param name="applicationUserDto">The application user DTO.</param>
    /// <returns>The result of the update operation.</returns>
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ApplicationUserDto applicationUserDto)
    {
        var result = await _userService.UpdateAsync(applicationUserDto);

        if (!result.IsSuccess)
        {
            _logger.LogInformation(result.Error.Description);

            return BadRequest(result.Error.Message);
        }

        return Ok();
    }

    /// <summary>
    ///     Assigns an employee to a role for a specific hospital.
    /// </summary>
    /// <param name="userId">The ID of the user to assign.</param>
    /// <param name="role">The role to assign.</param>
    /// <param name="hospitalId">The ID of the hospital.</param>
    /// <param name="specialty">The specialty of the employee (optional).</param>
    /// <returns>The result of the assignment operation.</returns>
    [Authorize(Roles = $"{RoleConstants.MasterSupervisor},{RoleConstants.Supervisor}")]
    [HttpPost("{userId:int}/role/{role}")]
    public async Task<IActionResult> AssignEmployee(int userId, string role, int hospitalId, string? specialty)
    {
        var result = await _userService.AssignEmployeeAsync(userId, role, hospitalId, specialty);

        if (!result.IsSuccess)
        {
            _logger.LogInformation(result.Error.Description);

            return BadRequest(result.Error.Message);
        }

        return Ok();
    }
}
