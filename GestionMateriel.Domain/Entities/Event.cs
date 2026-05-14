namespace GestionMateriel.Domain.Entities;

public class Event : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int StructureId { get; set; }
    public int? CreatedById { get; set; }
    public string? Comment { get; set; }

    // Navigation properties
    public Structure Structure { get; set; } = null!;
    public User? CreatedBy { get; set; }
    public ICollection<EventSubscription> Subscriptions { get; set; } = [];
}
