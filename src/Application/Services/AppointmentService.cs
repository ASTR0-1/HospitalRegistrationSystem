using System;
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

    public async Task<ClientAppointmentDTO> GetAsync(Guid appointmentId)
    {
        var appointment = await _repository.Appointment.GetAppointmentAsync(appointmentId, false);

        var appointmentDto = _mapper.Map<ClientAppointmentDTO>(appointment);

        return appointmentDto;
    }

    public async Task<IEnumerable<ClientAppointmentDTO>> GetByClientIdAsync(int clientId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<DoctorAppointmentDTO>> GetByDoctorIdAsync(int doctorId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ClientAppointmentCardDTO>> GetVisitedByClientIdAsync(int clientId)
    {
        throw new NotImplementedException();
    }

    public async Task<ClientAppointmentCardDTO> MarkAsVisitedAsync(Guid appointmentId, string diagnosis)
    {
        throw new NotImplementedException();
    }
}