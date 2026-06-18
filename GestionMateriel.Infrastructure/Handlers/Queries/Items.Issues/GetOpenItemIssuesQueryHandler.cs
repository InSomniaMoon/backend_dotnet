using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Features.ItemIssues.Queries;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Queries.Items.Issues;

public class GetOpenItemIssuesQueryHandler(GestionMaterielDbContext db, IMapper mapper)
    : IRequestHandler<GetOpenItemIssuesQuery, IEnumerable<ItemIssueResponse>>
{
    public async Task<IEnumerable<ItemIssueResponse>> Handle(GetOpenItemIssuesQuery query,
        CancellationToken cancellationToken)
    {
        var issues = await db.ItemIssues
            .AsNoTracking()
            .Where(ii => ii.Status == IssueStatusEnum.Open)
            .OrderByDescending(ii => ii.CreatedAt)
            .ToListAsync(cancellationToken);
        return issues.Select(mapper.Map<ItemIssueResponse>);
    }
}
