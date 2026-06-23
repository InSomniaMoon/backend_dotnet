using System.ComponentModel.DataAnnotations;

namespace GestionMateriel.Application.DTOs.Requests.Events;

public record UpdateEventRequest
{
    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Range(1, int.MaxValue)]
    public int StructureId { get; set; }

    [Range(1, int.MaxValue)]
    public int? CreatedById { get; set; }

    [MaxLength(1000)]
    public string? Comment { get; set; }

    public List<CreateUpdateEventItemRequest> Items { get; set; } = [];

    public record CreateUpdateEventItemRequest
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
