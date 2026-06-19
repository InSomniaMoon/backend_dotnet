using GestionMateriel.Application.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace GestionMateriel.Infrastructure.Services;

public sealed class LocalImageStorageService(
    IHostEnvironment hostEnvironment,
    IOptions<FileStorageOptions> fileStorageOptions
) : IImageStorageService
{
    private readonly FileStorageOptions _options = fileStorageOptions.Value;

    public async Task<string> UploadImageAsync(Stream fileStream, string imageType, string fileName,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fileStream);

        if (string.IsNullOrWhiteSpace(imageType))
            throw new ArgumentException("Image type is required.", nameof(imageType));

        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name is required.", nameof(fileName));

        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        var allowedExtensions = _options.AllowedExtensions
            .Select(e => e.ToLowerInvariant())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        if (string.IsNullOrWhiteSpace(extension) || !allowedExtensions.Contains(extension))
            throw new InvalidOperationException("File extension is not allowed.");

        var maxSizeInBytes = _options.MaxFileSizeInMb * 1024L * 1024L;
        var safeImageType = imageType.Trim().ToLowerInvariant();

        var storageRoot = Path.IsPathRooted(_options.StoragePath)
            ? _options.StoragePath
            : Path.Combine(hostEnvironment.ContentRootPath, _options.StoragePath);

        var targetDirectory = Path.Combine(storageRoot, safeImageType);
        Directory.CreateDirectory(targetDirectory);

        var storedFileName = $"{Guid.NewGuid():N}{extension}";
        var physicalPath = Path.Combine(targetDirectory, storedFileName);

        try
        {
            await using var destinationStream = new FileStream(
                physicalPath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                bufferSize: 81920,
                useAsync: true);

            await CopyWithSizeLimitAsync(fileStream, destinationStream, maxSizeInBytes, cancellationToken);
        }
        catch
        {
            if (File.Exists(physicalPath))
            {
                File.Delete(physicalPath);
            }

            throw;
        }

        return $"/uploads/{safeImageType}/{storedFileName}";
    }

    private static async Task CopyWithSizeLimitAsync(Stream source, Stream destination, long maxSizeInBytes,
        CancellationToken cancellationToken)
    {
        var totalRead = 0L;
        var buffer = new byte[81920];

        while (true)
        {
            var read = await source.ReadAsync(buffer.AsMemory(0, buffer.Length), cancellationToken);
            if (read == 0)
                break;

            totalRead += read;
            if (totalRead > maxSizeInBytes)
                throw new InvalidOperationException("File is too large.");

            await destination.WriteAsync(buffer.AsMemory(0, read), cancellationToken);
        }
    }
}
