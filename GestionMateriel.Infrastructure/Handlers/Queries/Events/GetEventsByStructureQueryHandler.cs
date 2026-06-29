using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Features.Events.Queries;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Queries.Events;

public class GetEventsByStructureQueryHandler(GestionMaterielDbContext db)
    : IRequestHandler<GetEventsByStructureQuery, IEnumerable<EventResponse>>
{
    public async Task<IEnumerable<EventResponse>> Handle(GetEventsByStructureQuery request,
        CancellationToken cancellationToken)
    {

        var isUnite = request.StructureType == Domain.Enums.StructureTypeEnum.Unite;
        var query = db.Events
            .AsNoTracking()
            .Where(e => e.StartDate >= request.StartDate || e.EndDate >= request.StartDate)
            .Where(e => e.StartDate <= request.EndDate || e.EndDate <= request.EndDate);

        if (isUnite)
        {
            query = query.Where(e => e.Structure.CodeStructure == request.StructureCode);
        }

        var events = await query.OrderByDescending(e => e.StartDate)
        .Select(e => new EventResponse()
        {
            Id = e.Id,
            Name = e.Name,
            StartDate = e.StartDate,
            EndDate = e.EndDate,
            StructureId = e.StructureId,
            Comment = e.Comment,
            CreatedById = e.UserId,
            Structure = new StructureResponse()
            {
                Id = e.Structure.Id,
                Name = e.Structure.Name,
                Color = e.Structure.Color,
            },
        })
        .ToListAsync(cancellationToken);
        return events;
    }
}
