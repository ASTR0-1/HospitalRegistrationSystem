using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HospitalRegistrationSystem.Application.DTOs.FeedbackDTOs;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Application.Mappers;
using HospitalRegistrationSystem.Application.Services;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Entities;
using Moq;
using NUnit.Framework;

namespace HospitalRegistrationSystem.Tests.Application.Service.FeedbackServ;

[TestFixture]
public class FeedbackServiceTests
{
    [SetUp]
    public void SetUp()
    {
        _mock = new Mock<IRepositoryManager>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
            cfg.CreateMap(typeof(PagedList<>), typeof(PagedList<>))
                .ConvertUsing(typeof(PagedListConverter<,>));
        });

        _mapper = config.CreateMapper();

        _feedbackService = new FeedbackService(_mock.Object, _mapper);
    }

    private Mock<IRepositoryManager> _mock;
    private FeedbackService _feedbackService;
    private IMapper _mapper;

    [Test]
    public async Task GetFeedbacksByUserIdAsync_WhenCalled_ReturnsFeedbacks()
    {
        // Arrange
        var feedbacks = new PagedList<Feedback>(new List<Feedback>(), 1, 1, 1);
        _mock.Setup(x => x.Feedback.GetFeedbacksByUserIdAsync(It.IsAny<PagingParameters>(), It.IsAny<int>()))
            .ReturnsAsync(feedbacks);

        // Act
        var result = await _feedbackService.GetFeedbacksByUserIdAsync(new PagingParameters(), 1);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public async Task CreateFeedbackAsync_ValidFeedback_CreatesFeedback()
    {
        // Arrange
        var feedbackDto = new FeedbackForCreationDto {AppointmentId = 1};
        var appointment = new Appointment {IsVisited = true};
        _mock.Setup(x => x.Appointment.GetAppointmentAsync(It.IsAny<int>(), false)).ReturnsAsync(appointment);
        _mock.Setup(x => x.Feedback.CreateFeedback(It.IsAny<Feedback>()));
        _mock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _feedbackService.CreateFeedbackAsync(feedbackDto);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }
}