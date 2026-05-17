using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Domain.Entities;

namespace GestionMateriel.Application.Mappings;

public static class ItemMappings
{
    public static ItemResponse ToResponse(this Item item)
    {
        return new ItemResponse
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            CategoryId = item.CategoryId,
            StructureId = item.StructureId,
            Usable = item.Usable,
            Stock = item.Stock,
            DateOfBuy = item.DateOfBuy,
            ImagePath = item.Image,
            State = item.State.ToString(),
            UsableStock = item.UsableStock,
            Category = item.Category != null ? new ItemCategoryResponse
            {
                Id = item.Category.Id,
                Name = item.Category.Name,
                StructureId = item.Category.StructureId,
                Identified = item.Category.Identified
            } : null
        };
    }
}
