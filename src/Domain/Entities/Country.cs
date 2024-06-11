using System.Collections.Generic;

namespace HospitalRegistrationSystem.Domain.Entities;

public class Country
{
    public int Id { get; set; }
    public string Name { get; set; }

    public string ISO2 { get; set; }
    public string ISO3 { get; set; }

    public ICollection<Region> Regions { get; set; } = new List<Region>();
}