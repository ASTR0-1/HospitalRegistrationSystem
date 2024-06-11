using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Entities;

namespace HospitalRegistrationSystem.Application.Interfaces.Data;

/// <summary>
///     Represents the interface for accessing and manipulating countries in the data layer.
/// </summary>
public interface ICountryRepository
{
    /// <summary>
    ///     Get countries asynchronously with paging.
    /// </summary>
    /// <param name="paging">The paging parameters.</param>
    /// <param name="trackChanges">Flag to indicate whether to track changes in the entities.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the paged list of countries.</returns>
    Task<PagedList<Country>> GetCountriesAsync(PagingParameters paging, bool trackChanges = false);

    /// <summary>
    ///     Get a country by ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the country.</param>
    /// <param name="trackChanges">Flag to indicate whether to track changes in the entity.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the country.</returns>
    Task<Country?> GetCountryAsync(int id, bool trackChanges = false);

    /// <summary>
    ///     Create a new country.
    /// </summary>
    /// <param name="country">The country to create.</param>
    void CreateCountry(Country country);

    /// <summary>
    ///     Delete a country.
    /// </summary>
    /// <param name="country">The country to delete.</param>
    void DeleteCountry(Country country);
}