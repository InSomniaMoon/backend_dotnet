using GestionMateriel.Application.DTOs.Requests.Items;
using GestionMateriel.Application.DTOs.Responses;

namespace GestionMateriel.Application.Commands;

public record CreateItemCommand(CreateItemRequest Request);
