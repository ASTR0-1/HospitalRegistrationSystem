using System.ComponentModel.DataAnnotations;

namespace HospitalRegistrationSystem.Application.DTOs;

public class UserForAuthenticationDto
{
    [Required(ErrorMessage = "Phone number is required")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
}
