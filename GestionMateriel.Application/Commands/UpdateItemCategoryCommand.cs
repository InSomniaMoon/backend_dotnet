using GestionMateriel.Application.DTOs.Requests.Categories;

namespace GestionMateriel.Application.Commands;

public record UpdateItemCategoryCommand(int Id, UpdateItemCategoryRequest Request);
