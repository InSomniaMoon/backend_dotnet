using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Domain.Entities;

namespace GestionMateriel.Application.Mappings;

public static class ItemCategoryMappings
{
    public static ItemCategoryResponse ToResponse(this ItemCategory category)
    {
        return new ItemCategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            StructureId = category.StructureId,
            Identified = category.Identified
        };
    }
}
