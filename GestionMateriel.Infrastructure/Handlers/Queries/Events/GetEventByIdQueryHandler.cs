using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Features.Events.Queries;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Queries.Events;

public class GetEventByIdQueryHandler(GestionMaterielDbContext db)
    : IRequestHandler<GetEventByIdQuery, EventResponse?>
{
    public async Task<EventResponse?> Handle(GetEventByIdQuery query, CancellationToken cancellationToken)
    {
        var entity = await db.Events.AsNoTracking().Select(entity => new EventWithItemsResponse
        {
            Id = entity.Id,
            Name = entity.Name,
            Comment = entity.Comment,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            StructureId = entity.StructureId,
            CreatedById = entity.UserId,
            CreatedBy = entity.CreatedBy == null ? null : new UserResponse
            {
                Id = entity.CreatedBy.Id,
                LastName = entity.CreatedBy.LastName,
                FirstName = entity.CreatedBy.FirstName,
            },
            Structure = new StructureResponse()
            {
                Id = entity.Structure.Id,
                Name = entity.Structure.Name,
                Color = entity.Structure.Color,
            },
            Items = entity.Subscriptions.Select(es => new ItemWithQuantityResponse
            {
                Id = es.Item.Id,
                Name = es.Item.Name,
                Description = es.Item.Description,
                CategoryId = es.Item.CategoryId,
                StructureId = es.Item.StructureId,
                Usable = es.Item.Usable,
                Stock = es.Item.Stock,
                UsableStock = es.Item.UsableStock,
                State = es.Item.State.ToString(),
                DateOfBuy = es.Item.DateOfBuy,
                Image = es.Item.Image,
                Category = new ItemCategoryResponse
                {
                    Id = es.Item.Category.Id,
                    Name = es.Item.Category.Name,
                    Identified = es.Item.Category.Identified
                },
                Quantity = es.Quantity
            })
        }).FirstOrDefaultAsync(e => e.Id == query.Id, cancellationToken);
        return entity;
    }
}
