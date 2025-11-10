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
            // Check if user is authenticated
            if (context.User?.Identity?.IsAuthenticated == true)
            {
                var userName = context.User.Identity.Name;
                var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Example: Log or store this info
                Console.WriteLine($"[AuthMiddleware] {userName} ({userId}) accessed {context.Request.Path}");
            }
            else
            {
                // Optionally, redirect unauthenticated users globally
                if (!context.Request.Path.StartsWithSegments("/Account/Login"))
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
