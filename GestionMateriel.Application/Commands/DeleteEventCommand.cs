using MediatR;

namespace GestionMateriel.Application.Commands;

public record DeleteEventCommand(int Id) : IRequest<bool>;
