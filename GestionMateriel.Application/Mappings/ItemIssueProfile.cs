using AutoMapper;
using GestionMateriel.Application.DTOs.Requests;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Domain.Entities;

namespace GestionMateriel.Application.Mappings;

public class ItemIssueProfile : Profile
{
    public ItemIssueProfile()
    {
        CreateMap<ItemIssue, ItemIssueResponse>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<CreateItemIssueRequest, ItemIssue>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => Domain.Enums.IssueStatusEnum.Open))
            .ForMember(dest => dest.ResolutionDate, opt => opt.Ignore())
            .ForMember(dest => dest.Item, opt => opt.Ignore())
            .ForMember(dest => dest.ReportedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Comments, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

        CreateMap<ItemIssueComment, ItemIssueCommentResponse>();

        CreateMap<CreateItemIssueCommentRequest, ItemIssueComment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ItemIssueId, opt => opt.Ignore())
            .ForMember(dest => dest.ItemIssue, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
    }
}
