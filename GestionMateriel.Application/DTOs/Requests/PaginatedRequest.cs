using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace GestionMateriel.Application.DTOs.Requests;

public class PaginatedRequest
{
    [Range(1, int.MaxValue)]
    public int Page { get; set; } = 1;

    [Range(1, 200)]
    public int Size { get; set; } = 50;
    public string? Q { get; set; }

    [RegularExpression("asc|desc|null", ErrorMessage = "SortDir must be either 'asc', 'desc' or 'null'.")]
    public string? SortDir { get; set; } = "asc";
    public string? SortBy { get; set; }


}