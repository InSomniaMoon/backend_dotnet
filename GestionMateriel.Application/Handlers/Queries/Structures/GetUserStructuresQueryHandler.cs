using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Interfaces;
using GestionMateriel.Application.Messaging;

namespace GestionMateriel.Application.Handlers.Queries.Structures;

public class GetUserStructuresQueryHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetUserStructuresQuery, UserWithStructuresResponse?>
{
    public async Task<UserWithStructuresResponse?> Handle(GetUserStructuresQuery query, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetWithStructuresAsync(query.UserId);
        return user is null ? null : mapper.Map<UserWithStructuresResponse>(user);
    }
}
