using GestionMateriel.Application.DTOs.Requests.Structures;
using GestionMateriel.Application.DTOs.Responses;

namespace GestionMateriel.Application.Commands;

public record UpdateStructureCommand(int Id, UpdateStructureRequest Request);
