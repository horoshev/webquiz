using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Web.Common
{
    public class UserPolicy
    {
        public static readonly IAuthorizationRequirement[] Requirements =
        {
            new DenyAnonymousAuthorizationRequirement()
        };
    }
}