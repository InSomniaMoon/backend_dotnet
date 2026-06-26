using GestionMateriel.Application.DTOs.Common;
using GestionMateriel.Application.DTOs.Requests;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Queries.Backoffice;

public class GetPaginatedStructuresHandler(
GestionMaterielDbContext db
) : IRequestHandler<PaginatedRequest, PaginatedResponse<StructureResponse>>
{
    public async Task<PaginatedResponse<StructureResponse>> Handle(PaginatedRequest request, CancellationToken cancellationToken = default)
    {
        var query = db.Structures.AsNoTracking().IgnoreQueryFilters()
        .Where(s => string.IsNullOrEmpty(request.Q) ||
                    EF.Functions.ILike(s.Name, $"%{request.Q}%") ||
                    EF.Functions.ILike(s.CodeStructure, $"%{request.Q}%") ||
                    EF.Functions.ILike(s.NomStructure, $"%{request.Q}%"))
;

        var totalCount = await query.CountAsync(cancellationToken);

        var structures = await query
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .Select(s => new StructureResponse
            {
                Id = s.Id,
                Name = s.Name,
                CodeStructure = s.CodeStructure,
                Color = s.Color,
                NomStructure = s.NomStructure,
                Type = s.Type.ToString(),
            })
            .ToListAsync(cancellationToken);



        return new PaginatedResponse<StructureResponse>
        {
            Data = structures,
            TotalCount = totalCount,
            Page = request.Page,
            Size = request.Size
        };
    }
}