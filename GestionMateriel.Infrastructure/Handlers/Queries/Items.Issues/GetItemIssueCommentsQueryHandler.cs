using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Application.Queries;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Queries.Items.Issues;

public class GetItemIssueCommentsQueryHandler(GestionMaterielDbContext db, IMapper mapper)
    : IRequestHandler<GetItemIssueCommentsQuery, IEnumerable<ItemIssueCommentResponse>>
{
    public async Task<IEnumerable<ItemIssueCommentResponse>> Handle(GetItemIssueCommentsQuery query, CancellationToken cancellationToken)
    {
        var comments = await db.ItemIssueComments
            .AsNoTracking()
            .Where(c => c.ItemIssueId == query.ItemIssueId)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
        return comments.Select(mapper.Map<ItemIssueCommentResponse>);
    }
}
