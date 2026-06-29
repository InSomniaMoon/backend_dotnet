namespace GestionMateriel.Application.DTOs.Responses.Backoffice;

public record ImportStructuresResponse(
    int TotalRows,
    int CreatedCount,
    int SkippedExistingCount,
    List<ImportStructureRowError> Errors
);

public record ImportStructureRowError(
    int Row,
    string CodeStructure,
    string Message
);
