using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Application.Queries;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Queries.Items.Categories;

public class GetItemCategoriesQueryHandler(GestionMaterielDbContext db, IMapper mapper)
    : IRequestHandler<GetItemCategoriesQuery, IEnumerable<ItemCategoryResponse>>
{
    public async Task<IEnumerable<ItemCategoryResponse>> Handle(GetItemCategoriesQuery query, CancellationToken cancellationToken)
    {
        var categories = await db.ItemCategories
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
        return categories.Select(mapper.Map<ItemCategoryResponse>);
    }
}
