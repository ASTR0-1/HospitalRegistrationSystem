using System.Collections.Generic;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.DTOs.UserDTOs;

namespace HospitalRegistrationSystem.Application.Interfaces.Services;

public interface IApplicationUserService
{
    Task<IEnumerable<ApplicationUserDto>> FindAsync(string searchString);
    Task<IEnumerable<ApplicationUserDto>> GetAllByRoleAsync();
    Task<ApplicationUserDto> GetAsync(int userId);
    Task<ApplicationUserDto> UpdateAsync(ApplicationUserDto applicationUserDto);
    Task<ApplicationUserDto> CreateAsync(ApplicationUserDto applicationUserDto);
}