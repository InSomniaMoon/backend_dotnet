using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Commands;

public class CreateItemIssueCommentCommandHandler(
    IItemIssueRepository itemIssueRepository,
    IItemIssueCommentRepository itemIssueCommentRepository,
    IMapper mapper) : IRequestHandler<CreateItemIssueCommentCommand, ItemIssueCommentResponse?>
{
    public async Task<ItemIssueCommentResponse?> Handle(CreateItemIssueCommentCommand command, CancellationToken cancellationToken)
    {
        var issue = await itemIssueRepository.GetByIdAsync(command.ItemIssueId);
        if (issue is null)
        {
            return null;
        }

        var comment = mapper.Map<ItemIssueComment>(command.Request);
        comment.ItemIssueId = command.ItemIssueId;

        await itemIssueCommentRepository.AddAsync(comment);
        await itemIssueCommentRepository.SaveChangesAsync();

        return mapper.Map<ItemIssueCommentResponse>(comment);
    }
}
