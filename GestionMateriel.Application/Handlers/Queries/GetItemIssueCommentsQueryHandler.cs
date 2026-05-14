using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Queries;

public class GetItemIssueCommentsQueryHandler : IRequestHandler<GetItemIssueCommentsQuery, IEnumerable<ItemIssueCommentResponse>>
{
    private readonly IItemIssueCommentRepository _itemIssueCommentRepository;
    private readonly IMapper _mapper;

    public GetItemIssueCommentsQueryHandler(IItemIssueCommentRepository itemIssueCommentRepository, IMapper mapper)
    {
        _itemIssueCommentRepository = itemIssueCommentRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ItemIssueCommentResponse>> Handle(GetItemIssueCommentsQuery query, CancellationToken cancellationToken)
    {
        var comments = await _itemIssueCommentRepository.GetByIssueAsync(query.ItemIssueId);
        return comments.Select(c => _mapper.Map<ItemIssueCommentResponse>(c));
    }
}
