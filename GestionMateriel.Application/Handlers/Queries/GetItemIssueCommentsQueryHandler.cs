using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Queries;

public class GetItemIssueCommentsQueryHandler(IItemIssueCommentRepository itemIssueCommentRepository, IMapper mapper) : IRequestHandler<GetItemIssueCommentsQuery, IEnumerable<ItemIssueCommentResponse>>
{
    public async Task<IEnumerable<ItemIssueCommentResponse>> Handle(GetItemIssueCommentsQuery query, CancellationToken cancellationToken)
    {
        var comments = await itemIssueCommentRepository.GetByIssueAsync(query.ItemIssueId);
        return comments.Select(c => mapper.Map<ItemIssueCommentResponse>(c));
    }
}
