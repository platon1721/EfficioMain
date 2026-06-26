using System.Text.Json;
using Efficio.BLL.Contracts.Exceptions;
using Efficio.DTO;

namespace Efficio.WebApp.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (BusinessException ex)
        {
            await HandleBusinessExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");
            await HandleUnexpectedExceptionAsync(context, ex);
        }
    }

    private static async Task HandleBusinessExceptionAsync(HttpContext context, BusinessException ex)
    {
        context.Response.StatusCode = ex.StatusCode;
        context.Response.ContentType = "application/json";

        var response = new RestApiErrorResponse
        {
            Type = GetRfcType(ex.StatusCode),
            Title = GetTitle(ex.StatusCode),
            Status = ex.StatusCode,
            Detail = ex.Message
        };

        if (ex is ValidationException validationEx && validationEx.Errors != null)
        {
            response.Errors = validationEx.Errors;
        }

        await WriteResponseAsync(context, response);
    }

    private static async Task HandleUnexpectedExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        var response = new RestApiErrorResponse
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Title = "Internal server error",
            Status = 500,
#if DEBUG
            Detail = ex.Message
#else
            Detail = "An unexpected error occurred"
#endif
        };

        await WriteResponseAsync(context, response);
    }

    private static string GetRfcType(int statusCode) => statusCode switch
    {
        400 => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
        401 => "https://tools.ietf.org/html/rfc7235#section-3.1",
        403 => "https://tools.ietf.org/html/rfc7231#section-6.5.3",
        404 => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
        409 => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
        _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
    };

    private static string GetTitle(int statusCode) => statusCode switch
    {
        400 => "Bad request",
        401 => "Unauthorized",
        403 => "Forbidden",
        404 => "Not found",
        409 => "Conflict",
        _ => "Error"
    };

    private static async Task WriteResponseAsync(HttpContext context, RestApiErrorResponse response)
    {
        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        await context.Response.WriteAsync(json);
    }
}