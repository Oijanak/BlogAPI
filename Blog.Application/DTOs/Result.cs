using System.Text.Json.Serialization;

namespace BlogApi.Application.DTOs;

public class Result<T>
{
    public bool IsSuccess { get; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Error { get; }
    public int StatusCode { get; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T? Data{ get; }

    private Result(bool isSuccess, string? error, int statusCode, T? value)
    {
        IsSuccess = isSuccess;
        Error = error;
        StatusCode = statusCode;
        Data = value;
    }

    public static Result<T> Success(T value, int statusCode = 200) => new(true, null, statusCode, value);
    public static Result<T> Failure(string error, int statusCode = 400) => new(false, error, statusCode, default);
}
