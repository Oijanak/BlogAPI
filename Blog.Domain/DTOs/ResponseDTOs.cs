namespace BlogApi.Domain.DTOs;

public class ApiErrorResponse
{
    public bool Success { get;} = false;
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;

}

public class ApiResponse<T>
{
    public bool Success { get; } = true;
    public T? Data { get; set; }
    public string? Message { get; set; }

   

}