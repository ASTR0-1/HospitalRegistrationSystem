using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.DTOs.FeedbackDTOs;
using HospitalRegistrationSystem.Application.Interfaces;
using HospitalRegistrationSystem.Application.Interfaces.Services;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalRegistrationSystem.WebAPI.Controllers;

/// <summary>
///     Controller for managing feedbacks.
/// </summary>
[ApiController]
[Route("api/feedbacks")]
public class FeedbackController : ControllerBase
{
    private readonly IFeedbackService _feedbackService;
    private readonly ILoggerManager _logger;

    /// <summary>
    ///     Initializes a new instance of the <see cref="FeedbackController" /> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="feedbackService">The feedback service.</param>
    public FeedbackController(ILoggerManager logger, IFeedbackService feedbackService)
    {
        _logger = logger;
        _feedbackService = feedbackService;
    }

    /// <summary>
    ///     Gets all feedbacks for a specific user.
    /// </summary>
    /// <param name="parameters">The paging parameters.</param>
    /// <param name="userId">The user ID.</param>
    /// <returns>The paged list of feedback DTOs.</returns>
    [HttpGet]
    public async Task<ActionResult<PagedList<FeedbackDto>>> GetAll([FromQuery] PagingParameters parameters,
        [FromQuery] int userId)
    {
        var result = await _feedbackService.GetFeedbacksByUserIdAsync(parameters, userId);
        if (!result.IsSuccess)
        {
            _logger.LogInformation(result.Error.Description);

            return NotFound(result.Error.Message);
        }

        var feedbackDtos = result.Value;

        return Ok(feedbackDtos);
    }

    /// <summary>
    ///     Gets the average rating for a specific user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>The average rating.</returns>
    [HttpGet("averageRating/{userId:int}")]
    public async Task<ActionResult<decimal>> GetAverageRating(int userId)
    {
        var result = await _feedbackService.GetAverageRatingAsync(userId);
        if (!result.IsSuccess)
        {
            _logger.LogInformation(result.Error.Description);

            return NotFound(result.Error.Message);
        }

        var averageRating = result.Value;

        return Ok(averageRating);
    }

    /// <summary>
    ///     Adds a new feedback.
    /// </summary>
    /// <param name="feedbackDto">The feedback DTO.</param>
    /// <returns>The result of the operation.</returns>
    [Authorize(Roles = RoleConstants.Client)]
    [HttpPost]
    public async Task<IActionResult> AddNew([FromBody] FeedbackForCreationDto feedbackDto)
    {
        var result = await _feedbackService.CreateFeedbackAsync(feedbackDto);
        if (!result.IsSuccess)
        {
            _logger.LogInformation(result.Error.Description);

            return BadRequest(result.Error.Message);
        }

        return Ok();
    }
}