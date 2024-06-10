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

namespace HospitalRegistrationSystem.Tests.Application.Service.CityServ;

[TestFixture]
public class CityServiceTests
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

        _cityService = new CityService(_mapper, _mock.Object);
    }

    private Mock<IRepositoryManager> _mock;
    private CityService _cityService;
    private IMapper _mapper;

    [Test]
    public async Task GetAllAsync_WhenCalled_ReturnsCities()
    {
        // Arrange
        var cities = new PagedList<City>(new List<City>(), 1, 1, 1);
        _mock.Setup(x => x.City.GetCitiesAsync(It.IsAny<PagingParameters>(), false)).ReturnsAsync(cities);

        // Act
        var result = await _cityService.GetAllAsync(new PagingParameters());

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public async Task AddNewAsync_ValidCity_CreatesCity()
    {
        // Arrange
        var cityDto = new CityDto {Name = "Test City"};
        _mock.Setup(x => x.City.CreateCity(It.IsAny<City>()));
        _mock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _cityService.AddNewAsync(cityDto);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }
}