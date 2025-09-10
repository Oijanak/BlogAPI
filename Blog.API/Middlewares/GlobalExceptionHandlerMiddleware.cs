using BlogApi.Application.DTOs;
using BlogApi.Application.Exceptions;
using FluentValidation;
using System.Net;
using System.Text.Json;

namespace BlogApi.API.Controllers.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Unhandled exception occurred while processing request {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        ApiErrorResponse response;

        switch (exception)
        {
            case ApiException apiEx:
                context.Response.StatusCode = apiEx.StatusCode;
                response = new ApiErrorResponse
                {
                    StatusCode = apiEx.StatusCode,
                    Message = apiEx.Message
                };

                _logger.LogWarning("API Exception: {Message} (Status: {StatusCode})",
                    apiEx.Message, apiEx.StatusCode);
                break;

            case FluentValidation.ValidationException validationEx:
                int statusCode = 400;

                if (validationEx.Errors.Any(e => e.ErrorCode == "404"))
                    statusCode = 404;

                var validationErrors = validationEx.Errors
                    .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
                    .ToList();

                context.Response.StatusCode = statusCode;

                _logger.LogWarning("Validation failed with errors: {@ValidationErrors}",
                    validationErrors);

                var errorResponse = new
                {
                    StatusCode = statusCode,
                    Message = "Validation failed",
                    Errors = validationErrors
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
                return;

            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response = new ApiErrorResponse
                {
                    StatusCode = 500,
                    Message = "Internal Server Error"
                };

                _logger.LogError(
                    "Unexpected server error occurred: {Message}",
                    exception.Message);
                break;
        }

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }
}
