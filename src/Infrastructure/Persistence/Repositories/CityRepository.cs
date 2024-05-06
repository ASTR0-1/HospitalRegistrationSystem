using System.Linq;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Application.Utility;
using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalRegistrationSystem.Infrastructure.Persistence.Repositories;

/// <summary>
///     Represents a repository for managing cities.
/// </summary>
public class CityRepository : RepositoryBase<City>, ICityRepository
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CityRepository"/> class.
    /// </summary>
    /// <param name="applicationContext">The application context.</param>
    public CityRepository(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }

    /// <inheritdoc/>
    public async Task<City?> GetCityAsync(int id, bool trackChanges = false)
    {
        var city = await FindByCondition(c => c.Id == id, trackChanges)
            .SingleOrDefaultAsync();

        return city;
    }

    /// <inheritdoc/>
    public async Task<PagedList<City>> GetCitiesAsync(PagingParameters paging, bool trackChanges = false)
    {
        var citiesQuery = FindAll(trackChanges)
            .OrderBy(c => c.Id);

        return await PagedList<City>.ToPagedListAsync(citiesQuery, paging.PageNumber, paging.PageSize);
    }

    /// <inheritdoc/>
    public void CreateCity(City city) => Create(city);

    /// <inheritdoc/>
    public void DeleteCity(City city) => Delete(city);
}
