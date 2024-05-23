using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.DTOs.HospitalDTOs;
using HospitalRegistrationSystem.Application.Utility;
using HospitalRegistrationSystem.Domain.Shared.ResultPattern;

namespace HospitalRegistrationSystem.Application.Interfaces.Services;

/// <summary>
///     Represents the interface for the Hospital service.
/// </summary>
public interface IHospitalService
{
    /// <summary>
    ///     Retrieves a list of hospitals asynchronously.
    /// </summary>
    /// <param name="paging">The paging parameters.</param>
    /// <param name="searchQuery">The search query.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of hospitals.</returns>
    Task<Result<PagedList<HospitalDto>>> GetHospitalsAsync(PagingParameters paging, string? searchQuery);

    /// <summary>
    ///     Retrieves a hospital by ID asynchronously.
    /// </summary>
    /// <param name="hospitalId">The ID of the hospital.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the hospital.</returns>
    Task<Result<HospitalDto>> GetHospitalAsync(int hospitalId);

    /// <summary>
    ///     Creates a new hospital asynchronously.
    /// </summary>
    /// <param name="hospitalForCreationDto">The hospital for creation DTO.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<Result> CreateHospitalAsync(HospitalForCreationDto hospitalForCreationDto);

    /// <summary>
    ///     Deletes a hospital by ID asynchronously.
    /// </summary>
    /// <param name="hospitalId">The ID of the hospital.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<Result> DeleteHospitalAsync(int hospitalId);
}
