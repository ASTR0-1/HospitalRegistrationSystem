using System.ComponentModel.DataAnnotations;

namespace HospitalRegistrationSystem.Application.DTOs.ApplicationUserDTOs;

public class AssignEmployeeDto
{
   public required int HospitalId { get; set; }
   public string Specialty { get; set; }
   public decimal DoctorPrice { get; set; }
}
