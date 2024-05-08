using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Domain.Shared;

namespace HospitalRegistrationSystem.Domain.Errors;

public class RegionError
{
    public static Error RegionIdNotFound(int regionId) =>
        new("Region id not found", $"The region with ID '{regionId}' does not exist.");
}