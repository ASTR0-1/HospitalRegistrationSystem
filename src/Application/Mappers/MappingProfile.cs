using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using HospitalRegistrationSystem.Application.DTOs.ApplicationUserDTOs;
using HospitalRegistrationSystem.Application.DTOs.AppointmentDTOs;
using HospitalRegistrationSystem.Application.DTOs.AuthenticationDTOs;
using HospitalRegistrationSystem.Application.DTOs.DoctorScheduleDTOs;
using HospitalRegistrationSystem.Application.DTOs.FeedbackDTOs;
using HospitalRegistrationSystem.Application.DTOs.HospitalDTOs;
using HospitalRegistrationSystem.Application.DTOs.LocationDTOs;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Entities;

namespace HospitalRegistrationSystem.Application.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap(typeof(PagedList<>), typeof(PagedList<>))
            .ConvertUsing(typeof(PagedListConverter<,>));

        CreateMap<UserForRegistrationDto, ApplicationUser>();

        CreateMap<ApplicationUser, ApplicationUserDto>()
            .ReverseMap();

        CreateMap<AppointmentForCreationDto, Appointment>();

        CreateMap<Appointment, AppointmentDto>();

        CreateMap<Country, CountryDto>()
            .ReverseMap();
        CreateMap<Region, RegionDto>()
            .ReverseMap();
        CreateMap<City, CityDto>()
            .ReverseMap();

        CreateMap<Hospital, HospitalDto>()
            .ForMember(dto => dto.Country, conf => conf.MapFrom(h => h.Address.City!.Region!.Country!.Name))
            .ForMember(dto => dto.Region, conf => conf.MapFrom(h => h.Address.City!.Region!.Name))
            .ForMember(dto => dto.City, conf => conf.MapFrom(h => h.Address.City!.Name))
            .ForMember(dto => dto.Street, conf => conf.MapFrom(h => h.Address.Street));

        CreateMap<HospitalForCreationDto, Hospital>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
            {
                CityId = src.CityId,
                Street = src.Street
            }));

        CreateMap<DoctorSchedule, DoctorScheduleDto>()
            .ForMember(dto => dto.DoctorId, conf => conf.MapFrom(ds => ds.DoctorId))
            .ForMember(dto => dto.WorkingHoursList, conf => conf.MapFrom(ds => DecodeWorkingHours(ds.WorkingHours)));

        CreateMap<DoctorSchedule, DoctorScheduleForManipulationDto>()
            .ForMember(dest => dest.WorkingHoursList, opt => 
                opt.MapFrom(src => DecodeWorkingHours(src.WorkingHours)));

        CreateMap<DoctorScheduleForManipulationDto, DoctorSchedule>()
            .ForMember(dest => dest.WorkingHours, opt => 
                opt.MapFrom(src => EncodeWorkingHours(src.WorkingHoursList)));

        CreateMap<Feedback, FeedbackDto>();

        CreateMap<FeedbackForCreationDto, Feedback>();
    }

    private List<int> DecodeWorkingHours(int workingHours)
    {
        var hours = new List<int>();

        for (var hour = 0; hour < 24; hour++)
        {
            if ((workingHours & (1 << hour)) != 0)
                hours.Add(hour);
        }

        return hours;
    }

    private int EncodeWorkingHours(IEnumerable<int> hours) => hours.Aggregate(0, (current, hour) => current | (1 << hour));
}