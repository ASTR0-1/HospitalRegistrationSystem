using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HospitalRegistrationSystem.Application.DTOs;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Application.Interfaces.Services;
using HospitalRegistrationSystem.Domain.Entities;

namespace HospitalRegistrationSystem.Application.Services;

public class DoctorService : IDoctorService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public DoctorService(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task AddNewAsync(Doctor doctor)
    {
        _repository.Doctor.CreateDoctor(doctor);
        await _repository.SaveAsync();
    }

    public async Task<IEnumerable<DoctorCardDTO>> FindAsync(string searchString)
    {
        IEnumerable<Doctor> doctors = await _repository.Doctor.GetDoctorsAsync(trackChanges: false);

        IEnumerable<Doctor> filteredDoctors = doctors
            .Where(c => string.Join(" ", c.FirstName, c.MiddleName, c.LastName)
            .Contains(searchString, System.StringComparison.InvariantCultureIgnoreCase));

        var doctorsDto = _mapper.Map<IEnumerable<DoctorCardDTO>>(filteredDoctors);

        return doctorsDto;
    }

    public async Task<IEnumerable<DoctorCardDTO>> GetAllAsync()
    {
        IEnumerable<Doctor> doctors = await _repository.Doctor.GetDoctorsAsync(trackChanges: false);

        var doctorsDto = _mapper.Map<IEnumerable<DoctorCardDTO>>(doctors);

        return doctorsDto;
    }

    public async Task<DoctorCardDTO> GetAsync(int doctorId)
    {
        Doctor doctor = await _repository.Doctor.GetDoctorAsync(doctorId, trackChanges: false);

        var doctorDto = _mapper.Map<DoctorCardDTO>(doctor);

        return doctorDto;
    }
}
