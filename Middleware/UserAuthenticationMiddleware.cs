using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace GlassLP.Middleware
{
    public class UserAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public UserAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            var allowsAnonymous = endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null;
            var isApiRequest = context.Request.Path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase);
            var isSwaggerRequest = context.Request.Path.StartsWithSegments("/swagger", StringComparison.OrdinalIgnoreCase);
            var isStaticResource = context.Request.Path.StartsWithSegments("/lib", StringComparison.OrdinalIgnoreCase)
                                   || context.Request.Path.StartsWithSegments("/css", StringComparison.OrdinalIgnoreCase)
                                   || context.Request.Path.StartsWithSegments("/js", StringComparison.OrdinalIgnoreCase)
                                   || context.Request.Path.StartsWithSegments("/images", StringComparison.OrdinalIgnoreCase);

            // Check if user is authenticated
            if (context.User?.Identity?.IsAuthenticated == true)
            {
                var userName = context.User.Identity?.Name;
                var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Example: Log or store this info
                Console.WriteLine($"[AuthMiddleware] {userName} ({userId}) accessed {context.Request.Path}");
            }
            else
            {
                // Skip redirect for API, Swagger, static assets, or endpoints that allow anonymous access
                if (!allowsAnonymous
                    && !isApiRequest
                    && !isSwaggerRequest
                    && !isStaticResource
                    && !context.Request.Path.StartsWithSegments("/Account/Login", StringComparison.OrdinalIgnoreCase))
                {
                    context.Response.Redirect("/Account/Login");
                    return;
                }
            }

            // Continue the request pipeline
            await _next(context);
         }
    }

    // Extension method to easily add it in Program.cs
    public static class UserAuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseUserAuthenticationLogger(this IApplicationBuilder app)
        {
            return app.UseMiddleware<UserAuthenticationMiddleware>();
        }
    }
}
