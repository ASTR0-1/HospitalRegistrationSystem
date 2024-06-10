using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HospitalRegistrationSystem.Application.DTOs.LocationDTOs;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Application.Mappers;
using HospitalRegistrationSystem.Application.Services;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Entities;
using Moq;
using NUnit.Framework;

namespace HospitalRegistrationSystem.Tests.Application.Service.CountryServ;

[TestFixture]
public class CountryServiceTests
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

        _countryService = new CountryService(_mapper, _mock.Object);
    }

    private Mock<IRepositoryManager> _mock;
    private CountryService _countryService;
    private IMapper _mapper;

    [Test]
    public async Task GetAllAsync_ValidParameters_ReturnsCountries()
    {
        // Arrange
        var countries = new PagedList<Country>(new List<Country>(), 1, 1, 1);
        _mock.Setup(x => x.Country.GetCountriesAsync(It.IsAny<PagingParameters>(), false)).ReturnsAsync(countries);

        // Act
        var result = await _countryService.GetAllAsync(new PagingParameters());

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public async Task GetAsync_ValidCountryId_ReturnsCountry()
    {
        // Arrange
        _mock.Setup(x => x.Country.GetCountryAsync(It.IsAny<int>(), false)).ReturnsAsync(new Country());

        // Act
        var result = await _countryService.GetAsync(1);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public async Task AddNewAsync_ValidCountry_AddsCountry()
    {
        // Arrange
        var countryDto = new CountryDto();
        _mock.Setup(x => x.Country.CreateCountry(It.IsAny<Country>()));
        _mock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _countryService.AddNewAsync(countryDto);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public async Task DeleteAsync_ValidCountryId_DeletesCountry()
    {
        // Arrange
        _mock.Setup(x => x.Country.GetCountryAsync(It.IsAny<int>(), false)).ReturnsAsync(new Country());
        _mock.Setup(x => x.Country.DeleteCountry(It.IsAny<Country>()));
        _mock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _countryService.DeleteAsync(1);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }
}