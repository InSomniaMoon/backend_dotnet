using AutoMapper;
using GestionMateriel.Application.DTOs.Common;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Application.Features.Items.Queries;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Queries.Items;

public class GetItemsQueryHandler(GestionMaterielDbContext db, IMapper mapper)
    : IRequestHandler<GetItemsQuery, PaginatedResponse<ItemResponse>>
{
    public async Task<PaginatedResponse<ItemResponse>> Handle(GetItemsQuery request,
        CancellationToken cancellationToken)
    {
        var query = db.Items
            .Include(i => i.Category)
            .Include(i => i.Issues.Where(ii => ii.Status == IssueStatusEnum.Open))
            .Where(i => string.IsNullOrEmpty(request.Q) ||
                        i.Name.Contains(request.Q, StringComparison.OrdinalIgnoreCase));

        var total = await query.CountAsync(cancellationToken);

        query = request.OrderBy switch
        {
            "name" when request.OrderDir == "desc" => query.OrderByDescending(i => i.Name),
            "name" => query.OrderBy(i => i.Name),
            "categoryId" when request.OrderDir == "desc" => query.OrderByDescending(i => i.CategoryId),
            "categoryId" => query.OrderBy(i => i.CategoryId),
            "state" when request.OrderDir == "desc" => query.OrderByDescending(i => i.State),
            "state" => query.OrderBy(i => i.State),
            "id" when request.OrderDir == "desc" => query.OrderByDescending(i => i.Id),
            "id" => query.OrderBy(i => i.Id),
            _ when request.OrderDir == "desc" => query.OrderByDescending(i => i.Id),
            _ => query.OrderBy(i => i.Id)
        };

        var items = await query
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .ToListAsync(cancellationToken);

        return new PaginatedResponse<ItemResponse>
        {
            Data = items.Select(mapper.Map<ItemResponse>),
            TotalCount = total,
            Page = request.Page,
            Size = request.Size
        };
    }
}
