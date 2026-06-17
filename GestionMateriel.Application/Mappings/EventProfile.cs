using AutoMapper;
using GestionMateriel.Application.DTOs.Requests.Events;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Domain.Entities;

namespace GestionMateriel.Application.Mappings;

public class EventProfile : Profile
{
    public EventProfile()
    {
        CreateMap<Event, EventResponse>();

        CreateMap<CreateEventRequest, Event>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Structure, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Subscriptions, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

        CreateMap<UpdateEventRequest, Event>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Structure, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Subscriptions, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        CreateMap<EventSubscription, EventSubscriptionResponse>();
    }
}
