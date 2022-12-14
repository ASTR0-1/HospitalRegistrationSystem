using System.Collections.Generic;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.DTOs;
using HospitalRegistrationSystem.Domain.Entities;

namespace HospitalRegistrationSystem.Application.Interfaces.Services;

public interface IAppointmentService
{
    Task AddNewAsync(Appointment appointment);
    Task<ClientAppointmentCardDTO> MarkAsVisitedAsync(int appointmentId, string diagnosis);
    Task<IEnumerable<ClientAppointmentDTO>> GetAllAsync();
    Task<ClientAppointmentDTO> GetAsync(int appointmentId);
    Task<IEnumerable<ClientAppointmentDTO>> GetByClientIdAsync(int clientId);
    Task<IEnumerable<ClientAppointmentCardDTO>> GetVisitedByClientIdAsync(int clientId);
    Task<IEnumerable<DoctorAppointmentDTO>> GetByDoctorIdAsync(int doctorId);
}
