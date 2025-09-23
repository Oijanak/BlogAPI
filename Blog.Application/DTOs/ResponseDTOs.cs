using System.Text.Json.Serialization;

namespace BlogApi.Application.DTOs;

public class ApiErrorResponse
{
    public bool Success { get;} = false;
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    

}

public class ApiResponse<T>
{
    public bool Success { get; } = true;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T? Data { get; set; }
    public string? Message { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? totalSize { get; set; }



}