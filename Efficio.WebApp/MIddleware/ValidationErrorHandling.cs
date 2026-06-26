using Efficio.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Efficio.WebApp.Middleware;

/// <summary>
/// Configures automatic ModelState validation to return RestApiErrorResponse format.
/// Call in Program.cs: builder.Services.AddValidationErrorHandling();
/// </summary>
public static class ValidationErrorHandling
{
    public static IServiceCollection AddValidationErrorHandling(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(e => e.Value?.Errors.Count > 0)
                    .ToDictionary(
                        e => e.Key,
                        e => e.Value!.Errors.Select(err => err.ErrorMessage).ToArray()
                    );

                var response = new RestApiErrorResponse
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "Validation failed",
                    Status = 400,
                    Detail = "One or more validation errors occurred",
                    Errors = errors
                };

                return new BadRequestObjectResult(response);
            };
        });

        return services;
    }
}