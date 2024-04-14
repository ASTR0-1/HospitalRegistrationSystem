using System.Collections.Generic;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.DTOs.UserDTOs;

namespace HospitalRegistrationSystem.Application.Interfaces.Services;

public interface IUserService
{
    Task<IEnumerable<UserCardDto>> FindAsync(string searchString);
    Task<IEnumerable<UserCardDto>> GetAllByRoleAsync();
    Task<UserCardDto> GetAsync(int userId);
}
