using System.Security.Claims;
using Application.Entities;
using IdentityServer4.Extensions;

namespace Web.Common
{
    public static class ClaimsPrincipalExtension
    {
        // public static User GetAuthor(this ClaimsPrincipal user)
        // {
        //     return user.GetSubjectId()
        // }

        public static string GetSubjectIdentifier(this ClaimsPrincipal user) =>
            user.FindFirstValue(ClaimTypes.NameIdentifier);
        // user.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
    }
}