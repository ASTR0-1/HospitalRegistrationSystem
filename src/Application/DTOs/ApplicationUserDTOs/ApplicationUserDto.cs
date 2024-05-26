using Newtonsoft.Json;

namespace HospitalRegistrationSystem.Application.DTOs.ApplicationUserDTOs;

[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public class ApplicationUserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string Gender { get; set; }
    public string? Specialty { get; set; }
    public decimal? VisitCost { get; set; }
    public decimal? TotalServiceCost { get; set; }
    public int? HospitalId { get; set; }
    public string? ProfilePhotoUrl { get; set; }
}