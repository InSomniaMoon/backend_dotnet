using GestionMateriel.Application.DTOs.Common;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Features.Items.Queries;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Queries.Items;

public class GetAvailableItemsForDatesQueryHandler(
    GestionMaterielDbContext db
) : IRequestHandler<GetAvailableItemsForDatesQuery, PaginatedResponse<ItemWithRestResponse>>
{
    public async Task<PaginatedResponse<ItemWithRestResponse>> Handle(GetAvailableItemsForDatesQuery request, CancellationToken cancellationToken)
    {
        request.Validate();

        var overlappingSubscriptions = db.EventSubscriptions
            .AsNoTracking()
            .Where(es => es.Event.StartDate < request.EndDate)
            .Where(es => es.Event.EndDate > request.StartDate)
            .Where(es => request.ForEventId == null || es.EventId != request.ForEventId);

        // Quantité déjà consommée par item, pour les catégories non identifiées (le matériel non
        // identifié est mutualisé entre événements via le stock, contrairement au matériel identifié).
        var usedQuantities = overlappingSubscriptions
            .Where(es => !es.Item.Category.Identified)
            .GroupBy(es => es.ItemId)
            .Select(g => new { ItemId = g.Key, UsedQuantity = g.Sum(es => es.Quantity) });

        var query = db.Items
            .AsNoTracking()
            .Where(i => string.IsNullOrEmpty(request.Q) ||
                        EF.Functions.ILike(i.Name, $"%{request.Q}%") ||
                        EF.Functions.ILike(i.Category.Name, $"%{request.Q}%"))
            .Where(i => request.CategoryId == null || i.CategoryId == request.CategoryId)
            // Un item de catégorie identifiée est disponible s'il n'est inscrit à aucun événement
            // dont les dates chevauchent la période demandée ; un item non identifié reste
            // disponible tant qu'il reste du stock libre sur la période.
            .Where(i => !i.Category.Identified ||
                        !overlappingSubscriptions.Any(es => es.ItemId == i.Id))
            .GroupJoin(
                usedQuantities,
                item => item.Id,
                used => used.ItemId,
                (item, used) => new
                {
                    Item = item,
                    UsedQuantity = used.Select(u => u.UsedQuantity).FirstOrDefault()
                }
            )
            // .Where(i => i.UsableStock > 0)
            .Where(x => x.Item.UsableStock - x.UsedQuantity > 0);

        var total = await query.CountAsync(cancellationToken);

        var orderedQuery = request.OrderBy switch
        {
            "categoryId" when request.OrderDir == "desc" => query.OrderByDescending(x => x.Item.CategoryId),
            "categoryId" => query.OrderBy(x => x.Item.CategoryId),
            "items.name" when request.OrderDir == "desc" => query.OrderByDescending(x => x.Item.Name),
            "items.name" => query.OrderBy(x => x.Item.Name),
            _ when request.OrderDir == "desc" => query.OrderByDescending(x => x.Item.Id),
            _ => query.OrderBy(x => x.Item.Id)
        };

        var items = await orderedQuery
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .Select(x => new
            {
                x.Item.Id,
                x.Item.Name,
                x.Item.Description,
                x.Item.CategoryId,
                x.Item.StructureId,
                x.Item.Usable,
                x.Item.Stock,
                x.Item.UsableStock,
                x.Item.State,
                x.Item.DateOfBuy,
                x.Item.Image,
                CategoryResponseId = x.Item.Category.Id,
                CategoryName = x.Item.Category.Name,
                CategoryStructureId = x.Item.Category.StructureId,
                CategoryIdentified = x.Item.Category.Identified,
                x.UsedQuantity
            })
            .ToListAsync(cancellationToken);

        var responses = items.Select(item =>
        {
            return new ItemWithRestResponse
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                CategoryId = item.CategoryId,
                StructureId = item.StructureId,
                Usable = item.Usable,
                Stock = item.Stock,
                Rest = item.UsableStock - item.UsedQuantity,
                UsableStock = item.UsableStock,
                State = item.State.ToString(),
                DateOfBuy = item.DateOfBuy,
                Image = item.Image,
                Category = new ItemCategoryResponse
                {
                    Id = item.CategoryResponseId,
                    Name = item.CategoryName,
                    StructureId = item.CategoryStructureId,
                    Identified = item.CategoryIdentified
                }
            };
        }).ToList();

        return new PaginatedResponse<ItemWithRestResponse>
        {
            Data = responses,
            TotalCount = total,
            Page = request.Page,
            Size = request.Size
        };
    }
}
