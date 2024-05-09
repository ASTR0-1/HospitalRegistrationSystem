using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Utility;
using HospitalRegistrationSystem.Domain.Entities;

namespace HospitalRegistrationSystem.Application.Interfaces.Data;

/// <summary>
///     Represents the interface for accessing and manipulating cities in the data layer.
/// </summary>
public interface ICityRepository
{
    /// <summary>
    ///     Get cities asynchronously with paging.
    /// </summary>
    /// <param name="paging">The paging parameters.</param>
    /// <param name="trackChanges">Flag to indicate whether to track changes in the entities.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the paged list of cities.</returns>
    Task<PagedList<City>> GetCitiesAsync(PagingParameters paging, bool trackChanges = false);

    /// <summary>
    ///     Get a city by ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the city.</param>
    /// <param name="trackChanges">Flag to indicate whether to track changes in the entity.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the city.</returns>
    Task<City?> GetCityAsync(int id, bool trackChanges = false);

    /// <summary>
    ///     Create a new city.
    /// </summary>
    /// <param name="city">The city to create.</param>
    void CreateCity(City city);

    /// <summary>
    ///     Delete a city.
    /// </summary>
    /// <param name="city">The city to delete.</param>
    void DeleteCity(City city);
}
