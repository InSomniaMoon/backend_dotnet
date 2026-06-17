namespace GestionMateriel.Application.Queries;

public record GetItemsQuery(
    int Page = 1,
    int Size = 20,
    string? Q = null,
    string? OrderDir = "asc",
    string? OrderBy = null
);
