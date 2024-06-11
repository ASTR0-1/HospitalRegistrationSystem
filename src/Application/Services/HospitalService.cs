using System.Threading.Tasks;
using AutoMapper;
using HospitalRegistrationSystem.Application.DTOs.HospitalDTOs;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Application.Interfaces.Services;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Entities;
using HospitalRegistrationSystem.Domain.Errors;
using HospitalRegistrationSystem.Domain.Shared.ResultPattern;

namespace HospitalRegistrationSystem.Application.Services;

/// <summary>
///     Represents a service for managing hospitals.
/// </summary>
public class HospitalService : IHospitalService
{
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _repository;

    /// <summary>
    ///     Initializes a new instance of the <see cref="HospitalService" /> class.
    /// </summary>
    /// <param name="repository">The repository manager.</param>
    /// <param name="mapper">The mapper.</param>
    public HospitalService(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<Result<PagedList<HospitalDto>>> GetHospitalsAsync(PagingParameters paging, string? searchQuery)
    {
        var hospitals = string.IsNullOrEmpty(searchQuery)
            ? await _repository.Hospital.GetHospitalsAsync(paging)
            : await _repository.Hospital.GetHospitalsAsync(paging, searchQuery);

        var hospitalsDto = _mapper.Map<PagedList<HospitalDto>>(hospitals);

        return Result<PagedList<HospitalDto>>.Success(hospitalsDto);
    }

    /// <inheritdoc />
    public async Task<Result<HospitalDto>> GetHospitalAsync(int hospitalId)
    {
        var hospital = await _repository.Hospital.GetHospitalAsync(hospitalId);
        if (hospital is null)
            return Result<HospitalDto>.Failure(HospitalError.HospitalIdNotFound(hospitalId));

        var hospitalDto = _mapper.Map<HospitalDto>(hospital);

        return Result<HospitalDto>.Success(hospitalDto);
    }

    /// <inheritdoc />
    public async Task<Result> CreateHospitalAsync(HospitalForCreationDto hospitalForCreationDto)
    {
        var city = await _repository.City.GetCityAsync(hospitalForCreationDto.CityId);
        if (city is null)
            return Result.Failure(CityError.CityIdNotFound(hospitalForCreationDto.CityId));

        if (hospitalForCreationDto.HospitalFeePercent <= 0)
            return Result.Failure(HospitalError.HospitalFeePercentInvalid(hospitalForCreationDto.Name,
                hospitalForCreationDto.HospitalFeePercent));

        var hospital = _mapper.Map<Hospital>(hospitalForCreationDto);

        _repository.Hospital.CreateHospital(hospital);
        await _repository.SaveAsync();

        return Result.Success();
    }

    /// <inheritdoc />
    public async Task<Result> DeleteHospitalAsync(int hospitalId)
    {
        var hospital = await _repository.Hospital.GetHospitalAsync(hospitalId);
        if (hospital is null)
            return Result.Failure(HospitalError.HospitalIdNotFound(hospitalId));

        _repository.Hospital.DeleteHospital(hospital);
        await _repository.SaveAsync();

        return Result.Success();
    }
}