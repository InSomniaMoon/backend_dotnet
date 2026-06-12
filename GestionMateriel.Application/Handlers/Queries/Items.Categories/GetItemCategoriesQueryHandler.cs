using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Interfaces;
using GestionMateriel.Application.Messaging;

namespace GestionMateriel.Application.Handlers.Queries.Items.Categories;

public class GetItemCategoriesQueryHandler(IItemCategoryRepository itemCategoryRepository, IMapper mapper) : IRequestHandler<GetItemCategoriesQuery, IEnumerable<ItemCategoryResponse>>
{
    public async Task<IEnumerable<ItemCategoryResponse>> Handle(GetItemCategoriesQuery query, CancellationToken cancellationToken)
    {
        var categories = await itemCategoryRepository.GetByStructureAsync();
        return categories.Select(c => mapper.Map<ItemCategoryResponse>(c));
    }
}
