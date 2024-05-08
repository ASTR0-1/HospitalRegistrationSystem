using AutoMapper;
using HospitalRegistrationSystem.Application.DTOs.ApplicationUserDTOs;
using HospitalRegistrationSystem.Application.DTOs.AuthenticationDTOs;
using HospitalRegistrationSystem.Application.DTOs.LocationDTOs;
using HospitalRegistrationSystem.Domain.Entities;

namespace HospitalRegistrationSystem.Application.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserForRegistrationDto, ApplicationUser>();

        CreateMap<ApplicationUser, ApplicationUserDto>()
            .ReverseMap();

        CreateMap<Country, CountryDto>();
        CreateMap<Region, RegionDto>();
        CreateMap<City, CityDto>();
    }
}