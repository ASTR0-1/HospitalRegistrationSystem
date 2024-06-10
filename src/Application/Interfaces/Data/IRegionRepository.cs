using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Entities;

namespace HospitalRegistrationSystem.Application.Interfaces.Data;

/// <summary>
///     Represents the interface for accessing and manipulating regions in the data layer.
/// </summary>
public interface IRegionRepository
{
    /// <summary>
    ///     Get regions asynchronously with paging.
    /// </summary>
    /// <param name="paging">The paging parameters.</param>
    /// <param name="trackChanges">Flag to indicate whether to track changes in the entities.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the paged list of regions.</returns>
    Task<PagedList<Region>> GetRegionsAsync(PagingParameters paging, bool trackChanges = false);

    /// <summary>
    ///     Get a region by ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the region.</param>
    /// <param name="trackChanges">Flag to indicate whether to track changes in the entity.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the region.</returns>
    Task<Region?> GetRegionAsync(int id, bool trackChanges = false);

    /// <summary>
    ///     Create a new region.
    /// </summary>
    /// <param name="region">The region to create.</param>
    void CreateRegion(Region region);

    /// <summary>
    ///     Delete a region.
    /// </summary>
    /// <param name="region">The region to delete.</param>
    void DeleteRegion(Region region);
}