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
    public async Task<IActionResult> GetDoctors([FromQuery] string SearchString,
        [FromQuery] PagingParameters pagingParameters)
    {
        if (string.IsNullOrEmpty(SearchString))
        {
            IEnumerable<DoctorCardDTO> doctorsDtos = await _doctorService.GetAllAsync();
            var pagedDoctors = PagedList<DoctorCardDTO>
                .ToPagedList(doctorsDtos, pagingParameters.PageNumber, pagingParameters.PageSize);

            Response.Headers.Add("X-Pagination",
               JsonConvert.SerializeObject(pagedDoctors.MetaData));

            return Ok(pagedDoctors);
        }

        IEnumerable<DoctorCardDTO> searchedDoctorsDtos = await _doctorService.FindAsync(SearchString);

        if (!searchedDoctorsDtos.Any())
        {
            _logger.LogInformation($"Doctors with given searchString: '{SearchString}' doesn't exist in the database.");

            return NotFound($"Doctors with given searchString: '{SearchString}' doesn't exist");
        }

        var pagedDoctorsSearched = PagedList<DoctorCardDTO>
                .ToPagedList(searchedDoctorsDtos, pagingParameters.PageNumber, pagingParameters.PageSize);
        Response.Headers.Add("X-Pagination",
               JsonConvert.SerializeObject(pagedDoctorsSearched.MetaData));

        return Ok(pagedDoctorsSearched);
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
