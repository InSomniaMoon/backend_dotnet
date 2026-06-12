using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Application.Queries;
using GestionMateriel.Infrastructure.Data;

namespace GestionMateriel.Infrastructure.Handlers.Queries.Items.Categories;

public class GetItemCategoryByIdQueryHandler(GestionMaterielDbContext db, IMapper mapper)
    : IRequestHandler<GetItemCategoryByIdQuery, ItemCategoryResponse?>
{
    public async Task<ItemCategoryResponse?> Handle(GetItemCategoryByIdQuery query, CancellationToken cancellationToken)
    {
        var category = await db.ItemCategories.FindAsync([query.Id], cancellationToken);
        return category is null ? null : mapper.Map<ItemCategoryResponse>(category);
    }
}
