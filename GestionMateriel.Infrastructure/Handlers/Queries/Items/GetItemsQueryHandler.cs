using GestionMateriel.Application.DTOs.Common;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Features.Items.Queries;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Queries.Items;

public class GetItemsQueryHandler(GestionMaterielDbContext db)
    : IRequestHandler<GetItemsQuery, PaginatedResponse<ItemResponse>>
{
    public async Task<PaginatedResponse<ItemResponse>> Handle(GetItemsQuery request,
        CancellationToken cancellationToken)
    {
        var query = db.Items
                .AsNoTracking()
                .Include(i => i.Category)
                .Include(i => i.Issues.Where(ii => ii.Status == IssueStatusEnum.Open))
                .Where(i => string.IsNullOrEmpty(request.Q)
                || EF.Functions.ILike(i.Name, $"%{request.Q}%")
                || EF.Functions.ILike(i.Category.Name, $"%{request.Q}%"))
            ;

        if (request.CategoryId.HasValue)
        {
            query = query.Where(i => i.CategoryId == request.CategoryId.Value);
        }

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
        .Select(i => new ItemResponse
        {
            Id = i.Id,
            Name = i.Name,
            DateOfBuy = i.DateOfBuy,
            State = i.State.ToString(),
            Description = i.Description,
            Image = i.Image,
            Stock = i.Stock,
            Usable = i.Usable,
            UsableStock = i.UsableStock,
            StructureId = i.StructureId,
            Category = new ItemCategoryResponse
            {
                Id = i.Category.Id,
                Name = i.Category.Name,
                Identified = i.Category.Identified
            },


        })
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .ToListAsync(cancellationToken);

        return new PaginatedResponse<ItemResponse>
        {
            Data = items,
            TotalCount = total,
            Page = request.Page,
            Size = request.Size
        };
    }
}
