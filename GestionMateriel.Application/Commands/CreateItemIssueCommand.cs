using GestionMateriel.Application.DTOs.Requests;
using GestionMateriel.Application.DTOs.Responses;
using MediatR;

namespace GestionMateriel.Application.Commands;

public record CreateItemIssueCommand(CreateItemIssueRequest Request) : IRequest<ItemIssueResponse>;
