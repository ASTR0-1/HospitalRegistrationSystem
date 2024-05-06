using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Application.Utility;
using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalRegistrationSystem.Infrastructure.Persistence.Repositories;

public class AppointmentRepository : RepositoryBase<Appointment>, IAppointmentRepository
{
    public AppointmentRepository(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }

    public async Task<Appointment?> GetAppointmentAsync(int id, bool trackChanges = false)
    {
        var appointment = await FindByCondition(a => a.Id == id, trackChanges)
            .Include(a => a.ApplicationUsers)
            .SingleOrDefaultAsync();

        return appointment;
    }

    public async Task<PagedList<Appointment>> GetAppointmentsAsync(PagingParameters paging, bool trackChanges = false)
    {
        var appointments = FindAll(trackChanges)
            .Include(a => a.ApplicationUsers)
            .OrderBy(a => a.Id);

        return await PagedList<Appointment>.ToPagedListAsync(appointments, paging.PageNumber, paging.PageSize);
    }
    public void CreateAppointment(Appointment appointment) => Create(appointment);

    public void DeleteAppointment(Appointment appointment) => Delete(appointment);
}