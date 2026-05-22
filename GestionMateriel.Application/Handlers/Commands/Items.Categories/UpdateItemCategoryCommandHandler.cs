using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Commands.Items.Categories;

public class UpdateItemCategoryCommandHandler(IItemCategoryRepository itemCategoryRepository, IMapper mapper) : IRequestHandler<UpdateItemCategoryCommand, ItemCategoryResponse?>
{
    public async Task<ItemCategoryResponse?> Handle(UpdateItemCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await itemCategoryRepository.GetByIdAsync(command.Id);
        if (category is null)
        {
            return null;
        }

        mapper.Map(command.Request, category);

        await itemCategoryRepository.UpdateAsync(category);
        await itemCategoryRepository.SaveChangesAsync();

        return mapper.Map<ItemCategoryResponse>(category);
    }
}
