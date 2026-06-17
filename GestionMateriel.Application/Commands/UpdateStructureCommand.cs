using GestionMateriel.Application.DTOs.Requests.Structures;

namespace GestionMateriel.Application.Commands;

public record UpdateStructureCommand(int Id, UpdateStructureRequest Request);
