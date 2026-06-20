using AutoMapper;
using GestionMateriel.Application.DTOs.Requests.Items;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Domain.Entities;

namespace GestionMateriel.Application.Mappings;

public class ItemProfile : Profile
{
    public ItemProfile()
    {
        CreateMap<Item, ItemResponse>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image));

        CreateMap<CreateItemRequest, Item>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CodeStructure, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.Structure, opt => opt.Ignore())
            .ForMember(dest => dest.Issues, opt => opt.Ignore())
            .ForMember(dest => dest.EventSubscriptions, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
            .ForMember(dest => dest.UsableStock, opt => opt.MapFrom(src => src.Stock));

        CreateMap<UpdateItemRequest, Item>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CodeStructure, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.Structure, opt => opt.Ignore())
            .ForMember(dest => dest.Issues, opt => opt.Ignore())
            .ForMember(dest => dest.EventSubscriptions, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image));
    }
}
