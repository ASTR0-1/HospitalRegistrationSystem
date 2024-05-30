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

namespace HospitalRegistrationSystem.Tests.Application.Service.RegionServ;

[TestFixture]
public class RegionServiceTests
{
    private Mock<IRepositoryManager> _mock = null!;
    private RegionService? _regionService;
    private IMapper? _mapper;

    [SetUp]
    public void SetUp()
    {
        _mock = new Mock<IRepositoryManager>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        _mapper = config.CreateMapper();

        _regionService = new RegionService(_mapper, _mock.Object);
    }

    [Test]
    public async Task GetAllAsync_WhenCalled_ReturnsAllRegions()
    {
        // Arrange
        var regions = new PagedList<Region>(new List<Region>(), 1, 1, 1);
        _mock.Setup(x => x.Region.GetRegionsAsync(It.IsAny<PagingParameters>(), false)).ReturnsAsync(regions);

        // Act
        var result = await _regionService.GetAllAsync(new PagingParameters());

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public async Task GetAsync_ExistingRegionId_ReturnsRegion()
    {
        // Arrange
        var existingId = 1;
        var region = new Region { Id = existingId };
        _mock.Setup(x => x.Region.GetRegionAsync(existingId, false)).ReturnsAsync(region);

        // Act
        var result = await _regionService.GetAsync(existingId);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
    }

    [Test]
    public async Task AddNewAsync_NewRegion_AddsRegionToDb()
    {
        // Arrange
        var regionDto = new RegionDto { Name = "T" };
        _mock.Setup(x => x.Region.CreateRegion(It.IsAny<Region>()));
        _mock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _regionService.AddNewAsync(regionDto);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public async Task DeleteAsync_ExistingRegionId_DeletesRegionFromDb()
    {
        // Arrange
        var existingId = 1;
        var region = new Region { Id = existingId };
        _mock.Setup(x => x.Region.GetRegionAsync(existingId, false)).ReturnsAsync(region);
        _mock.Setup(x => x.Region.DeleteRegion(It.IsAny<Region>()));
        _mock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _regionService.DeleteAsync(existingId);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }
}
