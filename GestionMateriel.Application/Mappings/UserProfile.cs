using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Domain.Entities;

namespace GestionMateriel.Application.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserResponse>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

        CreateMap<User, UserWithStructuresResponse>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))
            .ForMember(dest => dest.Structures, opt => opt.MapFrom(src => src.UserStructures));

        CreateMap<UserStructure, StructureMemberResponse>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
    }
}
