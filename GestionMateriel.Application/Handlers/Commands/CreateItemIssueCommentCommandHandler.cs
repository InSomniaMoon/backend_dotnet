using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Commands;

public class CreateItemIssueCommentCommandHandler : IRequestHandler<CreateItemIssueCommentCommand, ItemIssueCommentResponse?>
{
    private readonly IItemIssueRepository _itemIssueRepository;
    private readonly IItemIssueCommentRepository _itemIssueCommentRepository;
    private readonly IMapper _mapper;

    public CreateItemIssueCommentCommandHandler(
        IItemIssueRepository itemIssueRepository,
        IItemIssueCommentRepository itemIssueCommentRepository,
        IMapper mapper)
    {
        _itemIssueRepository = itemIssueRepository;
        _itemIssueCommentRepository = itemIssueCommentRepository;
        _mapper = mapper;
    }

    public async Task<ItemIssueCommentResponse?> Handle(CreateItemIssueCommentCommand command, CancellationToken cancellationToken)
    {
        var issue = await _itemIssueRepository.GetByIdAsync(command.ItemIssueId);
        if (issue is null)
        {
            return null;
        }

        var comment = _mapper.Map<ItemIssueComment>(command.Request);
        comment.ItemIssueId = command.ItemIssueId;

        await _itemIssueCommentRepository.AddAsync(comment);
        await _itemIssueCommentRepository.SaveChangesAsync();

        return _mapper.Map<ItemIssueCommentResponse>(comment);
    }
}
