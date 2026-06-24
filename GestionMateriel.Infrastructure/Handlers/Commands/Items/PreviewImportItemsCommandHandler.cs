using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using GestionMateriel.Application.DTOs.Requests.Items;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Features.Items.Commands;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Commands.Items;

public class PreviewImportItemsCommandHandler(
    GestionMaterielDbContext db
) : IRequestHandler<PreviewImportItemsCommand, PreviewImportItemsResponse>
{
    public async Task<PreviewImportItemsResponse> Handle(PreviewImportItemsCommand request, CancellationToken cancellationToken)
    {
        var csvRows = await ParseCSVFile(request.FileStream, cancellationToken);

        var knownCategories = await db.ItemCategories.AsNoTracking().Select(s => new ItemCategoryResponse
        {
            Id = s.Id,
            Name = s.Name,
            Identified = s.Identified,
        }).ToListAsync(cancellationToken);

        var unknownCategoryNames = csvRows
            .Where(r => !knownCategories.Any(c => c.Name == r.CategoryName))
            .Select(r => r.CategoryName)
            .Distinct()
            .ToList();

        var unknownCategories = unknownCategoryNames
        .Select(name => new UnknownCategory(name, csvRows.Count(r => r.CategoryName == name))).ToList();

        var response = new PreviewImportItemsResponse(
           request.StructureId,
              csvRows.Count,
              [.. csvRows.Select((row, index) => new ImportItemPreview(
                  row.ItemName,
                  row.Description,
                  row.CategoryName,
                  row.Quantity,
                  knownCategories.Any(c => c.Name == row.CategoryName) ? "matched" : "unknown",
                  row.Errors,
                  knownCategories.FirstOrDefault(c => c.Name == row.CategoryName)
              ))],
              unknownCategories,
            knownCategories
        );

        return response;



    }


    private static async Task<List<ImportItemPreview>> ParseCSVFile(Stream fileStream, CancellationToken cancellationToken)
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
        csv.Context.RegisterClassMap<ImportItemPreviewCsvRowMap>();
        var records = csv.GetRecords<ImportItemPreviewCsvRow>()
            .Select(r => new ImportItemPreview(
                r.ItemName,
                r.Description,
                r.CategoryName,
                r.Quantity,
                r.Status,
                [],
                null))
            .ToList();

        return records;
    }

    private static string NormalizeHeader(string? header)
    {
        return (header ?? string.Empty)
            .Trim()
            .ToLowerInvariant();
    }

    private sealed class ImportItemPreviewCsvRowMap : ClassMap<ImportItemPreviewCsvRow>
    {
        public ImportItemPreviewCsvRowMap()
        {
            Map(m => m.ItemName).Name("itemname", "nom", "name", "article", "materiel");
            Map(m => m.Description).Name("description", "desc");
            Map(m => m.CategoryName).Name("categoryname", "categorie", "category", "category_name");
            Map(m => m.Quantity).Name("quantity", "quantite", "qte").Optional().Default(1);
            Map(m => m.Status).Name("status", "etat").Optional().Default("unknown");
            Map(m => m.Errors).Name("errors", "erreurs").Optional().Default([]);
        }
    }

    private sealed class ImportItemPreviewCsvRow
    {
        public string ItemName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;

        public int Quantity { get; set; } = 1;

        public string Status { get; set; } = "unknown";

        public List<string> Errors { get; set; } = [];
    }
}