using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Entities;
using HospitalRegistrationSystem.Infrastructure.Persistence.Repositories;
using NUnit.Framework;

namespace HospitalRegistrationSystem.Tests.Infrastructure.Repository.HospitalRepo;

[TestFixture]
public class HospitalRepoTests
{
    [SetUp]
    public void SetUpFixture()
    {
        _dataFixture = new HospitalSeedDataFixture();
        _hospitalRepository = new HospitalRepository(_dataFixture.ApplicationContext);
    }

    [TearDown]
    public void TearDownFixture()
    {
        _dataFixture.Dispose();
    }

    private IHospitalRepository _hospitalRepository;
    private HospitalSeedDataFixture _dataFixture;

    [Test]
    public async Task GetHospitalsAsync_GetValue()
    {
        // Arrange
        var expectedCount = 2;
        var parameters = new PagingParameters
        {
            PageNumber = 1,
            PageSize = 10
        };

        // Act
        var actualCount = (await _hospitalRepository.GetHospitalsAsync(parameters)).Count;

        // Assert
        Assert.That(actualCount, Is.EqualTo(expectedCount));
    }

    [Test]
    public async Task GetHospitalAsync_GetValue()
    {
        // Arrange
        var expectedName = "Hospital1";

        // Act
        var actualName = (await _hospitalRepository.GetHospitalAsync(1)).Name;

        // Assert
        Assert.That(actualName, Is.EqualTo(expectedName));
    }

    [Test]
    public void CreateHospital_Success()
    {
        // Arrange
        var hospital = new Hospital { Id = 3, Name = "Hospital3", Address = new Address()
        {
            City = new City()
            {
                Id = 100,
                Name = "City100",
            },
            CityId = 100,
            Street = "New100"
        } };

        // Act
        _hospitalRepository.CreateHospital(hospital);
        _dataFixture.ApplicationContext.SaveChanges();

        // Assert
        Assert.That(_dataFixture.ApplicationContext.Hospitals.Count(), Is.EqualTo(3));
    }
}
