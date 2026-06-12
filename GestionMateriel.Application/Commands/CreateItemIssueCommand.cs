using GestionMateriel.Application.DTOs.Requests.Items.Issues;

namespace GestionMateriel.Application.Commands;

public record CreateItemIssueCommand(int ItemId, int ReportedById, CreateItemIssueRequest Request);
