
using GestionMateriel.Application.DTOs.Requests.Items.Issues;

namespace GestionMateriel.Application.Commands;

public record CreateItemIssueCommentCommand(int ItemIssueId, CreateItemIssueCommentRequest Request)
   ;
