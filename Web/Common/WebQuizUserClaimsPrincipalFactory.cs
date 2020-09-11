using System.Security.Claims;
using System.Threading.Tasks;
using Application.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Web.Common
{
    public class WebQuizUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<User>
    {
        public WebQuizUserClaimsPrincipalFactory(UserManager<User> userManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            var roles = await UserManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                identity.AddClaim(new Claim("role", role));
            }

            return identity;
        }
    }
}