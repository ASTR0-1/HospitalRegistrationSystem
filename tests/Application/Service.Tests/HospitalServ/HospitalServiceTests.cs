using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HospitalRegistrationSystem.Application.DTOs.HospitalDTOs;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Application.Mappers;
using HospitalRegistrationSystem.Application.Services;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Entities;
using Moq;
using NUnit.Framework;

namespace HospitalRegistrationSystem.Tests.Application.Service.HospitalServ;

[TestFixture]
public class HospitalServiceTests
{
    private Mock<IRepositoryManager> _mock = null!;
    private HospitalService _hospitalService = null!;
    private IMapper _mapper = null!;

    [SetUp]
    public void SetUp()
    {
        _mock = new Mock<IRepositoryManager>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        _mapper = config.CreateMapper();

        _hospitalService = new HospitalService(_mock.Object, _mapper);
    }

    [Test]
    public async Task GetHospitalsAsync_WhenCalled_ReturnsAllHospitals()
    {
        // Arrange
        var hospitals = new PagedList<Hospital>(new List<Hospital>(), 1, 1, 1);
        _mock.Setup(x => x.Hospital.GetHospitalsAsync(It.IsAny<PagingParameters>(), false)).ReturnsAsync(hospitals);

        // Act
        var result = await _hospitalService.GetHospitalsAsync(new PagingParameters(), null);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public async Task GetHospitalAsync_ExistingHospitalId_ReturnsHospital()
    {
        // Arrange
        var existingId = 1;
        var hospital = new Hospital { Id = existingId };
        _mock.Setup(x => x.Hospital.GetHospitalAsync(existingId, false)).ReturnsAsync(hospital);

        // Act
        var result = await _hospitalService.GetHospitalAsync(existingId);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value?.Id, Is.EqualTo(existingId));
    }

    [Test]
    public async Task CreateHospitalAsync_NewHospital_AddsHospitalToDb()
    {
        // Arrange
        var hospitalDto = new HospitalForCreationDto { CityId = 1, HospitalFeePercent = 10 };
        var city = new City { Id = 1 };
        _mock.Setup(x => x.City.GetCityAsync(It.IsAny<int>(), false)).ReturnsAsync(city);
        _mock.Setup(x => x.Hospital.CreateHospital(It.IsAny<Hospital>()));
        _mock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _hospitalService.CreateHospitalAsync(hospitalDto);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public async Task DeleteHospitalAsync_ExistingHospitalId_DeletesHospitalFromDb()
    {
        // Arrange
        var existingId = 1;
        var hospital = new Hospital { Id = existingId };
        _mock.Setup(x => x.Hospital.GetHospitalAsync(existingId, false)).ReturnsAsync(hospital);
        _mock.Setup(x => x.Hospital.DeleteHospital(It.IsAny<Hospital>()));
        _mock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _hospitalService.DeleteHospitalAsync(existingId);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }
}
