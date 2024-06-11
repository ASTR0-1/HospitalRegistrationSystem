using System.Linq;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Entities;
using HospitalRegistrationSystem.Infrastructure.Persistence.Repositories;
using NUnit.Framework;

namespace HospitalRegistrationSystem.Tests.Infrastructure.Repository.RegionRepo;

[TestFixture]
public class RegionRepoTests
{
    [SetUp]
    public void SetUpFixture()
    {
        _dataFixture = new RegionSeedDataFixture();
        _regionRepository = new RegionRepository(_dataFixture.ApplicationContext);
    }

    [TearDown]
    public void TearDownFixture()
    {
        _dataFixture.Dispose();
    }

    private IRegionRepository _regionRepository;
    private RegionSeedDataFixture _dataFixture;

    [Test]
    public async Task GetRegionsAsync_GetValue()
    {
        // Arrange
        var expectedCount = 2;
        var parameters = new PagingParameters
        {
            PageNumber = 1,
            PageSize = 10
        };

        // Act
        var actualCount = (await _regionRepository.GetRegionsAsync(parameters)).Count;

        // Assert
        Assert.That(actualCount, Is.EqualTo(expectedCount));
    }

    [Test]
    public async Task GetRegionAsync_GetValue()
    {
        // Arrange
        var expectedName = "Region1";

        // Act
        var actualName = (await _regionRepository.GetRegionAsync(1)).Name;

        // Assert
        Assert.That(actualName, Is.EqualTo(expectedName));
    }

    [Test]
    public void CreateRegion_Success()
    {
        // Arrange
        var region = new Region {Id = 3, Name = "Region3"};

        // Act
        _regionRepository.CreateRegion(region);
        _dataFixture.ApplicationContext.SaveChanges();

        // Assert
        Assert.That(_dataFixture.ApplicationContext.Regions.Count(), Is.EqualTo(3));
    }
}