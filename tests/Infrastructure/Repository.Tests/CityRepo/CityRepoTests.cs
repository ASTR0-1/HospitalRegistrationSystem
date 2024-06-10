using System.Linq;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Entities;
using HospitalRegistrationSystem.Infrastructure.Persistence.Repositories;
using NUnit.Framework;

namespace HospitalRegistrationSystem.Tests.Infrastructure.Repository.CityRepo;

[TestFixture]
public class CityRepoTests
{
    [SetUp]
    public void SetUpFixture()
    {
        _dataFixture = new CitySeedDataFixture();
        _cityRepository = new CityRepository(_dataFixture.ApplicationContext);
    }

    [TearDown]
    public void TearDownFixture()
    {
        _dataFixture.Dispose();
    }

    private ICityRepository _cityRepository;
    private CitySeedDataFixture _dataFixture;

    [Test]
    public async Task GetCitiesAsync_GetValue()
    {
        // Arrange
        var expectedCount = 2;

        // Act
        var actualCount = (await _cityRepository.GetCitiesAsync(new PagingParameters())).Count;

        // Assert
        Assert.That(actualCount, Is.EqualTo(expectedCount));
    }

    [Test]
    public async Task GetCityAsync_NotExistingId_GetNull()
    {
        // Arrange
        var notExistingId = 0;
        City expectedCity = null;

        // Act
        var actualCity = await _cityRepository.GetCityAsync(notExistingId);

        // Assert
        Assert.That(actualCity, Is.EqualTo(expectedCity));
    }

    [Test]
    public async Task GetCityAsync_ExistingId_GetValue()
    {
        // Arrange
        var expectedId = 1;

        // Act
        var actualId = (await _cityRepository.GetCityAsync(expectedId)).Id;

        // Assert
        Assert.That(actualId, Is.EqualTo(expectedId));
    }

    [Test]
    public void CreateCity_Success()
    {
        // Arrange
        var city = new City {Id = 3, Name = "City3"};

        // Act
        _cityRepository.CreateCity(city);
        _dataFixture.ApplicationContext.SaveChanges();

        // Assert
        Assert.That(_dataFixture.ApplicationContext.Cities.Count(), Is.EqualTo(3));
    }

    [Test]
    public void DeleteCity_Success()
    {
        // Arrange
        var city = _dataFixture.ApplicationContext.Cities.First();

        // Act
        _cityRepository.DeleteCity(city);
        _dataFixture.ApplicationContext.SaveChanges();

        // Assert
        Assert.That(_dataFixture.ApplicationContext.Cities.Count(), Is.EqualTo(1));
    }
}