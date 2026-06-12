using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Infrastructure.Data;

namespace GestionMateriel.Infrastructure.Handlers.Commands.Items.Issues;

public class CreateItemIssueCommentCommandHandler(GestionMaterielDbContext db, IMapper mapper)
    : IRequestHandler<CreateItemIssueCommentCommand, ItemIssueCommentResponse?>
{
    public async Task<ItemIssueCommentResponse?> Handle(CreateItemIssueCommentCommand command, CancellationToken cancellationToken)
    {
        var issue = await db.ItemIssues.FindAsync([command.ItemIssueId], cancellationToken);
        if (issue is null) return null;

        var comment = mapper.Map<ItemIssueComment>(command.Request);
        comment.ItemIssueId = command.ItemIssueId;

        await db.ItemIssueComments.AddAsync(comment, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
        return mapper.Map<ItemIssueCommentResponse>(comment);
    }
}
