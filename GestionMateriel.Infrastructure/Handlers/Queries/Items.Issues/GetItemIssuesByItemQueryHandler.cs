using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Application.Queries;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Queries.Items.Issues;

public class GetItemIssuesByItemQueryHandler(GestionMaterielDbContext db, IMapper mapper)
    : IRequestHandler<GetItemIssuesByItemQuery, IEnumerable<ItemIssueResponse>>
{
    public async Task<IEnumerable<ItemIssueResponse>> Handle(GetItemIssuesByItemQuery query, CancellationToken cancellationToken)
    {
        var issues = await db.ItemIssues
            .AsNoTracking()
            .Where(ii => ii.ItemId == query.ItemId)
            .OrderByDescending(ii => ii.CreatedAt)
            .ToListAsync(cancellationToken);
        return issues.Select(mapper.Map<ItemIssueResponse>);
    }
}
