using System.ComponentModel.DataAnnotations;

namespace HospitalRegistrationSystem.Application.DTOs.AuthenticationDTOs;

public class UserForAuthenticationDto
{
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
}
