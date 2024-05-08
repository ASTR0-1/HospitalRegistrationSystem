using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Domain.Shared;

namespace HospitalRegistrationSystem.Domain.Errors;

public class CountryError
{
    public static Error CountryIdNotFound(int countryId) => new("Country id not found", $"The country with ID '{countryId}' does not exist.");
}