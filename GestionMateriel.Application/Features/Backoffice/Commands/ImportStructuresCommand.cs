namespace GestionMateriel.Application.Features.Backoffice.Commands;

public record ImportStructuresCommand(
    Stream FileStream,
    string FileName,
    CancellationToken CancellationToken
);
