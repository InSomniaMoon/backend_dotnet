namespace GestionMateriel.Application.DTOs.Common;

public class PaginatedResponse<T>
{
    public IEnumerable<T> Data { get; set; } = [];
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int Size { get; set; }

    public int TotalPages => (TotalCount + Size - 1) / Size;
}

public class PaginationFilter
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
}

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Details { get; set; }
    public Dictionary<string, string[]>? ValidationErrors { get; set; }
}

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
}
