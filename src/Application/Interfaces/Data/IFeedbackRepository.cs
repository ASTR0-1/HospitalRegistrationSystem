using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Entities;

namespace HospitalRegistrationSystem.Application.Interfaces.Data;

/// <summary>
///     Represents a repository for managing feedback data.
/// </summary>
public interface IFeedbackRepository
{
    /// <summary>
    ///     Adds a new feedback asynchronously.
    /// </summary>
    /// <param name="feedback">The feedback to add.</param>
    void CreateFeedback(Feedback feedback);

    /// <summary>
    ///     Gets the feedbacks for a specific user asynchronously.
    /// </summary>
    /// <param name="paging">The paging parameters.</param>
    /// <param name="userId">The user ID.</param>
    /// <returns>A task that represents the asynchronous operation and contains the paged list of feedbacks.</returns>
    Task<PagedList<Feedback>> GetFeedbacksByUserIdAsync(PagingParameters paging, int userId);

    /// <summary>
    ///     Gets the average rating for a specific user asynchronously.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>A task that represents the asynchronous operation and contains the average rating.</returns>
    Task<decimal> GetAverageRatingAsync(int userId);
}