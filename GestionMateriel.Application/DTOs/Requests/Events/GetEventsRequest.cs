using System.ComponentModel.DataAnnotations;

namespace GestionMateriel.Application.DTOs.Requests.Events;

public class GetEventsRequest
{
    [Range(1, int.MaxValue)]
    public int? StructureId { get; set; }

    public bool ActualOnly { get; set; }
}
