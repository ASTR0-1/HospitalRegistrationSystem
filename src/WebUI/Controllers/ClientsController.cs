using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using HospitalRegistrationSystem.Application.Interfaces;
using HospitalRegistrationSystem.Application.Interfaces.DTOs;
using HospitalRegistrationSystem.Application.Interfaces.Services;
using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HospitalRegistrationSystem.WebUI.Controllers;

[Route("api/clients")]
[ApiController]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly IValidator<ClientForCreationDTO> _validator;

    public ClientsController(IClientService clientService, ILoggerManager logger,
        IMapper mapper, IValidator<ClientForCreationDTO> validator)
    {
        _clientService = clientService;
        _logger = logger;
        _mapper = mapper;
        _validator = validator;
    }

    [HttpGet(Name = "Clients")]
    public async Task<IActionResult> GetClients([FromQuery] string searchString)
    {
        if (string.IsNullOrEmpty(searchString))
        {
            IEnumerable<ClientCardDTO> allClientsDtos = await _clientService.GetAllAsync();

            return Ok(allClientsDtos);
        }

        IEnumerable<ClientCardDTO> clientsDtos = await _clientService.FindAsync(searchString);

        if (!clientsDtos.Any())
        {
            _logger.LogInformation($"Clients with given searchString: '{searchString}' doesn't exist in the database.");

            return NotFound();
        }

        return Ok(clientsDtos);
    }

    [HttpPost]
    public async Task<IActionResult> PostClient([FromBody] ClientForCreationDTO clientDto)
    {
        if (clientDto == null)
        {
            _logger.LogError("ClientCardDTO object sent from a client is null");

            return BadRequest("ClientCardDTO object is null");
        }

        ValidationResult result = await _validator.ValidateAsync(clientDto);

        if (!result.IsValid)
        {
            _logger.LogError("Invalid model state for the ClientCardDTO object");

            return UnprocessableEntity(result.Errors);
        }

        var clientEntity = _mapper.Map<Client>(clientDto);

        await _clientService.AddNewAsync(clientEntity);

        return CreatedAtRoute("Clients", new { searchString = clientDto.FirstName },
            clientDto);
    }
}
