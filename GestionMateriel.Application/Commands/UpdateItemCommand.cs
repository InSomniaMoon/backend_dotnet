using GestionMateriel.Application.DTOs.Requests.Items;

namespace GestionMateriel.Application.Commands;

public record UpdateItemCommand(int Id, UpdateItemRequest Request);
