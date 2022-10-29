using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Application.Interfaces.DTOs;
using HospitalRegistrationSystem.Application.Interfaces.Services;
using HospitalRegistrationSystem.Domain.Entities;

namespace HospitalRegistrationSystem.Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

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
            IEnumerable<Appointment> appointments = await _repository.Appointment.GetAppointmentsAsync(trackChanges: false);

            var appointmentsDto = _mapper.Map<IEnumerable<ClientAppointmentDTO>>(appointments);

            return appointmentsDto;
        }

        public async Task<IEnumerable<ClientAppointmentDTO>> GetByClientIdAsync(int clientId)
        {
            IEnumerable<Appointment> appointments = await _repository.Appointment.GetAppointmentsAsync(trackChanges: false);

            IEnumerable<Appointment> clientAppointments = appointments
                .Where(a => a.ClientId == clientId);

            var appointmentsDto = _mapper.Map<IEnumerable<ClientAppointmentDTO>>(clientAppointments);

            return appointmentsDto;
        }

        public async Task<IEnumerable<DoctorAppointmentDTO>> GetByDoctorIdAsync(int doctorId)
        {
            IEnumerable<Appointment> appointments = await _repository.Appointment.GetAppointmentsAsync(trackChanges: false);

            IEnumerable<Appointment> doctorAppointments = appointments
                .Where(a => a.DoctorId == doctorId);

            var appointmentsDto = _mapper.Map<IEnumerable<DoctorAppointmentDTO>>(doctorAppointments);

            return appointmentsDto;
        }

        public async Task<IEnumerable<ClientAppointmentCardDTO>> GetVisitedByClientIdAsync(int clientId)
        {
            IEnumerable<Appointment> appointments = await _repository.Appointment.GetAppointmentsAsync(trackChanges: false);

            IEnumerable<Appointment> visitedClientAppointments = appointments
                .Where(a => a.ClientId == clientId && a.IsVisited == true);

            var appointmentsDto = _mapper.Map<IEnumerable<ClientAppointmentCardDTO>>(visitedClientAppointments);

            return appointmentsDto;
        }

        public async Task<ClientAppointmentDTO> MarkAsVisitedAsync(int appointmentId, string diagnosis)
        {
            Appointment appointment = await _repository.Appointment.GetAppointmentAsync(appointmentId, trackChanges: true);
            if (appointment == null)
                return null;

            appointment.IsVisited = true;
            appointment.Diagnosis = diagnosis;

            await _repository.SaveAsync();

            var appointmentDto = _mapper.Map<ClientAppointmentCardDTO>(appointment);
            return appointmentDto;
        }
    }
}
