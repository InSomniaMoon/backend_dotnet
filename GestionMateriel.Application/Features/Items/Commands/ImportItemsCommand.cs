using GestionMateriel.Application.DTOs.Requests.Items;

namespace GestionMateriel.Application.Features.Items.Commands;

public record ImportItemsCommand(Stream FileStream,
string FileName,
int StructureId,
string CodeStructure,
 List<ImportItemCategoryResolution> Resolutions, CancellationToken CancellationToken);
