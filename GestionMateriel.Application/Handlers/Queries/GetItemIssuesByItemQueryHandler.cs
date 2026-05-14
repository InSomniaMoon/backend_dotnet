using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Queries;

public class GetItemIssuesByItemQueryHandler : IRequestHandler<GetItemIssuesByItemQuery, IEnumerable<ItemIssueResponse>>
{
    private readonly IItemIssueRepository _itemIssueRepository;
    private readonly IMapper _mapper;

    public GetItemIssuesByItemQueryHandler(IItemIssueRepository itemIssueRepository, IMapper mapper)
    {
        _itemIssueRepository = itemIssueRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ItemIssueResponse>> Handle(GetItemIssuesByItemQuery query, CancellationToken cancellationToken)
    {
        var issues = await _itemIssueRepository.GetByItemAsync(query.ItemId);
        return issues.Select(i => _mapper.Map<ItemIssueResponse>(i));
    }
}
