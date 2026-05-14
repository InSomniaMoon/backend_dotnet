using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Queries;

public class GetOpenItemIssuesQueryHandler : IRequestHandler<GetOpenItemIssuesQuery, IEnumerable<ItemIssueResponse>>
{
    private readonly IItemIssueRepository _itemIssueRepository;
    private readonly IMapper _mapper;

    public GetOpenItemIssuesQueryHandler(IItemIssueRepository itemIssueRepository, IMapper mapper)
    {
        _itemIssueRepository = itemIssueRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ItemIssueResponse>> Handle(GetOpenItemIssuesQuery query, CancellationToken cancellationToken)
    {
        var issues = await _itemIssueRepository.GetOpenIssuesAsync();
        return issues.Select(i => _mapper.Map<ItemIssueResponse>(i));
    }
}
