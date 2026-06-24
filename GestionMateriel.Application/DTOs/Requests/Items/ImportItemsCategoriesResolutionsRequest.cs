namespace GestionMateriel.Application.DTOs.Requests.Items;

public record ImportItemsCategoriesResolutionsRequest(
    List<ImportItemCategoryResolution> Resolutions
);


public record ImportItemCategoryResolution(
    string Action,
    string Name,
    int? CategoryId,
    bool? Identified
);