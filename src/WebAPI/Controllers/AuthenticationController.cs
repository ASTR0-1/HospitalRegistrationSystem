using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using HospitalRegistrationSystem.Application.DTOs.AuthenticationDTOs;
using HospitalRegistrationSystem.Application.DTOs.TokenDTOs;
using HospitalRegistrationSystem.Application.Interfaces;
using HospitalRegistrationSystem.Application.Interfaces.Identity;
using HospitalRegistrationSystem.Domain.Constants;
using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace HospitalRegistrationSystem.WebAPI.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/authentication")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationManager _authenticationManager;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly IValidator<UserForAuthenticationDto> _userForAuthenticationDtoValidator;

    private readonly IValidator<UserForRegistrationDto> _userForRegistrationDtoValidator;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthenticationController(IAuthenticationManager authenticationManager,
        UserManager<ApplicationUser> userManager,
        ILoggerManager logger, IMapper mapper,
        IValidator<UserForRegistrationDto> userForRegistrationDtoValidator,
        IValidator<UserForAuthenticationDto> userForAuthenticationDtoValidator)
    {
        _authenticationManager = authenticationManager;
        _userManager = userManager;

        _logger = logger;
        _mapper = mapper;

        _userForRegistrationDtoValidator = userForRegistrationDtoValidator;
        _userForAuthenticationDtoValidator = userForAuthenticationDtoValidator;
    }

    /// <summary>
    ///     Registers client by provided information and credentials.
    /// </summary>
    /// <param name="userForRegistrationDto">Data transfer object to create the user.</param>
    /// <returns>Model with UserId and token.</returns>
    /// <remarks>HTTP POST: api/authentication/register</remarks>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthenticationResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(UnprocessableEntityObjectResult), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Register(UserForRegistrationDto userForRegistrationDto)
    {
        var validationResult = await _userForRegistrationDtoValidator.ValidateAsync(userForRegistrationDto);

        if (!validationResult.IsValid)
        {
            _logger.LogInformation($"Registration failed with un processable entity." +
                                   $"\nErrors: {JsonConvert.SerializeObject(validationResult.Errors)}");

            var validationErrors = validationResult.Errors.Select(error => new
            {
                error.PropertyName,
                error.ErrorMessage
            });

            return UnprocessableEntity(validationErrors);
        }

        // Validation passed, proceed with registration logic
        var user = _mapper.Map<ApplicationUser>(userForRegistrationDto);
        user.UserName = user.PhoneNumber;

        var result = await _userManager.CreateAsync(user, userForRegistrationDto.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.TryAddModelError(error.Code, error.Description);

            _logger.LogInformation($"User registration failed for user with phone number: '{user.PhoneNumber}'. " +
                                   $"\nModelState: {JsonConvert.SerializeObject(ModelState)}");

            return BadRequest(ModelState);
        }

        await _userManager.AddToRoleAsync(user, RoleConstants.Client);
        var token = await _authenticationManager.CreateTokenAsync(user, true);

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
        var validationResult = await _userForAuthenticationDtoValidator.ValidateAsync(userForAuthenticationDto);

        if (!validationResult.IsValid)
        {
            _logger.LogInformation($"Authentication failed with un processable entity." +
                                   $"\nErrors: {JsonConvert.SerializeObject(validationResult.Errors)}");

            var validationErrors = validationResult.Errors.Select(error => new
            {
                error.PropertyName,
                error.ErrorMessage
            });

            return UnprocessableEntity(validationErrors);
        }

        if (!await _authenticationManager.ValidateUserAsync(userForAuthenticationDto))
        {
            _logger.LogInformation(
                $"Incorrect authenticate try of user with phone number: '{userForAuthenticationDto.Password}'.");

            return Unauthorized();
        }

        var user = await _userManager.Users
            .FirstAsync(u => u.PhoneNumber == userForAuthenticationDto.PhoneNumber);
        var token = await _authenticationManager.CreateTokenAsync(populateExpiration: true);

        return Ok(new AuthenticationResultDto
        {
            UserId = user.Id,
            Token = token
        });
    }

    /// <summary>
    ///     Refreshes the authentication token.
    /// </summary>
    /// <param name="tokenDto">The token DTO.</param>
    /// <returns>The refreshed token.</returns>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromBody] TokenDto tokenDto)
    {
        try
        {
            var token = await _authenticationManager.RefreshTokenAsync(tokenDto);

            return Ok(token);
        }
        catch (SecurityTokenException ex)
        {
            _logger.LogError($"Error occurred while refreshing token: {ex.Message}");

            return Unauthorized();
        }
    }
}