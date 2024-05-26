using System.Collections.Generic;

namespace HospitalRegistrationSystem.Domain.Entities;

public class Hospital
{
    public int Id { get; set; }
    public string Name { get; set; }

    public decimal HospitalFeePercent { get; set; }

    public Address Address { get; set; }

    public ICollection<ApplicationUser> ApplicationUsers { get; set; } = new List<ApplicationUser>();
}
