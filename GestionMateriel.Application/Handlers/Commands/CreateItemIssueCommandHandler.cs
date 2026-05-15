using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Commands;

public class CreateItemIssueCommandHandler(IItemIssueRepository itemIssueRepository, IMapper mapper) : IRequestHandler<CreateItemIssueCommand, ItemIssueResponse>
{
    public async Task<ItemIssueResponse> Handle(CreateItemIssueCommand command, CancellationToken cancellationToken)
    {
        var issue = mapper.Map<ItemIssue>(command.Request);

        await itemIssueRepository.AddAsync(issue);
        await itemIssueRepository.SaveChangesAsync();

        return mapper.Map<ItemIssueResponse>(issue);
    }
}
