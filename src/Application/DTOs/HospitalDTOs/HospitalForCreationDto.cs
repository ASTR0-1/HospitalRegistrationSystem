using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalRegistrationSystem.Application.DTOs.HospitalDTOs;

public class HospitalForCreationDto
{
    public string Name { get; set; }

    public int CityId { get; set; }
    public string Street { get; set; }
}
