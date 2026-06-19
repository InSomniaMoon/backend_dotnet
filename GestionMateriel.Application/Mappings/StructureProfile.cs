using AutoMapper;
using GestionMateriel.Application.DTOs.Requests.Structures;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Enums;

namespace GestionMateriel.Application.Mappings;

public class StructureProfile : Profile
{
    public StructureProfile()
    {
        CreateMap<Structure, StructureResponse>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));

        CreateMap<UserStructure, StructureMemberResponse>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

        CreateMap<CreateStructureRequest, Structure>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => StructureTypeEnum.FromString(src.Type)))
            .ForMember(dest => dest.UserStructures, opt => opt.Ignore())
            .ForMember(dest => dest.Items, opt => opt.Ignore())
            .ForMember(dest => dest.ItemCategories, opt => opt.Ignore())
            .ForMember(dest => dest.Events, opt => opt.Ignore());



        CreateMap<UpdateStructureRequest, Structure>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CodeStructure, opt => opt.Ignore())
            .ForMember(dest => dest.UserStructures, opt => opt.Ignore())
            .ForMember(dest => dest.Items, opt => opt.Ignore())
            .ForMember(dest => dest.ItemCategories, opt => opt.Ignore())
            .ForMember(dest => dest.Events, opt => opt.Ignore());


    }
}
