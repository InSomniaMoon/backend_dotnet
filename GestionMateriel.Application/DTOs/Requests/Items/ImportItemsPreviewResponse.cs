using GestionMateriel.Application.DTOs.Responses;

namespace GestionMateriel.Application.DTOs.Requests.Items;

public record PreviewImportItemsResponse(
    int StructureId,
    int ItemsCount,
    List<ImportItemPreview> Rows,
    List<UnknownCategory> UnknownCategories,
    List<ItemCategoryResponse> ExistingCategories
);

// "row": 2,
// "item_name": "Tente 2 places",
// "description": "Tente l\u221a\u00a9g\u221a\u00aere 2 places",
// "category_name": "Tente",
// "quantity": 1,
// "status": "unknown",
// "errors": [],
// "category": null
public record ImportItemPreview(
    string ItemName,
    string Description,
    string CategoryName,
    int Quantity,
    string Status,
    List<string> Errors,
    ItemCategoryResponse? Category
)
{
    
};


public record UnknownCategory(
    string Name,
    int Occurrences
);



