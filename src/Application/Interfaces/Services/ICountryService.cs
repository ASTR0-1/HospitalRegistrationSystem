using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.DTOs.LocationDTOs;
using HospitalRegistrationSystem.Application.Utility;
using HospitalRegistrationSystem.Domain.Shared;

namespace HospitalRegistrationSystem.Application.Interfaces.Services;

/// <summary>
///     Represents a service for managing countries.
/// </summary>
public interface ICountryService
{
    /// <summary>
    ///     Retrieves all countries asynchronously with paging.
    /// </summary>
    /// <param name="paging">The paging parameters.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of countries.</returns>
    Task<Result<PagedList<CountryDto>>> GetAllAsync(PagingParameters paging);

    /// <summary>
    ///     Retrieves a country by its ID asynchronously.
    /// </summary>
    /// <param name="countryId">The ID of the country.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the country.</returns>
    Task<Result<CountryDto>> GetAsync(int countryId);

    /// <summary>
    ///     Adds a new country asynchronously.
    /// </summary>
    /// <param name="countryCreationDto">The country data to be added.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<Result> AddNewAsync(CountryDto countryCreationDto);

    /// <summary>
    ///     Deletes a country by its ID asynchronously.
    /// </summary>
    /// <param name="countryId">The ID of the country to be deleted.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<Result> DeleteAsync(int countryId);
}
