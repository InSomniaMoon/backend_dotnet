using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Application.Features.Items.Queries;
using GestionMateriel.Infrastructure.Data;

namespace GestionMateriel.Infrastructure.Handlers.Queries.Items;

public class GetItemByIdQueryHandler(GestionMaterielDbContext db, IMapper mapper)
    : IRequestHandler<GetItemByIdQuery, ItemResponse?>
{
    public async Task<ItemResponse?> Handle(GetItemByIdQuery query, CancellationToken cancellationToken)
    {
        var item = await db.Items.FindAsync([query.Id], cancellationToken);
        return item is null ? null : mapper.Map<ItemResponse>(item);
    }
}
