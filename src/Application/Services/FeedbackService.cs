using System.Threading.Tasks;
using AutoMapper;
using HospitalRegistrationSystem.Application.DTOs.FeedbackDTOs;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Application.Interfaces.Services;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Entities;
using HospitalRegistrationSystem.Domain.Errors;
using HospitalRegistrationSystem.Domain.Shared.ResultPattern;

namespace HospitalRegistrationSystem.Application.Services;

/// <summary>
///     Represents a feedback service.
/// </summary>
public class FeedbackService : IFeedbackService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    /// <summary>
    ///     Initializes a new instance of the <see cref="FeedbackService"/> class.
    /// </summary>
    /// <param name="repository">The repository manager.</param>
    /// <param name="mapper">The mapper.</param>
    public FeedbackService(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<Result<PagedList<FeedbackDto>>> GetFeedbacksByUserIdAsync(PagingParameters paging, int userId)
    {
        var feedbacks = await _repository.Feedback.GetFeedbacksByUserIdAsync(paging, userId);
        var feedbackDtos = _mapper.Map<PagedList<FeedbackDto>>(feedbacks);

        return Result<PagedList<FeedbackDto>>.Success(feedbackDtos);
    }

    /// <inheritdoc />
    public async Task<Result<decimal>> GetAverageRatingAsync(int userId)
    {
        var user = await _repository.ApplicationUser.GetApplicationUserAsync(userId);
        if (user is null)
            return Result<decimal>.Failure(ApplicationUserError.UserIdNotFound(userId));

        var averageRating = await _repository.Feedback.GetAverageRatingAsync(userId);

        return Result<decimal>.Success(averageRating);
    }

    /// <inheritdoc />
    public async Task<Result> CreateFeedbackAsync(FeedbackForCreationDto feedbackDto)
    {
        var feedback = _mapper.Map<Feedback>(feedbackDto);

        var appointment = await _repository.Appointment.GetAppointmentAsync(feedback.AppointmentId);
        if (appointment is null)
            return Result.Failure(AppointmentError.AppointmentNotFound(feedback.AppointmentId));

        if (!appointment.IsVisited)
            return Result.Failure(FeedbackError.FeedbackOnNotVisitedAppointmentError(feedback.AppointmentId));

        _repository.Feedback.CreateFeedback(feedback);
        await _repository.SaveAsync();

        return Result.Success();
    }
}
