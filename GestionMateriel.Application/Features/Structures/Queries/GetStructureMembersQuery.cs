
namespace GestionMateriel.Application.Features.Structures.Queries;

public record GetStructureMembersQuery(
    int StructureId,
    int PageNumber = 1,
    int PageSize = 50,
    string? Q = null,
    string? OrderDir = "asc",
    string? OrderBy = "lastname")
{
    public string[] ValidOrderByFields { get; } = ["firstname", "lastname", "email", "role"];

    public bool Validate()
    {
        // OrderBy is in ["firstName", "lastName", "email", "role"] or OrderBy is null;

        if (OrderBy != null && !ValidOrderByFields.Contains(OrderBy))
        {
            throw new ArgumentException(
                $"Invalid OrderBy value. Must be one of: {string.Join(", ", ValidOrderByFields)}",
                nameof(OrderBy));
        }

        return true;
    }
};