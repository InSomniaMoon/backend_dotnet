using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Queries;

public class GetItemCategoryByIdQueryHandler : IRequestHandler<GetItemCategoryByIdQuery, ItemCategoryResponse?>
{
    private readonly IItemCategoryRepository _itemCategoryRepository;
    private readonly IMapper _mapper;

    public GetItemCategoryByIdQueryHandler(IItemCategoryRepository itemCategoryRepository, IMapper mapper)
    {
        _itemCategoryRepository = itemCategoryRepository;
        _mapper = mapper;
    }

    public async Task<ItemCategoryResponse?> Handle(GetItemCategoryByIdQuery query, CancellationToken cancellationToken)
    {
        var category = await _itemCategoryRepository.GetByIdAsync(query.Id);
        return category is null ? null : _mapper.Map<ItemCategoryResponse>(category);
    }
}
