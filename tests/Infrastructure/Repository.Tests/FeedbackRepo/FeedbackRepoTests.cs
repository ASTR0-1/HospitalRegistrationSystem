using System;
using System.Linq;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Entities;
using HospitalRegistrationSystem.Infrastructure.Persistence.Repositories;
using NUnit.Framework;

namespace HospitalRegistrationSystem.Tests.Infrastructure.Repository.FeedbackRepo;
[TestFixture]
public class FeedbackRepoTests
{
    [SetUp]
    public void SetUpFixture()
    {
        _dataFixture = new FeedbackSeedDataFixture();
        _feedbackRepository = new FeedbackRepository(_dataFixture.ApplicationContext);
    }

    [TearDown]
    public void TearDownFixture()
    {
        _dataFixture.Dispose();
    }

    private IFeedbackRepository _feedbackRepository;
    private FeedbackSeedDataFixture _dataFixture;

    [Test]
    public async Task GetFeedbacksByUserIdAsync_GetValue()
    {
        // Arrange
        var expectedCount = 2;
        var parameters = new PagingParameters
        {
            PageNumber = 1,
            PageSize = 10
        };

        // Act
        var actualCount = (await _feedbackRepository.GetFeedbacksByUserIdAsync(parameters, 1)).Count;

        // Assert
        Assert.That(actualCount, Is.EqualTo(expectedCount));
    }

    [Test]
    public async Task GetAverageRatingAsync_GetValue()
    {
        // Arrange
        var expectedAverageRating = 4.5m;

        // Act
        var actualAverageRating = await _feedbackRepository.GetAverageRatingAsync(1);

        // Assert
        Assert.That(actualAverageRating, Is.EqualTo(expectedAverageRating));
    }

    [Test]
    public void CreateFeedback_Success()
    {
        // Arrange
        var feedback = new Feedback { Id = 3, AppointmentId = 1, FeedbackDate = new DateTime(2022, 1, 3), Rating = 3 };

        // Act
        _feedbackRepository.CreateFeedback(feedback);
        _dataFixture.ApplicationContext.SaveChanges();

        // Assert
        Assert.That(_dataFixture.ApplicationContext.Feedbacks.Count(), Is.EqualTo(3));
    }
}
