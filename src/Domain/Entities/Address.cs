namespace HospitalRegistrationSystem.Domain.Entities;

public class Address
{
    public int CityId { get; set; }
    public City? City { get; set; }

    public string Street { get; set; }
}