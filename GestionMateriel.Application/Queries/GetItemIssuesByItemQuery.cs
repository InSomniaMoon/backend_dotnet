using GestionMateriel.Application.DTOs.Responses;
using MediatR;

namespace GestionMateriel.Application.Queries;

public record GetItemIssuesByItemQuery(int ItemId) : IRequest<IEnumerable<ItemIssueResponse>>;
