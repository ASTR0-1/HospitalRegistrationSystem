using System.Collections.Generic;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Domain.Entities;

namespace HospitalRegistrationSystem.Application.Interfaces.Data;

public interface IDoctorRepository
{
    Task<IEnumerable<Doctor>> GetDoctorsAsync(bool trackChanges);
    Task<Doctor> GetDoctorAsync(int id, bool trackChanges);
    void CreateDoctor(Doctor doctor);
    void DeleteDoctor(Doctor doctor);
}