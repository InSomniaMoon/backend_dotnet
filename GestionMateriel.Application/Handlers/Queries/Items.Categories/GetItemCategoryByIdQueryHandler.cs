using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Interfaces;
using GestionMateriel.Application.Messaging;

namespace GestionMateriel.Application.Handlers.Queries.Items.Categories;

public class GetItemCategoryByIdQueryHandler(IItemCategoryRepository itemCategoryRepository, IMapper mapper) : IRequestHandler<GetItemCategoryByIdQuery, ItemCategoryResponse?>
{
    public async Task<ItemCategoryResponse?> Handle(GetItemCategoryByIdQuery query, CancellationToken cancellationToken)
    {
        var category = await itemCategoryRepository.GetByIdAsync(query.Id);
        return category is null ? null : mapper.Map<ItemCategoryResponse>(category);
    }
}
