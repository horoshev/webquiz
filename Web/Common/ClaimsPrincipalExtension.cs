using System.Security.Claims;

namespace Web.Common
{
    public static class ClaimsPrincipalExtension
    {
        public static string GetSubjectIdentifier(this ClaimsPrincipal user) =>
            user.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}