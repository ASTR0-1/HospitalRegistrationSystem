using Newtonsoft.Json;

namespace HospitalRegistrationSystem.Application.DTOs.UserDTOs;

[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public class UserCardDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string Gender { get; set; }
    public string? Specialty { get; set; }
    public string? ProfilePhotoUrl { get; set; }
}
