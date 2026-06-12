namespace GestionMateriel.Application.Messaging;

public interface IRequestHandler<in TRequest, TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}
