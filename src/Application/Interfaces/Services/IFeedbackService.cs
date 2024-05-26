using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.DTOs.FeedbackDTOs;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Entities;
using HospitalRegistrationSystem.Domain.Shared.ResultPattern;

namespace HospitalRegistrationSystem.Application.Interfaces.Services;

/// <summary>
///     Represents a service for managing feedbacks.
/// </summary>
public interface IFeedbackService
{
    /// <summary>
    ///     Retrieves feedbacks by user ID asynchronously.
    /// </summary>
    /// <param name="paging">The paging parameters.</param>
    /// <param name="userId">The user ID.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the feedbacks.</returns>
    Task<Result<PagedList<FeedbackDto>>> GetFeedbacksByUserIdAsync(PagingParameters paging, int userId);

    /// <summary>
    ///     Retrieves the average rating for a user asynchronously.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the average rating.</returns>
    Task<Result<decimal>> GetAverageRatingAsync(int userId);

    /// <summary>
    ///     Creates a new feedback asynchronously.
    /// </summary>
    /// <param name="feedback">The feedback to create.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<Result> CreateFeedbackAsync(FeedbackForCreationDto feedback);
}
