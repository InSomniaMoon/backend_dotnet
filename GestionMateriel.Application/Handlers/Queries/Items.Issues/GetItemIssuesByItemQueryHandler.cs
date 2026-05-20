using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Queries.Items.Issues;

public class GetItemIssuesByItemQueryHandler(IItemIssueRepository itemIssueRepository, IMapper mapper) : IRequestHandler<GetItemIssuesByItemQuery, IEnumerable<ItemIssueResponse>>
{
    public async Task<IEnumerable<ItemIssueResponse>> Handle(GetItemIssuesByItemQuery query, CancellationToken cancellationToken)
    {
        var issues = await itemIssueRepository.GetByItemAsync(query.ItemId);
        return issues.Select(i => mapper.Map<ItemIssueResponse>(i));
    }
}
