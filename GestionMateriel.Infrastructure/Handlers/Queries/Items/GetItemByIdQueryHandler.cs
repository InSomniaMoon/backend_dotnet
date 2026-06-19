using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Application.Features.Items.Queries;
using GestionMateriel.Infrastructure.Data;

namespace GestionMateriel.Infrastructure.Handlers.Queries.Items;

public class GetItemByIdQueryHandler(GestionMaterielDbContext db)
    : IRequestHandler<GetItemByIdQuery, ItemResponse?>
{
    public async Task<ItemResponse?> Handle(GetItemByIdQuery query, CancellationToken cancellationToken)
    {
        var item = await db.Items.FindAsync([query.Id], cancellationToken);
        return item is null
            ? null
            : new ItemResponse
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Image = item.Image,
                CategoryId = item.CategoryId,
                Category = new ItemCategoryResponse()
                {
                    Id = item.Category.Id,
                    Name = item.Category.Name
                }
            };
    }
}
