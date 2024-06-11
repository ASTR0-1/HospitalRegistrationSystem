using System.Linq;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalRegistrationSystem.Infrastructure.Persistence.Repositories;

/// <summary>
///     Repository for managing feedbacks.
/// </summary>
public class FeedbackRepository : RepositoryBase<Feedback>, IFeedbackRepository
{
    public FeedbackRepository(ApplicationContext applicationContext) : base(applicationContext)
    {
    }

    /// <inheritdoc />
    public async Task<PagedList<Feedback>> GetFeedbacksByUserIdAsync(PagingParameters paging, int userId)
    {
        var feedbacksQuery = FindByCondition(f => f.Appointment.ApplicationUsers.Any(au => au.Id == userId),
                false
                , f => f.Appointment
                , f => f.Appointment.ApplicationUsers)
            .OrderByDescending(f => f.FeedbackDate);

        return await PagedList<Feedback>.ToPagedListAsync(feedbacksQuery, paging.PageNumber, paging.PageSize);
    }

    /// <inheritdoc />
    public async Task<decimal> GetAverageRatingAsync(int userId)
    {
        var averageRating = await FindByCondition(f => f.Appointment.ApplicationUsers.Any(au => au.Id == userId),
                false
                , f => f.Appointment
                , f => f.Appointment.ApplicationUsers)
            .AverageAsync(f => f.Rating);

        return averageRating;
    }

    /// <inheritdoc />
    public void CreateFeedback(Feedback feedback)
    {
        Create(feedback);
    }
}