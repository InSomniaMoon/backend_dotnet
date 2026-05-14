using MediatR;

namespace GestionMateriel.Application.Commands;

public record DeleteItemCategoryCommand(int Id) : IRequest<bool>;
