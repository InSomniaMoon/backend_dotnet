using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Features.ItemIssues.Queries;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Queries.Items.Issues;

public class GetOpenItemIssuesQueryHandler(GestionMaterielDbContext db)
    : IRequestHandler<GetOpenItemIssuesQuery, IEnumerable<ItemIssueResponse>>
{
    public async Task<IEnumerable<ItemIssueResponse>> Handle(GetOpenItemIssuesQuery query,
        CancellationToken cancellationToken)
    {
        var issues = await db.ItemIssues
            .AsNoTracking()
            .Where(ii => ii.Status == IssueStatusEnum.Open)
            .OrderByDescending(ii => ii.CreatedAt)
            .Select(ii => new ItemIssueResponse()
            {
                AffectedQuantity = ii.AffectedQuantity,
                CreatedAt = ii.CreatedAt,
                Id = ii.Id,
                ItemId = ii.ItemId,
                ReportedById = ii.ReportedBy,
                ResolutionDate = ii.ResolutionDate,
                Status = ii.Status.ToString(),
                Value = ii.Value,
                Item = new ItemResponse()
                {
                    Id = ii.Item.Id,
                    Image = ii.Item.Image,
                    Stock = ii.Item.Stock,
                    Name = ii.Item.Name,
                    State = ii.Item.State.ToString(),
                    Usable = ii.Item.Usable,
                    CategoryId = ii.Item.CategoryId,
                    Category = new ItemCategoryResponse()
                    {
                        Id = ii.Item.Category.Id,
                        Name = ii.Item.Category.Name
                    },
                }

            })
            .ToListAsync(cancellationToken);
        return issues;
    }
}
