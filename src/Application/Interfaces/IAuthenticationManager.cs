using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.DTOs;

namespace HospitalRegistrationSystem.Application.Interfaces;

public interface IAuthenticationManager
{
    Task<bool> ValidateUserAsync(UserForAuthenticationDto userForAuthentication);
    Task<string> CreateTokenAsync();
}
