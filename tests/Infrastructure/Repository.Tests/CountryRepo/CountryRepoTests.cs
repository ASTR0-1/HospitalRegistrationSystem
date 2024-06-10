using System.Linq;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Entities;
using HospitalRegistrationSystem.Infrastructure.Persistence.Repositories;
using NUnit.Framework;

namespace HospitalRegistrationSystem.Tests.Infrastructure.Repository.CountryRepo;

[TestFixture]
public class CountryRepoTests
{
    [SetUp]
    public void SetUpFixture()
    {
        _dataFixture = new CountrySeedDataFixture();
        _countryRepository = new CountryRepository(_dataFixture.ApplicationContext);
    }

    [TearDown]
    public void TearDownFixture()
    {
        _dataFixture.Dispose();
    }

    private ICountryRepository _countryRepository;
    private CountrySeedDataFixture _dataFixture;

    [Test]
    public async Task GetCountriesAsync_GetValue()
    {
        // Arrange
        var expectedCount = 2;

        // Act
        var actualCount = (await _countryRepository.GetCountriesAsync(new PagingParameters())).Count;

        // Assert
        Assert.That(actualCount, Is.EqualTo(expectedCount));
    }

    [Test]
    public async Task GetCountryAsync_NotExistingId_GetNull()
    {
        // Arrange
        var notExistingId = 0;
        Country expectedCountry = null;

        // Act
        var actualCountry = await _countryRepository.GetCountryAsync(notExistingId);

        // Assert
        Assert.That(actualCountry, Is.EqualTo(expectedCountry));
    }

    [Test]
    public async Task GetCountryAsync_ExistingId_GetValue()
    {
        // Arrange
        var expectedId = 1;

        // Act
        var actualId = (await _countryRepository.GetCountryAsync(expectedId)).Id;

        // Assert
        Assert.That(actualId, Is.EqualTo(expectedId));
    }

    [Test]
    public void CreateCountry_Success()
    {
        // Arrange
        var country = new Country {Id = 3, Name = "Country3", ISO2 = "C3", ISO3 = "C3Y"};

        // Act
        _countryRepository.CreateCountry(country);
        _dataFixture.ApplicationContext.SaveChanges();

        // Assert
        Assert.That(_dataFixture.ApplicationContext.Countries.Count(), Is.EqualTo(3));
    }

    [Test]
    public void DeleteCountry_Success()
    {
        // Arrange
        var country = _dataFixture.ApplicationContext.Countries.First();

        // Act
        _countryRepository.DeleteCountry(country);
        _dataFixture.ApplicationContext.SaveChanges();

        // Assert
        Assert.That(_dataFixture.ApplicationContext.Countries.Count(), Is.EqualTo(1));
    }
}