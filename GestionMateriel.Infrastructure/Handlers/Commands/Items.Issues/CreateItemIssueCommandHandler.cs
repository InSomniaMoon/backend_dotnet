using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Infrastructure.Data;

namespace GestionMateriel.Infrastructure.Handlers.Commands.Items.Issues;

public class CreateItemIssueCommandHandler(GestionMaterielDbContext db, IMapper mapper)
    : IRequestHandler<CreateItemIssueCommand, ItemIssueResponse>
{
    public async Task<ItemIssueResponse> Handle(CreateItemIssueCommand command, CancellationToken cancellationToken)
    {
        var issue = mapper.Map<ItemIssue>(command.Request);
        await db.ItemIssues.AddAsync(issue, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
        return mapper.Map<ItemIssueResponse>(issue);
    }
}
