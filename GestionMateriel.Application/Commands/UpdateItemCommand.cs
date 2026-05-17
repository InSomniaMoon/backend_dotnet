using GestionMateriel.Application.DTOs.Requests.Items;
using GestionMateriel.Application.DTOs.Responses;
using MediatR;

namespace GestionMateriel.Application.Commands;

public record UpdateItemCommand(int Id, UpdateItemRequest Request) : IRequest<ItemResponse?>;
