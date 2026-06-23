using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Features.Items.Commands;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Infrastructure.Data;

namespace GestionMateriel.Infrastructure.Handlers.Commands.Items;

public class CreateItemCommandHandler(GestionMaterielDbContext db, IMapper mapper)
    : IRequestHandler<CreateItemCommand, ItemResponse>
{
    public async Task<ItemResponse> Handle(CreateItemCommand command, CancellationToken cancellationToken)
    {
        var item = mapper.Map<Domain.Entities.Item>(command.Request);
        item.UsableStock = item.Stock;
        item.CreatedAt = DateTime.UtcNow;
        item.UpdatedAt = DateTime.UtcNow;
        item.Description = string.IsNullOrWhiteSpace(command.Request.Description) ? null : command.Request.Description;


        await db.Items.AddAsync(item, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
        return mapper.Map<ItemResponse>(item);
    }
}
