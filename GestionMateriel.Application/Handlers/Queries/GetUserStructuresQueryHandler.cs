using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Queries;

public class GetUserStructuresQueryHandler : IRequestHandler<GetUserStructuresQuery, UserWithStructuresResponse?>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUserStructuresQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserWithStructuresResponse?> Handle(GetUserStructuresQuery query, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetWithStructuresAsync(query.UserId);
        return user is null ? null : _mapper.Map<UserWithStructuresResponse>(user);
    }
}
