using AutoMapper;
using HospitalRegistrationSystem.Application.DTOs.ApplicationUserDTOs;
using HospitalRegistrationSystem.Application.DTOs.AuthenticationDTOs;
using HospitalRegistrationSystem.Application.DTOs.HospitalDTOs;
using HospitalRegistrationSystem.Application.DTOs.LocationDTOs;
using HospitalRegistrationSystem.Application.Utility;
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

        CreateMap(typeof(PagedList<>), typeof(PagedList<>))
            .ConvertUsing(typeof(PagedListConverter<,>));

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
    }
}