using System.Linq;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalRegistrationSystem.Infrastructure.Persistence.Repositories;

/// <summary>
///     Repository for managing doctor schedules.
/// </summary>
public class DoctorScheduleRepository : RepositoryBase<DoctorSchedule>, IDoctorScheduleRepository
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="DoctorScheduleRepository" /> class.
    /// </summary>
    /// <param name="applicationContext">The application context.</param>
    public DoctorScheduleRepository(ApplicationContext applicationContext) : base(applicationContext)
    {
    }

    /// <inheritdoc />
    public async Task<PagedList<DoctorSchedule>> GetDoctorSchedulesAsync(DoctorScheduleParameters parameters,
        int doctorId, bool trackChanges = false)
    {
        var doctorSchedulesQuery = FindByCondition(ds => ds.DoctorId == doctorId
                                                         && ds.Date >= parameters.From
                                                         && ds.Date <= parameters.To,
                trackChanges)
            .OrderByDescending(ds => ds.Date);

        return await PagedList<DoctorSchedule>.ToPagedListAsync(doctorSchedulesQuery, parameters.PageNumber,
            parameters.PageSize);
    }

    /// <inheritdoc />
    public Task<DoctorSchedule?> GetDoctorScheduleByIdAsync(int id, bool trackChanges = false)
    {
        var doctorSchedule = FindByCondition(ds => ds.Id == id, trackChanges)
            .SingleOrDefaultAsync();

        return doctorSchedule;
    }

    /// <inheritdoc />
    public void CreateDoctorSchedule(DoctorSchedule doctorSchedule)
    {
        Create(doctorSchedule);
    }

    /// <inheritdoc />
    public void UpdateDoctorSchedule(DoctorSchedule doctorSchedule)
    {
        Update(doctorSchedule);
    }

    /// <inheritdoc />
    public void DeleteDoctorSchedule(DoctorSchedule doctorSchedule)
    {
        Delete(doctorSchedule);
    }
}