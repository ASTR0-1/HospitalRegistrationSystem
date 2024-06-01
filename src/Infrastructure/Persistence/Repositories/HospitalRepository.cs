using System.Linq;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalRegistrationSystem.Infrastructure.Persistence.Repositories;

/// <summary>
///     Represents a repository for managing hospitals.
/// </summary>
public class HospitalRepository : RepositoryBase<Hospital>, IHospitalRepository
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="HospitalRepository"/> class.
    /// </summary>
    /// <param name="applicationContext">The application context.</param>
    public HospitalRepository(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }

    /// <inheritdoc/>
    public async Task<PagedList<Hospital>> GetHospitalsAsync(PagingParameters paging, string searchQuery, bool trackChanges = false)
    {
        var searchTerms = searchQuery.Split(' ')
            .Select(term => $"%{term.ToLower().Trim()}%")
            .ToList();

        var hospitalsQuery = FindByCondition(
                    h => searchTerms.Any(term => EF.Functions.Like(h.Name, term) ||
                                                 EF.Functions.Like(h.Address.City!.Region!.Name, term) ||
                                                 EF.Functions.Like(h.Address.City.Name, term) ||
                                                 EF.Functions.Like(h.Address.City.Region.Name, term) ||
                                                 EF.Functions.Like(h.Address.City.Region.Country!.Name, term) ||
                                                 EF.Functions.Like(h.Address.Street, term)
                    ),
                    trackChanges,
                    h => h.Address.City!,
                    h => h.Address.City!.Region!,
                    h => h.Address.City!.Region!.Country!)
                .OrderBy(h => h.Id);

        return await PagedList<Hospital>.ToPagedListAsync(hospitalsQuery, paging.PageNumber, paging.PageSize);
    }

    /// <inheritdoc/>
    public async Task<PagedList<Hospital>> GetHospitalsAsync(PagingParameters paging, bool trackChanges = false)
    {
        var hospitalsQuery = FindAll(trackChanges)
            .OrderBy(h => h.Id);

        return await PagedList<Hospital>.ToPagedListAsync(hospitalsQuery, paging.PageNumber, paging.PageSize);
    }

    /// <inheritdoc/>
    public async Task<Hospital?> GetHospitalAsync(int id, bool trackChanges = false)
    {
        var hospital = await FindByCondition(h => h.Id == id, trackChanges)
            .SingleOrDefaultAsync();

        return hospital;
    }

    /// <inheritdoc/>
    public void CreateHospital(Hospital hospital) => Create(hospital);

    /// <inheritdoc/>
    public void DeleteHospital(Hospital hospital) => Delete(hospital);
}
