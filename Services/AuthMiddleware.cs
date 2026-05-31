using System.Security.Claims;

namespace IdentityApi.Services;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context, TokenService tokens)
    {
        var header = context.Request.Headers.Authorization.FirstOrDefault();
        if (header?.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) == true)
        {
            var token = header["Bearer ".Length..].Trim();
            var principal = tokens.ValidateToken(token);
            if (principal != null)
            {
                // Re-wrap in an authenticated identity so IsAuthenticated == true
                var identity = new ClaimsIdentity(principal.Claims, "Bearer");
                context.User = new ClaimsPrincipal(identity);
            }
        }
        await _next(context);
    }
}
