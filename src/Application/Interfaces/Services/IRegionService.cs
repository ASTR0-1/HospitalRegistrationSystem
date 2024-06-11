using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.DTOs.LocationDTOs;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Shared.ResultPattern;

namespace HospitalRegistrationSystem.Application.Interfaces.Services;

/// <summary>
///     Represents a service for managing regions.
/// </summary>
public interface IRegionService
{
    /// <summary>
    ///     Retrieves all regions asynchronously with paging.
    /// </summary>
    /// <param name="paging">The paging parameters.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of regions.</returns>
    Task<Result<PagedList<RegionDto>>> GetAllAsync(PagingParameters paging);

    /// <summary>
    ///     Retrieves a region by its ID asynchronously.
    /// </summary>
    /// <param name="regionId">The ID of the region.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the region.</returns>
    Task<Result<RegionDto>> GetAsync(int regionId);

    /// <summary>
    ///     Adds a new region asynchronously.
    /// </summary>
    /// <param name="regionCreationDto">The region data to be added.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<Result> AddNewAsync(RegionDto regionCreationDto);

    /// <summary>
    ///     Deletes a region by its ID asynchronously.
    /// </summary>
    /// <param name="regionId">The ID of the region to be deleted.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<Result> DeleteAsync(int regionId);
}