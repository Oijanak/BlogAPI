using System.Net;

namespace BlogApi.Domain.Exceptions;
public class ApiException : ApplicationException
{
    public int StatusCode { get; set; }

    public ApiException(string message, HttpStatusCode statusCode) : base(message)
    {
        StatusCode = (int)statusCode; 
    }
}