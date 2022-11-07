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

[Route("api/doctors")]
[ApiController]
public class DoctorsController : ControllerBase
{
    private readonly IDoctorService _doctorService;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly IValidator<DoctorForCreationDTO> _validator;

    public DoctorsController(IDoctorService doctorService, ILoggerManager logger,
            IMapper mapper, IValidator<DoctorForCreationDTO> validator)
    {
        _doctorService = doctorService;
        _logger = logger;
        _mapper = mapper;
        _validator = validator;
    }

    [HttpGet(Name = "Doctors")]
    public async Task<IActionResult> GetDoctors([FromQuery] string searchString)
    {
        if (string.IsNullOrEmpty(searchString))
        {
            IEnumerable<DoctorCardDTO> allDoctorsDtos = await _doctorService.GetAllAsync();

            return Ok(allDoctorsDtos);
        }

        IEnumerable<DoctorCardDTO> doctorsDtos = await _doctorService.FindAsync(searchString);

        if (!doctorsDtos.Any())
        {
            _logger.LogInformation($"Doctors with given searchString: '{searchString}' doesn't exist in the database.");

            return NotFound();
        }

        return Ok(doctorsDtos);
    }

    [HttpPost]
    public async Task<IActionResult> PostDoctor([FromBody] DoctorForCreationDTO doctorDto)
    {
        if (doctorDto == null)
        {
            _logger.LogError("DoctorCardDTO object sent from a client is null");

            return BadRequest("DoctorCardDTO object is null");
        }

        ValidationResult result = await _validator.ValidateAsync(doctorDto);

        if (!result.IsValid)
        {
            _logger.LogError("Invalid model state for the DoctorCardDTO object");

            return UnprocessableEntity(result.Errors);
        }

        var doctorEntity = _mapper.Map<Doctor>(doctorDto);

        await _doctorService.AddNewAsync(doctorEntity);

        return CreatedAtRoute("Doctors", new { searchString = doctorDto.FirstName },
            doctorDto);
    }
}
