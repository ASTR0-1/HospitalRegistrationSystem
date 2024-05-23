using System.Collections.Generic;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.DTOs.AppointmentDTOs;
using HospitalRegistrationSystem.Domain.Entities;

namespace HospitalRegistrationSystem.Application.Interfaces.Services;

public interface IAppointmentService
{
    Task AddNewAsync(Appointment appointment);

    Task<ClientAppointmentCardDto> MarkAsVisitedAsync(int appointmentId, string diagnosis);
    
    Task<IEnumerable<ClientAppointmentDto>> GetAllAsync();
    Task<ClientAppointmentDto> GetAsync(int appointmentId);
    Task<IEnumerable<ClientAppointmentDto>> GetByClientIdAsync(int clientId);
    Task<IEnumerable<ClientAppointmentCardDto>> GetVisitedByClientIdAsync(int clientId);
    Task<IEnumerable<DoctorAppointmentDto>> GetByDoctorIdAsync(int doctorId);
}