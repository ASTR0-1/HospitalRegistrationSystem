using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Domain.Shared;

namespace HospitalRegistrationSystem.Domain.Errors;
public class CityError
{
    public static Error CityIdNotFound(int cityId) => new("City id not found", $"The city with ID '{cityId}' does not exist.");
}
