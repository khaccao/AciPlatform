using System.Security.Claims;
using AciPlatform.Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace AciPlatform.Application.Helpers;

public static class HttpContextExtension
{
    public static bool IsLogin(this HttpContext httpContext)
    {
        return httpContext.User.Identity?.IsAuthenticated ?? false;
    }

    public static int GetUserId(this HttpContext httpContext)
    {
        var currentUserId = httpContext.User?.FindFirst(x => x.Type == "UserId")?.Value;
        int.TryParse(currentUserId, out int userId);
        return userId;
    }

    public static string? GetUserName(this HttpContext httpContext)
    {
        return httpContext.User?.FindFirst(x => x.Type == ClaimTypes.Name)?.Value;
    }

    public static IdentityUser GetIdentityUser(this HttpContext httpContext)
    {
        return new IdentityUser
        {
            Id = httpContext.GetClaim("UserId", 0),
            UserName = httpContext.User?.Identity?.Name ?? string.Empty,
            Role = httpContext.GetClaim("RoleName", string.Empty)!,
            FullName = httpContext.GetClaim("FullName", string.Empty)!,
            CompanyCode = httpContext.GetClaim("CompanyCode", string.Empty)!,
        };
    }

    private static T GetClaim<T>(this HttpContext httpContext, string key, T defaultVal)
    {
        var claim = httpContext.User?.FindFirst(x => x.Type == key)?.Value;

        if (claim == null)
        {
            return defaultVal;
        }

        try
        {
            Type targetType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
            var convertedValue = Convert.ChangeType(claim, targetType);
            return convertedValue != null ? (T)convertedValue : defaultVal;
        }
        catch
        {
            return defaultVal;
        }
    }
}
