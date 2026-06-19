using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Features.Categories.Queries;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Queries.Items.Categories;

public class GetItemCategoryByIdQueryHandler(GestionMaterielDbContext db, IMapper mapper)
    : IRequestHandler<GetItemCategoryByIdQuery, ItemCategoryResponse?>
{
    public async Task<ItemCategoryResponse?> Handle(GetItemCategoryByIdQuery query, CancellationToken cancellationToken)
    {
        var category = await db.ItemCategories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == query.Id, cancellationToken);
        return category is null ? null : mapper.Map<ItemCategoryResponse>(category);
    }
}
