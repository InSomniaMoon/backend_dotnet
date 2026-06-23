using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Features.Events.Queries;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Queries.Events;

public class GetActualEventsQueryHandler(GestionMaterielDbContext db)
    : IRequestHandler<GetActualEventsQuery, IEnumerable<EventResponse>>
{
    public async Task<IEnumerable<EventResponse>> Handle(GetActualEventsQuery query,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        return await db.Events
            .AsNoTracking()
            .Where(e => e.EndDate >= now)
            .Where(e => e.StartDate <= now)
            .OrderBy(e => e.StartDate)
            .Select(e => new EventWithItemsResponse
            {
                Id = e.Id,
                Name = e.Name,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                CreatedBy = e.CreatedBy == null ? null : new UserResponse
                {
                    Id = e.CreatedBy.Id,
                    FirstName = e.CreatedBy.FirstName,
                    LastName = e.CreatedBy.LastName,
                    Email = e.CreatedBy.Email
                },
                CreatedById = e.UserId,
                StructureId = e.StructureId,

                Comment = e.Comment,
                Items = e.Subscriptions.Select(i => new ItemWithQuantityResponse
                {
                    Id = i.Item.Id,
                    Name = i.Item.Name,
                    Description = i.Item.Description,
                    CategoryId = i.Item.CategoryId,
                    StructureId = i.Item.StructureId,
                    Usable = i.Item.Usable,
                    Image = i.Item.Image,
                    Category = i.Item.Category == null ? null : new ItemCategoryResponse
                    {
                        Id = i.Item.Category.Id,
                        Name = i.Item.Category.Name
                    },
                    Quantity = i.Quantity,
                })
            })
            .ToListAsync(cancellationToken);
    }
}
