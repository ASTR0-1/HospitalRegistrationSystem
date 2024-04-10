using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalRegistrationSystem.Infrastructure.Persistence.Repositories;

public class AppointmentRepository : RepositoryBase<Appointment>, IAppointmentRepository
{
    public AppointmentRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
    {
    }

    public void CreateAppointment(Appointment appointment)
    {
        Create(appointment);
    }

    public void DeleteAppointment(Appointment appointment)
    {
        Delete(appointment);
    }

    public async Task<Appointment> GetAppointmentAsync(Guid id, bool trackChanges)
    {
        return await FindByCondition(a => a.Id == id, trackChanges)
            .Include(a => a.ApplicationUsers)
            .SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<Appointment>> GetAppointmentsAsync(bool trackChanges)
    {
        return await FindAll(trackChanges)
            .Include(a => a.ApplicationUsers)
            .OrderBy(a => a.Id)
            .ToListAsync();
    }
}