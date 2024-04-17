using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HospitalRegistrationSystem.Application.DTOs.AppointmentDTOs;
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

    public async Task<IEnumerable<ClientAppointmentDto>> GetAllAsync()
    {
        var appointments = await _repository.Appointment.GetAppointmentsAsync(false);

        var appointmentsDto = _mapper.Map<IEnumerable<ClientAppointmentDto>>(appointments);

        return appointmentsDto;
    }

    public async Task<ClientAppointmentDto> GetAsync(int appointmentId)
    {
        var appointment = await _repository.Appointment.GetAppointmentAsync(appointmentId, false);

        var appointmentDto = _mapper.Map<ClientAppointmentDto>(appointment);

        return appointmentDto;
    }

    public async Task<IEnumerable<ClientAppointmentDto>> GetByClientIdAsync(int clientId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<DoctorAppointmentDto>> GetByDoctorIdAsync(int doctorId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ClientAppointmentCardDto>> GetVisitedByClientIdAsync(int clientId)
    {
        throw new NotImplementedException();
    }

    public async Task<ClientAppointmentCardDto> MarkAsVisitedAsync(int appointmentId, string diagnosis)
    {
        throw new NotImplementedException();
    }
}