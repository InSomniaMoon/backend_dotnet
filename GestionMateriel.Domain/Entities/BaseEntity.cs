using System.ComponentModel.DataAnnotations.Schema;

namespace GestionMateriel.Domain.Entities;

public abstract class BaseEntity
{
    [Column("id")]
    public int Id { get; set; }
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}

