using System.Linq;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Application.Utility;
using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalRegistrationSystem.Infrastructure.Persistence.Repositories;

/// <summary>
/// Represents a repository for managing regions.
/// </summary>
public class RegionRepository : RepositoryBase<Region>, IRegionRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RegionRepository"/> class.
    /// </summary>
    /// <param name="applicationContext">The application context.</param>
    public RegionRepository(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }

    /// <inheritdoc/>
    public async Task<Region?> GetRegionAsync(int id, bool trackChanges = false)
    {
        var region = await FindByCondition(r => r.Id == id, trackChanges)
            .SingleOrDefaultAsync();

        return region;
    }

    /// <inheritdoc/>
    public async Task<PagedList<Region>> GetRegionsAsync(PagingParameters paging, bool trackChanges = false)
    {
        var regionsQuery = FindAll(trackChanges)
            .OrderBy(r => r.Id);

        return await PagedList<Region>.ToPagedListAsync(regionsQuery, paging.PageNumber, paging.PageSize);
    }

    /// <inheritdoc/>
    public void CreateRegion(Region region) => Create(region);

    /// <inheritdoc/>
    public void DeleteRegion(Region region) => Delete(region);
}
