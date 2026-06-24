namespace GestionMateriel.Application.Features.Items.Commands;

public record PreviewImportItemsCommand(
    Stream FileStream,
    string FileName,
    int StructureId,
    CancellationToken CancellationToken
    );
