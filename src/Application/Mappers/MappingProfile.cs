using AutoMapper;
using HospitalRegistrationSystem.Application.Interfaces.DTOs;
using HospitalRegistrationSystem.Domain.Entities;

namespace HospitalRegistrationSystem.Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Appointment, ClientAppointmentDTO>()
                    .ForPath(ca => ca.DoctorId,
                        opt => opt.MapFrom(a => a.DoctorId))
                    .ForPath(ca => ca.DoctorFirstName,
                        opt => opt.MapFrom(a => a.Doctor.FirstName))
                    .ForPath(ca => ca.DoctorMiddleName,
                        opt => opt.MapFrom(a => a.Doctor.MiddleName))
                    .ForPath(ca => ca.DoctorLastName,
                        opt => opt.MapFrom(a => a.Doctor.LastName))
                    .ForPath(ca => ca.DoctorSpecialty,
                        opt => opt.MapFrom(a => a.Doctor.Specialty))
                    .ForPath(ca => ca.DoctorGender,
                        opt => opt.MapFrom(a => a.Doctor.Gender));

            CreateMap<Appointment, ClientAppointmentCardDTO>()
                    .ForPath(ca => ca.DoctorId,
                        opt => opt.MapFrom(a => a.DoctorId))
                    .ForPath(ca => ca.DoctorFirstName,
                        opt => opt.MapFrom(a => a.Doctor.FirstName))
                    .ForPath(ca => ca.DoctorMiddleName,
                        opt => opt.MapFrom(a => a.Doctor.MiddleName))
                    .ForPath(ca => ca.DoctorLastName,
                        opt => opt.MapFrom(a => a.Doctor.LastName))
                    .ForPath(ca => ca.DoctorSpecialty,
                        opt => opt.MapFrom(a => a.Doctor.Specialty))
                    .ForPath(ca => ca.DoctorGender,
                        opt => opt.MapFrom(a => a.Doctor.Gender));

            CreateMap<Appointment, DoctorAppointmentDTO>()
                    .ForPath(da => da.ClientId,
                        opt => opt.MapFrom(a => a.ClientId))
                    .ForPath(da => da.ClientFirstName,
                        opt => opt.MapFrom(a => a.Client.FirstName))
                    .ForPath(da => da.ClientMiddleName,
                        opt => opt.MapFrom(a => a.Client.MiddleName))
                    .ForPath(da => da.ClientLastName,
                        opt => opt.MapFrom(a => a.Client.LastName))
                    .ForPath(da => da.ClientGender,
                        opt => opt.MapFrom(a => a.Client.Gender));

            CreateMap<Appointment, AppointmentForCreationDTO>()
                .ReverseMap();

            CreateMap<Client, ClientCardDTO>()
                .ReverseMap();

            CreateMap<Client, ClientForCreationDTO>()
                .ReverseMap();

            CreateMap<Doctor, DoctorCardDTO>()
                .ReverseMap();

            CreateMap<Doctor, DoctorForCreationDTO>()
                .ReverseMap();
        }
    }
}
