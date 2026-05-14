using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Queries;

public class GetItemCategoriesQueryHandler : IRequestHandler<GetItemCategoriesQuery, IEnumerable<ItemCategoryResponse>>
{
    private readonly IItemCategoryRepository _itemCategoryRepository;
    private readonly IMapper _mapper;

    public GetItemCategoriesQueryHandler(IItemCategoryRepository itemCategoryRepository, IMapper mapper)
    {
        _itemCategoryRepository = itemCategoryRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ItemCategoryResponse>> Handle(GetItemCategoriesQuery query, CancellationToken cancellationToken)
    {
        var categories = await _itemCategoryRepository.GetByStructureAsync(query.StructureId);
        return categories.Select(c => _mapper.Map<ItemCategoryResponse>(c));
    }
}
