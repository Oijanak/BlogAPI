namespace Blog.API.Filters;

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text;
using System.Threading.Tasks;

public class RequestResponseLoggingFilter : IAsyncActionFilter
{
    private readonly ILogger<RequestResponseLoggingFilter> _logger;

    public RequestResponseLoggingFilter(ILogger<RequestResponseLoggingFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var request = context.HttpContext.Request;
        request.EnableBuffering();
        string requestBody = "";
        if (request.ContentLength > 0 && request.Body.CanSeek)
        {
            request.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            requestBody = await reader.ReadToEndAsync();
            request.Body.Seek(0, SeekOrigin.Begin);
        }

        _logger.LogInformation(
            "Incoming HTTP Request: {Method} {Path} | Query: {QueryString} | Body: {Body}",
            request.Method,
            request.Path,
            request.QueryString,
            requestBody
        );

        
        var executedContext = await next();
        
        var response = context.HttpContext.Response;
        _logger.LogInformation(
            "Outgoing HTTP Response: With StatusCode {StatusCode} | ContentType: {ContentType}",
            response.StatusCode,
            response.ContentType
        );
    }
}
