using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Infrastructure.Data;

namespace GestionMateriel.Infrastructure.Handlers.Commands.Items;

public class UpdateItemCommandHandler(GestionMaterielDbContext db, IMapper mapper)
    : IRequestHandler<UpdateItemCommand, ItemResponse?>
{
    public async Task<ItemResponse?> Handle(UpdateItemCommand command, CancellationToken cancellationToken)
    {
        var item = await db.Items.FindAsync([command.Id], cancellationToken);
        if (item is null) return null;

        mapper.Map(command.Request, item);
        item.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync(cancellationToken);
        return mapper.Map<ItemResponse>(item);
    }
}
