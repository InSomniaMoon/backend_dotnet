using GestionMateriel.Application.Features.Backoffice.Queries;
using GestionMateriel.Domain.Entities;

namespace GestionMateriel.Application.Features.Backoffice.Commands;

public record UpdateUserStructuresCommand(int UserId, List<UserStructureRequest> Structures);