using HospitalRegistrationSystem.Application.DTOs.TokenDTOs;

namespace HospitalRegistrationSystem.Application.DTOs.AuthenticationDTOs;

public class AuthenticationResultDto
{
    public int UserId { get; set; }
    public TokenDto Token { get; set; }
}