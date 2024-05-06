using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace NUServer.Utils.Identity
{
    public static class Extensions
    {
        public static Guid? GetUserId(this ClaimsPrincipal claims)
            => Guid.TryParse(claims.FindFirstValue(ClaimTypes.NameIdentifier), out var result) ? result : null;
    }
}
