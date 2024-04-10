using System.Collections.Generic;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.DTOs;

namespace HospitalRegistrationSystem.Application.Interfaces.Services;

public interface IUserService
{
    Task<IEnumerable<UserCardDTO>> FindAsync(string searchString);
    Task<IEnumerable<UserCardDTO>> GetAllByRoleAsync();
    Task<UserCardDTO> GetAsync(int userId);
}
