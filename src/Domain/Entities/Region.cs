using System.Collections.Generic;

namespace HospitalRegistrationSystem.Domain.Entities;

public class Region
{
    public int Id { get; set; }
    public string Name { get; set; }

    public int CountryId { get; set; }
    public Country? Country { get; set; }

    public ICollection<City> Cities { get; set; } = new List<City>();
}