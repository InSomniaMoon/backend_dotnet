using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Queries;

public class GetOpenItemIssuesQueryHandler(IItemIssueRepository itemIssueRepository, IMapper mapper) : IRequestHandler<GetOpenItemIssuesQuery, IEnumerable<ItemIssueResponse>>
{
    public async Task<IEnumerable<ItemIssueResponse>> Handle(GetOpenItemIssuesQuery query, CancellationToken cancellationToken)
    {
        var issues = await itemIssueRepository.GetOpenIssuesAsync();
        return issues.Select(i => mapper.Map<ItemIssueResponse>(i));
    }
}
