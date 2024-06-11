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
///     Represents a service for managing regions.
/// </summary>
public class RegionService : IRegionService
{
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _repository;

    /// <summary>
    ///     Initializes a new instance of the <see cref="RegionService" /> class.
    /// </summary>
    /// <param name="mapper">The mapper.</param>
    /// <param name="repository">The repository.</param>
    public RegionService(IMapper mapper, IRepositoryManager repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    /// <inheritdoc />
    public async Task<Result<PagedList<RegionDto>>> GetAllAsync(PagingParameters paging)
    {
        var regions = await _repository.Region.GetRegionsAsync(paging);
        var regionDtos = _mapper.Map<PagedList<RegionDto>>(regions);

        return Result<PagedList<RegionDto>>.Success(regionDtos);
    }

    /// <inheritdoc />
    public async Task<Result<RegionDto>> GetAsync(int regionId)
    {
        var region = await _repository.Region.GetRegionAsync(regionId);
        if (region is null)
            return Result<RegionDto>.Failure(RegionError.RegionIdNotFound(regionId));

        var regionDto = _mapper.Map<RegionDto>(region);

        return Result<RegionDto>.Success(regionDto);
    }

    /// <inheritdoc />
    public async Task<Result> AddNewAsync(RegionDto regionCreationDto)
    {
        var region = _mapper.Map<Region>(regionCreationDto);

        _repository.Region.CreateRegion(region);
        await _repository.SaveAsync();

        return Result.Success();
    }

    /// <inheritdoc />
    public async Task<Result> DeleteAsync(int regionId)
    {
        var existingRegion = await _repository.Region.GetRegionAsync(regionId);
        if (existingRegion is null)
            return Result.Failure(RegionError.RegionIdNotFound(regionId));

        _repository.Region.DeleteRegion(existingRegion);
        await _repository.SaveAsync();

        return Result.Success();
    }
}