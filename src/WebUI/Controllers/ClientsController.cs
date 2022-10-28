using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HospitalRegistrationSystem.Application.Interfaces;
using HospitalRegistrationSystem.Application.Interfaces.DTOs;
using HospitalRegistrationSystem.Application.Interfaces.Services;
using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HospitalRegistrationSystem.WebUI.Controllers
{
    [Route("api/clients")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public ClientsController(IClientService clientService, ILoggerManager logger, IMapper mapper)
        {
            _clientService = clientService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet(Name = "Clients")]
        public async Task<IActionResult> GetClients([FromQuery] string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                var clientsDtos = await _clientService.GetAllAsync();

                return Ok(clientsDtos);
            }

            IEnumerable<ClientCardDTO> clientDtos = await _clientService.FindAsync(searchString);

            if(!clientDtos.Any())
            {
                _logger.LogInformation($"Clients with given searchString: '{searchString}' doesn't exist in the database.");

                return NotFound();
            }

            return Ok(clientDtos);
        }

        [HttpPost]
        public async Task<IActionResult> PostClient([FromBody]ClientForCreationDTO clientDto)
        {
            if(clientDto == null)
            {
                _logger.LogError("ClientCardDTO object sent from a client is null");

                return BadRequest("ClientCardDTO object is null");
            }

            var clientEntity = _mapper.Map<Client>(clientDto);

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the ClientCardDTO object");
                return UnprocessableEntity(ModelState);
            }

            await _clientService.AddNewAsync(clientEntity);

            return CreatedAtRoute("Clients", new { searchString = clientDto.FirstName },
                clientDto);
        }
    }
}
