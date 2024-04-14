using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using HospitalRegistrationSystem.Application.DTOs.AuthenticationDTOs;
using HospitalRegistrationSystem.Application.Interfaces;
using HospitalRegistrationSystem.Domain.Constants;
using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace HospitalRegistrationSystem.WebAPI.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/authentication")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationManager _authenticationManager;
    private readonly UserManager<ApplicationUser> _userManager;
    
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public AuthenticationController(IAuthenticationManager authenticationManager, UserManager<ApplicationUser> userManager,
        ILoggerManager logger, IMapper mapper)
    {
        _authenticationManager = authenticationManager;
        _userManager = userManager;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    ///     Registers client by provided information and credentials.
    /// </summary>
    /// <param name="clientForRegistrationDto">Data transfer object to create the user.</param>
    /// <returns>Model with UserId and token.</returns>
    /// <remarks>HTTP POST: api/authentication/register</remarks>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthenticationResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterClient([FromBody] ClientForRegistrationDto clientForRegistrationDto)
    {
        var user = _mapper.Map<ApplicationUser>(clientForRegistrationDto);
        user.UserName = user.PhoneNumber;

        var result = await _userManager.CreateAsync(user, clientForRegistrationDto.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.TryAddModelError(error.Code, error.Description);

            _logger.LogError($"Client registration failed for client with phone number: '{user.PhoneNumber}'. " +
                             $"\nModelState: {JsonConvert.SerializeObject(ModelState)}");

            return BadRequest(ModelState);
        }

        await _userManager.AddToRoleAsync(user, RoleConstants.Client);
        var token = await _authenticationManager.CreateTokenAsync(user);

        return Ok(new AuthenticationResultDto
        {
            UserId = user.Id,
            Token = token
        });
    }

    /// <summary>
    ///     Sign in user by provided credentials.
    /// </summary>
    /// <param name="userForAuthenticationDto">Data transfer object for sign in.</param>
    /// <returns>Ok result.</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthenticationResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto userForAuthenticationDto)
    {
        if (!await _authenticationManager.ValidateUserAsync(userForAuthenticationDto))
        {
            _logger.LogInformation($"Incorrect authenticate try of user with phone number: '{userForAuthenticationDto.Password}'.");

            return Unauthorized();
        }

        var user = await _userManager.Users
            .FirstAsync(u => u.PhoneNumber == userForAuthenticationDto.PhoneNumber);
        var token = await _authenticationManager.CreateTokenAsync();

        return Ok(new AuthenticationResultDto
        {
            UserId = user.Id,
            Token = token
        });
    }
}
