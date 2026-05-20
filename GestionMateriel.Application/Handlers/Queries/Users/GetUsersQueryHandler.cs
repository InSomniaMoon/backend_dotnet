using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Queries.Users;

public class GetUsersQueryHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetUsersQuery, IEnumerable<UserResponse>>
{
    public async Task<IEnumerable<UserResponse>> Handle(GetUsersQuery query, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetAllAsync();
        return users.Select(u => mapper.Map<UserResponse>(u));
    }
}
