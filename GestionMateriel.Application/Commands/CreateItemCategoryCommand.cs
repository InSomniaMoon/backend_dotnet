using GestionMateriel.Application.DTOs.Requests.Categories;

namespace GestionMateriel.Application.Commands;

public record CreateItemCategoryCommand(CreateItemCategoryRequest Request, int StructureId, string? CodeStructure);
