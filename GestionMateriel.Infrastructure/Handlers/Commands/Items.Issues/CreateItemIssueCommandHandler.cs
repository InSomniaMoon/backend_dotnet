using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Infrastructure.Data;

namespace GestionMateriel.Infrastructure.Handlers.Commands.Items.Issues;

public class CreateItemIssueCommandHandler(GestionMaterielDbContext db)
    : IRequestHandler<CreateItemIssueCommand, ItemIssueResponse>
{
    public async Task<ItemIssueResponse> Handle(CreateItemIssueCommand command, CancellationToken cancellationToken)
    {
        var issue = new ItemIssue
        {
            ItemId = command.ItemId,
            AffectedQuantity = command.Request.AffectedQuantity,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            ReportedBy = command.ReportedById,
            Value = command.Request.Value,
        };
        await db.ItemIssues.AddAsync(issue, cancellationToken);

        var item = await db.Items.FindAsync([command.ItemId], cancellationToken);
        if (!command.Request.Usable)
        {
            item?.Usable = false;
            item?.State = ItemState.KO;
        }
        else
        {
            item?.Usable = true;
            item?.State = ItemState.NOK;
        }

        await db.SaveChangesAsync(cancellationToken);
        return new ItemIssueResponse
        {
            Id = issue.Id,
            ItemId = issue.ItemId,
            CreatedAt = issue.CreatedAt,
            Status = issue.Status.ToString(),
            Value = issue.Value,
            ReportedById = issue.ReportedBy,
            AffectedQuantity = issue.AffectedQuantity,
        };
    }
}
