using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.DTOs.ApplicationUserDTOs;
using HospitalRegistrationSystem.Application.Interfaces;
using HospitalRegistrationSystem.Application.Interfaces.Services;
using HospitalRegistrationSystem.Application.Utility;
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
    /// <param name="role">The role.</param>
    /// <param name="paging">The paging parameters.</param>
    /// <returns>The list of application users.</returns>
    [HttpPost("role/{role}")]
    public async Task<ActionResult<PagedList<ApplicationUserDto>>> GetAllByRole(string role, [FromBody] PagingParameters paging)
    {
        var result = await _userService.GetAllByRoleAsync(paging, role);

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
    ///     Assigns a user to the specified role.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="role">The role.</param>
    /// <returns>The result of the assignment operation.</returns>
    [HttpPost("{userId:int}/role/{role}")]
    public async Task<IActionResult> AssignUserToRole(int userId, string role)
    {
        var result = await _userService.AssignUserToRoleAsync(userId, role);

        if (!result.IsSuccess)
        {
            _logger.LogInformation(result.Error.Description);

            return BadRequest(result.Error.Message);
        }

        return Ok();
    }
}
