using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Common;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Application.Services;

namespace GestionMateriel.Infrastructure.Handlers.Commands.Common;

public class UploadImageCommandHandler(
    IImageStorageService imageStorageService
) : IRequestHandler<UploadImageCommand, ImageCreatedResponse>
{
    public async Task<ImageCreatedResponse> Handle(UploadImageCommand request, CancellationToken cancellationToken)
    {
        var imageUrl = await imageStorageService.UploadImageAsync(
            request.File,
            request.ImageType,
            request.FileName,
            cancellationToken);

        return new ImageCreatedResponse
        {
            Path = imageUrl
        };
    }
}