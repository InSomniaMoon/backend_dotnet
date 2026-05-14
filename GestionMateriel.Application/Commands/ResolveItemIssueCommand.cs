using GestionMateriel.Application.DTOs.Responses;
using MediatR;

namespace GestionMateriel.Application.Commands;

public record ResolveItemIssueCommand(int Id) : IRequest<ItemIssueResponse?>;
