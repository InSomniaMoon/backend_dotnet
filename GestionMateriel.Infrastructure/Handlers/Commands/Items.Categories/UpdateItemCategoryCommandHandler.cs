using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Infrastructure.Data;

namespace GestionMateriel.Infrastructure.Handlers.Commands.Items.Categories;

public class UpdateItemCategoryCommandHandler(GestionMaterielDbContext db, IMapper mapper)
    : IRequestHandler<UpdateItemCategoryCommand, ItemCategoryResponse?>
{
    public async Task<ItemCategoryResponse?> Handle(UpdateItemCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await db.ItemCategories.FindAsync([command.Id], cancellationToken);
        if (category is null) return null;

        mapper.Map(command.Request, category);
        await db.SaveChangesAsync(cancellationToken);
        return mapper.Map<ItemCategoryResponse>(category);
    }
}
