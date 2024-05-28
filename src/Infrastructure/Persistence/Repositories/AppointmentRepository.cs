using System;
using System.Linq;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalRegistrationSystem.Infrastructure.Persistence.Repositories;

/// <summary>
///     
/// </summary>
public class AppointmentRepository : RepositoryBase<Appointment>, IAppointmentRepository
{
    /// <summary>
    ///     
    /// </summary>
    /// <param name="applicationContext"></param>
    public AppointmentRepository(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }

    /// <inheritdoc/>
    public Task<PagedList<Appointment>> GetIncomingAppointmentsByUserIdAsync(PagingParameters paging, int userId, bool trackChanges = false)
    {
        var appointments = FindByCondition(a => a.ApplicationUsers.Any(u => u.Id == userId)
                                                && a.VisitTime > DateTime.Now
                , trackChanges
                , a => a.ApplicationUsers)
            .OrderBy(a => a.Id);

        return PagedList<Appointment>.ToPagedListAsync(appointments, paging.PageNumber, paging.PageSize);
    }

    /// <inheritdoc/>
    public async Task<Appointment?> GetAppointmentAsync(int id, bool trackChanges = false)
    {
        var appointment = await FindByCondition(a => a.Id == id
                ,trackChanges
                ,a => a.ApplicationUsers)
            .SingleOrDefaultAsync();

        return appointment;
    }

    /// <inheritdoc/>
    public async Task<PagedList<Appointment>> GetAppointmentsByUserIdAsync(PagingParameters paging, int userId, bool? isVisited = null, bool trackChanges = false)
    {
        var appointments = FindByCondition(a => a.ApplicationUsers.Any(u => u.Id == userId)
                && (!isVisited.HasValue || a.IsVisited == isVisited)
                ,trackChanges
                ,a => a.ApplicationUsers)
            .OrderBy(a => a.Id);

        return await PagedList<Appointment>.ToPagedListAsync(appointments, paging.PageNumber, paging.PageSize);
    }

    /// <inheritdoc/>
    public void CreateAppointment(Appointment appointment) => Create(appointment);

    /// <inheritdoc/>
    public void DeleteAppointment(Appointment appointment) => Delete(appointment);
}