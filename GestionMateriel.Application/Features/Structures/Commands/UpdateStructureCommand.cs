namespace GestionMateriel.Application.Features.Structures.Commands;

public record UpdateStructureCommand(
    int Id,
    string Color,
    string Name,
    List<int> MembersIds,
    string? ImageUrl
);