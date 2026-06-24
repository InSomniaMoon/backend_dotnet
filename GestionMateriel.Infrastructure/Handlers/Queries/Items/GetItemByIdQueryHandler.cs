using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Features.Items.Queries;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Queries.Items;

public class GetItemByIdQueryHandler(GestionMaterielDbContext db)
    : IRequestHandler<GetItemByIdQuery, ItemResponse?>
{
    public async Task<ItemResponse?> Handle(GetItemByIdQuery query, CancellationToken cancellationToken)
    {
        return await db.Items.AsNoTracking().Select(item => new ItemResponse
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Image = item.Image,
            CategoryId = item.CategoryId,
            DateOfBuy = item.DateOfBuy,
            Stock = item.Stock,
            Usable = item.Usable,
            State = item.State.Value,
            StructureId = item.StructureId,
            UsableStock = item.UsableStock,

            Category = new ItemCategoryResponse()
            {
                Id = item.Category.Id,
                Name = item.Category.Name
            }
        }).FirstOrDefaultAsync(i => i.Id == query.Id, cancellationToken);
    }
}
