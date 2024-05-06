using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalRegistrationSystem.Domain.Entities;

public class Hospital
{
    public int Id { get; set; }
    public string Name { get; set; }

    public Address Address { get; set; }

    public ICollection<ApplicationUser> ApplicationUsers { get; set; } = new List<ApplicationUser>();
}
