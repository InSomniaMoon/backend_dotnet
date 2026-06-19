using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace GestionMateriel.Infrastructure.Handlers.Commands.Items;

public class UpdateItemCommandHandler(
    GestionMaterielDbContext db,
    IMapper mapper,
    ILogger<UpdateItemCommandHandler> logger
)
    : IRequestHandler<UpdateItemCommand, ItemResponse?>
{
    public async Task<ItemResponse?> Handle(UpdateItemCommand command, CancellationToken cancellationToken)
    {
        var item = await db.Items.FindAsync([command.Id], cancellationToken);
        if (item is null) return null;

        item.UpdatedAt = DateTime.UtcNow;
        item.CategoryId = command.Request.CategoryId;
        item.Description = command.Request.Description;
        item.Name = command.Request.Name;
        item.Usable = command.Request.Usable;
        item.DateOfBuy = command.Request.DateOfBuy;
        item.Image = command.Request.Image;
        item.Stock = command.Request.Stock;

        logger.LogInformation("Updated item : {item}", item);


        await db.SaveChangesAsync(cancellationToken);
        return mapper.Map<ItemResponse>(item);
    }
}
