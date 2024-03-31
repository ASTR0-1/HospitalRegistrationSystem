using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HospitalRegistrationSystem.Application.DTOs;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Application.Interfaces.Services;
using HospitalRegistrationSystem.Domain.Entities;

namespace HospitalRegistrationSystem.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _repository;

    public AppointmentService(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task AddNewAsync(Appointment appointment)
    {
        _repository.Appointment.CreateAppointment(appointment);
        await _repository.SaveAsync();
    }

    public async Task<IEnumerable<ClientAppointmentDTO>> GetAllAsync()
    {
        var appointments = await _repository.Appointment.GetAppointmentsAsync(false);

        var appointmentsDto = _mapper.Map<IEnumerable<ClientAppointmentDTO>>(appointments);

        return appointmentsDto;
    }

    public async Task<ClientAppointmentDTO> GetAsync(int appointmentId)
    {
        var appointment = await _repository.Appointment.GetAppointmentAsync(appointmentId, false);

        var appointmentDto = _mapper.Map<ClientAppointmentDTO>(appointment);

        return appointmentDto;
    }

    public async Task<IEnumerable<ClientAppointmentDTO>> GetByClientIdAsync(int clientId)
    {
        var appointments = await _repository.Appointment.GetAppointmentsAsync(false);

        var clientAppointments = appointments
            .Where(a => a.ClientId == clientId && a.IsVisited == false);

        var appointmentsDto = _mapper.Map<IEnumerable<ClientAppointmentDTO>>(clientAppointments);

        return appointmentsDto;
    }

    public async Task<IEnumerable<DoctorAppointmentDTO>> GetByDoctorIdAsync(int doctorId)
    {
        var appointments = await _repository.Appointment.GetAppointmentsAsync(false);

        var doctorAppointments = appointments
            .Where(a => a.DoctorId == doctorId && a.IsVisited == false);

        var appointmentsDto = _mapper.Map<IEnumerable<DoctorAppointmentDTO>>(doctorAppointments);

        return appointmentsDto;
    }

    public async Task<IEnumerable<ClientAppointmentCardDTO>> GetVisitedByClientIdAsync(int clientId)
    {
        var appointments = await _repository.Appointment.GetAppointmentsAsync(false);

        var visitedClientAppointments = appointments
            .Where(a => a.ClientId == clientId && a.IsVisited);

        var appointmentsDto = _mapper.Map<IEnumerable<ClientAppointmentCardDTO>>(visitedClientAppointments);

        return appointmentsDto;
    }

    public async Task<ClientAppointmentCardDTO> MarkAsVisitedAsync(int appointmentId, string diagnosis)
    {
        var appointment = await _repository.Appointment.GetAppointmentAsync(appointmentId, true);
        if (appointment == null)
            return null;

        appointment.IsVisited = true;
        appointment.Diagnosis = diagnosis;

        await _repository.SaveAsync();

        var appointmentDto = _mapper.Map<ClientAppointmentCardDTO>(appointment);
        return appointmentDto;
    }
}