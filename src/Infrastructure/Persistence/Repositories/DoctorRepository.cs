using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalRegistrationSystem.Infrastructure.Persistence.Repositories
{
    public class DoctorRepository : RepositoryBase<Doctor>, IDoctorRepository
    {
        public DoctorRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        { }

        public void CreateDoctor(Doctor doctor) =>
            Create(doctor);

        public void DeleteDoctor(Doctor doctor) =>
            Delete(doctor);

        public async Task<Doctor> GetDoctorAsync(int id, bool trackChanges) =>
            await FindByCondition(d => d.Id == id, trackChanges)
                .SingleOrDefaultAsync();

        public async Task<IEnumerable<Doctor>> GetDoctorsAsync(bool trackChanges) =>
            await FindAll(trackChanges)
                .OrderBy(d => d.Id)
                .ToListAsync();
    }
}
