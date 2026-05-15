using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Commands;

public class CreateItemCategoryCommandHandler(IItemCategoryRepository itemCategoryRepository, IMapper mapper) : IRequestHandler<CreateItemCategoryCommand, ItemCategoryResponse>
{
    public async Task<ItemCategoryResponse> Handle(CreateItemCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = mapper.Map<ItemCategory>(command.Request);

        await itemCategoryRepository.AddAsync(category);
        await itemCategoryRepository.SaveChangesAsync();

        return mapper.Map<ItemCategoryResponse>(category);
    }
}
