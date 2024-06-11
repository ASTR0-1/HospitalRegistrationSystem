using System;
using HospitalRegistrationSystem.Application.DTOs.ApplicationUserDTOs;
using Newtonsoft.Json;

namespace HospitalRegistrationSystem.Application.DTOs.AppointmentDTOs;

[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public class AppointmentDto
{
    public int Id { get; set; }
    public DateTime VisitTime { get; set; }
    public string Diagnosis { get; set; }
    public bool IsVisited { get; set; }
    public ApplicationUserDto Doctor { get; set; }
    public ApplicationUserDto Client { get; set; }
}