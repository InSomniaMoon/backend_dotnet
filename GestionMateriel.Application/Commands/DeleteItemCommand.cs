using MediatR;

namespace GestionMateriel.Application.Commands;

public record DeleteItemCommand(int Id) : IRequest<bool>;
