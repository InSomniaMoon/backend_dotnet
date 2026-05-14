using GestionMateriel.Application.DTOs.Responses;
using MediatR;

namespace GestionMateriel.Application.Queries;

public record GetItemIssueCommentsQuery(int ItemIssueId) : IRequest<IEnumerable<ItemIssueCommentResponse>>;
