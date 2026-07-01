using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using GestionMateriel.Application.DTOs.Requests.Items;
using GestionMateriel.Application.DTOs.Responses.Items;
using GestionMateriel.Application.Features.Items.Commands;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Infrastructure.Data;

namespace GestionMateriel.Infrastructure.Handlers.Commands.Items;

public class ImportItemsCommandHandler(
    GestionMaterielDbContext db
) : IRequestHandler<ImportItemsCommand, ImportItemsResponse>
{
    public async Task<ImportItemsResponse> Handle(ImportItemsCommand request, CancellationToken cancellationToken)
    {

        request.FileStream.Position = 0; // Reset the stream position to the beginning
        var csvRows = await ParseCSVFile(request.FileStream, cancellationToken);

        var CategoriesToCreate = request.Resolutions
            .Where(r => r.Action == "create")
            .Select(r => new ItemCategory()
            {
                StructureId = request.StructureId,
                CodeStructure = request.CodeStructure,
                Name = r.Name,
                Identified = r.Identified ?? false,
            })
            .ToList();
        var existingCategories = request.Resolutions.Where(r => r.Action == "existing" && r.CategoryId.HasValue)
            .ToList();
        db.ItemCategories.AddRange(CategoriesToCreate);

        var items = csvRows.Select(row => (new Item()
        {
            State = ItemState.OK,
            StructureId = request.StructureId,
            CodeStructure = request.CodeStructure,
            Name = row.ItemName,
            Description = row.Description,
            Stock = row.Quantity,
            UsableStock = row.Quantity,
            Usable = true,
        }, row.CategoryName)).ToList();

        var itemsWithStructure = items.Select((row) =>
          {
              var categoryName = row.CategoryName;
              var item = row.Item1;
              var resolution = request.Resolutions.FirstOrDefault(r => r.Name == categoryName);
              if (resolution is null)
              {
                  // category already exists and has good name. we just take the first category with that name
                  var existingCategory = db.ItemCategories.FirstOrDefault(c => c.Name == categoryName);

                  if (existingCategory is not null)
                  {
                      item.CategoryId = existingCategory.Id;
                  }
              }
              else
              {
                  if (resolution.Action == "create")
                  {
                      var newCategory = CategoriesToCreate.FirstOrDefault(c => c.Name == resolution.Name)!;
                      item.Category = newCategory;
                  }
                  else if (resolution.Action == "existing" && resolution.CategoryId.HasValue)
                  {
                      item.CategoryId = resolution.CategoryId.Value;
                  }
                  else
                  {
                      // This should not happen, but just in case
                      throw new InvalidOperationException($"Invalid resolution for category '{resolution.Name}'.");
                  }
              }
              return item;
          });

        db.Items.AddRange(itemsWithStructure);

        await db.SaveChangesAsync(cancellationToken);

        return new ImportItemsResponse(
         itemsWithStructure.Count()
        );

    }

    private static Encoding DetectEncoding(byte[] bytes)
    {
        // Check UTF-8 BOM
        if (bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
            return Encoding.UTF8;

        // Check UTF-16 LE BOM
        if (bytes.Length >= 2 && bytes[0] == 0xFF && bytes[1] == 0xFE)
            return Encoding.Unicode;

        // Try strict UTF-8 validation (no fallback replacement characters)
        try
        {
            new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true).GetString(bytes);
            return Encoding.UTF8;
        }
        catch (DecoderFallbackException)
        {
            return Encoding.Latin1;
        }
    }

    private static async Task<List<ImportItemPreview>> ParseCSVFile(Stream fileStream, CancellationToken cancellationToken)
    {
        using var buffer = new MemoryStream();
        await fileStream.CopyToAsync(buffer, cancellationToken);
        var bytes = buffer.ToArray();
        var encoding = DetectEncoding(bytes);
        using var memStream = new MemoryStream(bytes);
        using var reader = new StreamReader(memStream, encoding);


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
                r.Errors ?? [],
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
