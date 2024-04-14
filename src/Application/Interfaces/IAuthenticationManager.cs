using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.DTOs.AuthenticationDTOs;
using HospitalRegistrationSystem.Domain.Entities;

namespace HospitalRegistrationSystem.Application.Interfaces;

public interface IAuthenticationManager
{
    Task<bool> ValidateUserAsync(UserForAuthenticationDto userForAuthentication);
    Task<string> CreateTokenAsync(ApplicationUser? user = null);
}
