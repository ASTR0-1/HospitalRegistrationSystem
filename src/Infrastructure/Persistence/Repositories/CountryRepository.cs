using System.Linq;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalRegistrationSystem.Infrastructure.Persistence.Repositories;

/// <summary>
///     Represents a repository for managing countries.
/// </summary>
public class CountryRepository : RepositoryBase<Country>, ICountryRepository
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CountryRepository" /> class.
    /// </summary>
    /// <param name="applicationContext">The application context.</param>
    public CountryRepository(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }

    /// <inheritdoc />
    public async Task<Country?> GetCountryAsync(int id, bool trackChanges = false)
    {
        var country = await FindByCondition(c => c.Id == id, trackChanges)
            .SingleOrDefaultAsync();

        return country;
    }

    /// <inheritdoc />
    public async Task<PagedList<Country>> GetCountriesAsync(PagingParameters paging, bool trackChanges = false)
    {
        var countriesQuery = FindAll(trackChanges)
            .OrderBy(c => c.Id);

        return await PagedList<Country>.ToPagedListAsync(countriesQuery, paging.PageNumber, paging.PageSize);
    }

    /// <inheritdoc />
    public void CreateCountry(Country country)
    {
        Create(country);
    }

    /// <inheritdoc />
    public void DeleteCountry(Country country)
    {
        Delete(country);
    }
}