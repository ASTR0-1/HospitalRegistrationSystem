using AutoMapper;
using HospitalRegistrationSystem.Application.DTOs;
using HospitalRegistrationSystem.Application.DTOs.AuthenticationDTOs;
using HospitalRegistrationSystem.Domain.Entities;

namespace HospitalRegistrationSystem.Application.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ClientForRegistrationDto, ApplicationUser>();
    }
}