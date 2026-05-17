using GestionMateriel.Application.DTOs.Requests.Categories;
using GestionMateriel.Application.DTOs.Responses;
using MediatR;

namespace GestionMateriel.Application.Commands;

public record CreateItemCategoryCommand(CreateItemCategoryRequest Request) : IRequest<ItemCategoryResponse>;
