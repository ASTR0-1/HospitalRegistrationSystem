using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.DTOs.LocationDTOs;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Shared.ResultPattern;

namespace HospitalRegistrationSystem.Application.Interfaces.Services;

/// <summary>
///     Represents a service for managing cities.
/// </summary>
public interface ICityService
{
    /// <summary>
    ///     Retrieves all cities asynchronously with paging.
    /// </summary>
    /// <param name="paging">The paging parameters.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of cities.</returns>
    Task<Result<PagedList<CityDto>>> GetAllAsync(PagingParameters paging);

    /// <summary>
    ///     Retrieves a city by its ID asynchronously.
    /// </summary>
    /// <param name="cityId">The ID of the city.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the city.</returns>
    Task<Result<CityDto>> GetAsync(int cityId);

    /// <summary>
    ///     Adds a new city asynchronously.
    /// </summary>
    /// <param name="cityCreationDto">The city data to be added.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<Result> AddNewAsync(CityDto cityCreationDto);

    /// <summary>
    ///     Deletes a city by its ID asynchronously.
    /// </summary>
    /// <param name="cityId">The ID of the city to be deleted.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<Result> DeleteAsync(int cityId);
}