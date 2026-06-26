using GestionMateriel.Application.DTOs.Common;
using GestionMateriel.Application.DTOs.Requests;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Queries.Backoffice;

public class GetPaginatedUsersRequestHandler(
    GestionMaterielDbContext db
    ) : IRequestHandler<PaginatedRequest, PaginatedResponse<UserResponse>>
{
    public async Task<PaginatedResponse<UserResponse>> Handle(PaginatedRequest request, CancellationToken cancellationToken = default)
    {

        var authorizedOrderByProperties = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "firstName",
            "lastName",
            "email"
        };
        var query = db.Users.AsNoTracking()
        .Where(u => string.IsNullOrEmpty(request.Q) ||
                    EF.Functions.ILike(u.FirstName, $"%{request.Q}%") ||
                    EF.Functions.ILike(u.LastName, $"%{request.Q}%") ||
                    EF.Functions.ILike(u.Email, $"%{request.Q}%"));

        var totalCount = await query.CountAsync(cancellationToken);

        if (request.OrderBy is not null && authorizedOrderByProperties.Contains(request.OrderBy))
        {
            query = request.OrderBy switch
            {
                "firstName" when request.OrderDir?.ToLower() == "asc" => query.OrderBy(u => u.FirstName),
                "firstName" when request.OrderDir?.ToLower() == "desc" => query.OrderByDescending(u => u.FirstName),
                "lastName" when request.OrderDir?.ToLower() == "asc" => query.OrderBy(u => u.LastName),
                "lastName" when request.OrderDir?.ToLower() == "desc" => query.OrderByDescending(u => u.LastName),
                "email" when request.OrderDir?.ToLower() == "asc" => query.OrderBy(u => u.Email),
                "email" when request.OrderDir?.ToLower() == "desc" => query.OrderByDescending(u => u.Email),
                _ => query.OrderBy(u => u.LastName)
            };
        }
        else
        {
            query = query.OrderBy(u => u.LastName);
        }




        var users = await query
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .ToListAsync(cancellationToken);

        var userResponses = users.Select(u => new UserResponse
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Email = u.Email,
            Role = u.Role.ToString(),
            Phone = u.Phone,
        }).ToList();

        return new PaginatedResponse<UserResponse>
        {
            Data = userResponses,
            TotalCount = totalCount,
            Page = request.Page,
            Size = request.Size
        };
    }
}