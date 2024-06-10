using System.Threading.Tasks;
using AutoMapper;
using HospitalRegistrationSystem.Application.DTOs.LocationDTOs;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Application.Interfaces.Services;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Entities;
using HospitalRegistrationSystem.Domain.Errors;
using HospitalRegistrationSystem.Domain.Shared.ResultPattern;

namespace HospitalRegistrationSystem.Application.Services;

/// <summary>
///     Service class for managing cities.
/// </summary>
public class CityService : ICityService
{
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _repository;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CityService" /> class.
    /// </summary>
    /// <param name="mapper">The mapper.</param>
    /// <param name="repository">The repository.</param>
    public CityService(IMapper mapper, IRepositoryManager repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    /// <inheritdoc />
    public async Task<Result<PagedList<CityDto>>> GetAllAsync(PagingParameters paging)
    {
        var cities = await _repository.City.GetCitiesAsync(paging);
        var cityDtos = _mapper.Map<PagedList<CityDto>>(cities);

        return Result<PagedList<CityDto>>.Success(cityDtos);
    }

    /// <inheritdoc />
    public async Task<Result<CityDto>> GetAsync(int cityId)
    {
        var city = await _repository.City.GetCityAsync(cityId);
        if (city is null)
            return Result<CityDto>.Failure(CityError.CityIdNotFound(cityId));

        var cityDto = _mapper.Map<CityDto>(city);

        return Result<CityDto>.Success(cityDto);
    }

    /// <inheritdoc />
    public async Task<Result> AddNewAsync(CityDto cityCreationDto)
    {
        var city = _mapper.Map<City>(cityCreationDto);

        _repository.City.CreateCity(city);
        await _repository.SaveAsync();

        return Result.Success();
    }

    /// <inheritdoc />
    public async Task<Result> DeleteAsync(int cityId)
    {
        var existingCity = await _repository.City.GetCityAsync(cityId);
        if (existingCity is null)
            return Result.Failure(CityError.CityIdNotFound(cityId));

        _repository.City.DeleteCity(existingCity);
        await _repository.SaveAsync();

        return Result.Success();
    }
}