using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Commands;

public class CreateItemCategoryCommandHandler : IRequestHandler<CreateItemCategoryCommand, ItemCategoryResponse>
{
    private readonly IItemCategoryRepository _itemCategoryRepository;
    private readonly IMapper _mapper;

    public CreateItemCategoryCommandHandler(IItemCategoryRepository itemCategoryRepository, IMapper mapper)
    {
        _itemCategoryRepository = itemCategoryRepository;
        _mapper = mapper;
    }

    public async Task<ItemCategoryResponse> Handle(CreateItemCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = _mapper.Map<ItemCategory>(command.Request);

        await _itemCategoryRepository.AddAsync(category);
        await _itemCategoryRepository.SaveChangesAsync();

        return _mapper.Map<ItemCategoryResponse>(category);
    }
}
