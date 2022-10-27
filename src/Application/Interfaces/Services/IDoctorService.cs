using System.Collections.Generic;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Interfaces.DTOs;
using HospitalRegistrationSystem.Domain.Entities;

namespace HospitalRegistrationSystem.Application.Interfaces.Services
{
    public interface IDoctorService
    {
        Task AddNewAsync(Doctor doctor);
        Task<IEnumerable<DoctorCardDTO>> FindAsync(string searchString);
        Task<IEnumerable<DoctorCardDTO>> GetAllAsync();
    }
}
