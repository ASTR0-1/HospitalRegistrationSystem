using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalRegistrationSystem.Domain.Entities;

public class DoctorSchedule
{
    public int Id { get; set; }
    
    public int DoctorId { get; set; }
    public ApplicationUser Doctor { get; set; }

    public int WorkingHours { get; set; }
    public DateOnly Date { get; set; }
}
