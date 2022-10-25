using System.Collections.Generic;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Interfaces.DTO;
using HospitalRegistrationSystem.Domain.Entities;

namespace HospitalRegistrationSystem.Application.Interfaces.Services
{
    public interface IDoctorService
    {
        void AddNew(Doctor doctor);
        Task<IEnumerable<DoctorCardDTO>> FindAsync(string searchString);
        Task<IEnumerable<DoctorCardDTO>> GetAllAsync();
    }
}
