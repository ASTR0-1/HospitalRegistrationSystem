using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Utility;
using HospitalRegistrationSystem.Domain.Entities;

namespace HospitalRegistrationSystem.Application.Interfaces.Data;

/// <summary>
///     Represents the interface for accessing and manipulating hospitals in the data layer.
/// </summary>
public interface IHospitalRepository
{

    /// <summary>
    ///     Get hospitals asynchronously with paging and search query.
    /// </summary>
    /// <param name="paging">The paging parameters.</param>
    /// <param name="searchQuery">The search query.</param>
    /// <param name="trackChanges">Flag to indicate whether to track changes in the entities.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the paged list of hospitals.</returns>
    Task<PagedList<Hospital>> GetHospitalsAsync(PagingParameters paging, string searchQuery, bool trackChanges = false);

    /// <summary>
    ///     Get hospitals asynchronously with paging.
    /// </summary>
    /// <param name="paging">The paging parameters.</param>
    /// <param name="trackChanges">Flag to indicate whether to track changes in the entities.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the paged list of hospitals.</returns>
    Task<PagedList<Hospital>> GetHospitalsAsync(PagingParameters paging, bool trackChanges = false);

    /// <summary>
    ///     Get a hospital by ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the hospital.</param>
    /// <param name="trackChanges">Flag to indicate whether to track changes in the entity.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the hospital.</returns>
    Task<Hospital?> GetHospitalAsync(int id, bool trackChanges = false);

    /// <summary>
    ///     Create a new hospital.
    /// </summary>
    /// <param name="hospital">The hospital to create.</param>
    void CreateHospital(Hospital hospital);

    /// <summary>
    ///     Delete a hospital.
    /// </summary>
    /// <param name="hospital">The hospital to delete.</param>
    void DeleteHospital(Hospital hospital);
}
