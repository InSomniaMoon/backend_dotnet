namespace GestionMateriel.Application.Commands;

public record UploadImageCommand(Stream File, string ImageType, string FileName);
