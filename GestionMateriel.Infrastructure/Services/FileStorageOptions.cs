namespace GestionMateriel.Infrastructure.Services;

public sealed class FileStorageOptions
{
    public int MaxFileSizeInMb { get; set; } = 10;
    public string[] AllowedExtensions { get; set; } = [".jpg", ".jpeg", ".png", ".gif"];
    public string StoragePath { get; set; } = "wwwroot/uploads";
}
