using BlogApi.Domain.DTOs;
using BlogApi.Domain.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace BlogApi.API.Controllers.Middlewares;
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        ApiErrorResponse response;

        switch (exception)
        {
            case ApiException apiEx:
                context.Response.StatusCode = apiEx.StatusCode;
                response = new ApiErrorResponse{StatusCode=apiEx.StatusCode,Message=apiEx.Message};
                break;

            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response = new ApiErrorResponse { StatusCode = 500, Message="Internal Server Error" };
                break;
        }

        var json = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(json);
    }
}
