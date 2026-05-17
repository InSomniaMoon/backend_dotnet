
using GestionMateriel.Application.DTOs.Requests.Items.Issues;
using GestionMateriel.Application.DTOs.Responses;
using MediatR;

namespace GestionMateriel.Application.Commands;

public record CreateItemIssueCommentCommand(int ItemIssueId, CreateItemIssueCommentRequest Request)
    : IRequest<ItemIssueCommentResponse?>;
