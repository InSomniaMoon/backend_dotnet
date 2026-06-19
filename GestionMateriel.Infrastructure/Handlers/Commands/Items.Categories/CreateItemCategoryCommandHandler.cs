using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Infrastructure.Data;

namespace GestionMateriel.Infrastructure.Handlers.Commands.Items.Categories;

public class CreateItemCategoryCommandHandler(GestionMaterielDbContext db, IMapper mapper)
    : IRequestHandler<CreateItemCategoryCommand, ItemCategoryResponse>
{
    public async Task<ItemCategoryResponse> Handle(CreateItemCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = mapper.Map<ItemCategory>(command.Request);
        category.StructureId = command.StructureId;
        category.CodeStructure = command.CodeStructure;
        await db.ItemCategories.AddAsync(category, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
        return mapper.Map<ItemCategoryResponse>(category);
    }
}
