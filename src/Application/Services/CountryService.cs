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
///     Represents a service for managing countries.
/// </summary>
public class CountryService : ICountryService
{
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _repository;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CountryService"/> class.
    /// </summary>
    /// <param name="mapper">The mapper.</param>
    /// <param name="repository">The repository.</param>
    public CountryService(IMapper mapper, IRepositoryManager repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    /// <inheritdoc />
    public async Task<Result<PagedList<CountryDto>>> GetAllAsync(PagingParameters paging)
    {
        var countries = await _repository.Country.GetCountriesAsync(paging);
        var countryDtos = _mapper.Map<PagedList<CountryDto>>(countries);

        return Result<PagedList<CountryDto>>.Success(countryDtos);
    }

    /// <inheritdoc />
    public async Task<Result<CountryDto>> GetAsync(int countryId)
    {
        var country = await _repository.Country.GetCountryAsync(countryId);
        if (country is null)
            return Result<CountryDto>.Failure(CountryError.CountryIdNotFound(countryId));

        var countryDto = _mapper.Map<CountryDto>(country);

        return Result<CountryDto>.Success(countryDto);
    }

    /// <inheritdoc />
    public async Task<Result> AddNewAsync(CountryDto countryCreationDto)
    {
        var country = _mapper.Map<Country>(countryCreationDto);

        _repository.Country.CreateCountry(country);
        await _repository.SaveAsync();

        return Result.Success();
    }

    /// <inheritdoc />
    public async Task<Result> DeleteAsync(int countryId)
    {
        var existingCountry = await _repository.Country.GetCountryAsync(countryId);
        if (existingCountry is null)
            return Result.Failure(CountryError.CountryIdNotFound(countryId));

        _repository.Country.DeleteCountry(existingCountry);
        await _repository.SaveAsync();

        return Result.Success();
    }
}
