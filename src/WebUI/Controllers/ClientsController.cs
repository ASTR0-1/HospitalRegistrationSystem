using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using HospitalRegistrationSystem.Application.Interfaces;
using HospitalRegistrationSystem.Application.Interfaces.DTOs;
using HospitalRegistrationSystem.Application.Interfaces.Services;
using HospitalRegistrationSystem.Application.Utility;
using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HospitalRegistrationSystem.WebAPI.Controllers;

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
    public async Task<IActionResult> GetClients([FromQuery] string SearchString,
        [FromQuery] PagingParameters pagingParameters)
    {
        if (string.IsNullOrEmpty(SearchString))
        {
            IEnumerable<ClientCardDTO> clientsDtos = await _clientService.GetAllAsync();
            var pagedClients = PagedList<ClientCardDTO>
                .ToPagedList(clientsDtos, pagingParameters.PageNumber, pagingParameters.PageSize);

            Response.Headers.Add("X-Pagination",
                JsonConvert.SerializeObject(pagedClients.MetaData));

            return Ok(pagedClients);
        }

        IEnumerable<ClientCardDTO> searchedClientsDtos = await _clientService.FindAsync(SearchString);

        if (!searchedClientsDtos.Any())
        {
            _logger.LogInformation($"Clients with given searchString: '{SearchString}' doesn't exist in the database.");

            return NotFound($"Clients with given searchString: '{SearchString}' doesn't exist");
        }

        var pagedClientsSearched = PagedList<ClientCardDTO>
            .ToPagedList(searchedClientsDtos, pagingParameters.PageNumber, pagingParameters.PageSize);
        Response.Headers.Add("X-Pagination",
            JsonConvert.SerializeObject(pagedClientsSearched.MetaData));

        return Ok(pagedClientsSearched);
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
