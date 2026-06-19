namespace GestionMateriel.Application.Services;

public interface IImageStorageService
{
    Task<string> UploadImageAsync(Stream fileStream, string imageType, string fileName, CancellationToken cancellationToken = default);
}
