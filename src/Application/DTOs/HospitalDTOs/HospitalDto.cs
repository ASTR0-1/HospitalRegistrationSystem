namespace HospitalRegistrationSystem.Application.DTOs.HospitalDTOs;

public class HospitalDto
{
    public int Id { get; set; }
    public string Name { get; set; }

    public string Country { get; set; }
    public string Region { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
}