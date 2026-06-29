using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using GestionMateriel.Application.DTOs.Responses.Backoffice;
using GestionMateriel.Application.Features.Backoffice.Commands;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Commands.Backoffice;

public class ImportStructuresCommandHandler(
    GestionMaterielDbContext db
) : IRequestHandler<ImportStructuresCommand, ImportStructuresResponse>
{
    public async Task<ImportStructuresResponse> Handle(ImportStructuresCommand request, CancellationToken cancellationToken)
    {
        var csvRows = ParseCsvFile(request.FileStream);

        var existingCodes = await db.Structures.AsNoTracking()
        .IgnoreQueryFilters()
            .Select(s => s.CodeStructure)
            .ToListAsync(cancellationToken);
        var existingCodesSet = new HashSet<string>(existingCodes);

        var errors = new List<ImportStructureRowError>();
        var codesSeenInFile = new HashSet<string>();
        var structuresToCreate = new List<Structure>();

        for (var i = 0; i < csvRows.Count; i++)
        {
            var row = csvRows[i];
            var rowNumber = i + 2; // +1 for 0-index, +1 for header line

            if (string.IsNullOrWhiteSpace(row.CodeStructure) || string.IsNullOrWhiteSpace(row.NomStructure))
            {
                errors.Add(new ImportStructureRowError(rowNumber, row.CodeStructure, "nomStructure et codeStructure sont obligatoires."));
                continue;
            }

            if (!codesSeenInFile.Add(row.CodeStructure))
            {
                errors.Add(new ImportStructureRowError(rowNumber, row.CodeStructure, "codeStructure dupliqué dans le fichier."));
                continue;
            }

            if (existingCodesSet.Contains(row.CodeStructure))
            {
                continue;
            }

            structuresToCreate.Add(new Structure
            {
                CodeStructure = row.CodeStructure,
                NomStructure = row.NomStructure,
                Name = string.IsNullOrWhiteSpace(row.NomCustom) ? row.NomStructure : row.NomCustom,
                ParentCode = string.IsNullOrWhiteSpace(row.ParentCodeStructure) ? null : row.ParentCodeStructure,
                Type = ComputeTypeFromCode(row.CodeStructure),
                Color = "#8132d1"
            });
        }

        var skippedExistingCount = csvRows.Count(r => existingCodesSet.Contains(r.CodeStructure));

        db.Structures.AddRange(structuresToCreate);
        await db.SaveChangesAsync(cancellationToken);

        return new ImportStructuresResponse(
            csvRows.Count,
            structuresToCreate.Count,
            skippedExistingCount,
            errors
        );
    }

    private static StructureTypeEnum ComputeTypeFromCode(string codeStructure)
    {
        if (codeStructure.EndsWith("0000", StringComparison.Ordinal))
        {
            return StructureTypeEnum.Territoire;
        }

        if (codeStructure.EndsWith("00", StringComparison.Ordinal))
        {
            return StructureTypeEnum.Groupe;
        }

        return StructureTypeEnum.Unite;
    }

    private static List<ImportStructureCsvRow> ParseCsvFile(Stream fileStream)
    {
        using var reader = new StreamReader(fileStream, Encoding.UTF8);
        var configuration = new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
        {
            Delimiter = ";",
            HeaderValidated = null,
            MissingFieldFound = null,
            PrepareHeaderForMatch = args => NormalizeHeader(args.Header),
        };

        using var csv = new CsvReader(reader, configuration);
        csv.Context.RegisterClassMap<ImportStructureCsvRowMap>();
        return [.. csv.GetRecords<ImportStructureCsvRow>()];
    }

    private static string NormalizeHeader(string? header)
    {
        return (header ?? string.Empty).Trim().ToLowerInvariant();
    }

    private sealed class ImportStructureCsvRowMap : ClassMap<ImportStructureCsvRow>
    {
        public ImportStructureCsvRowMap()
        {
            Map(m => m.NomStructure).Name("nomstructure", "nom_structure", "nom");
            Map(m => m.CodeStructure).Name("codestructure", "code_structure", "code");
            Map(m => m.ParentCodeStructure).Name("parentcodestructure", "parent_code_structure", "parentcode", "parent_code").Optional();
            Map(m => m.NomCustom).Name("nomcustom", "nom_custom").Optional();
        }
    }

    private sealed class ImportStructureCsvRow
    {
        public string NomStructure { get; set; } = string.Empty;
        public string CodeStructure { get; set; } = string.Empty;
        public string? ParentCodeStructure { get; set; }
        public string? NomCustom { get; set; }
    }
}
