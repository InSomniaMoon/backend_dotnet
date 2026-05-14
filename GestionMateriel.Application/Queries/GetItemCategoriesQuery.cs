using GestionMateriel.Application.DTOs.Responses;
using MediatR;

namespace GestionMateriel.Application.Queries;

public record GetItemCategoriesQuery(int StructureId) : IRequest<IEnumerable<ItemCategoryResponse>>;
