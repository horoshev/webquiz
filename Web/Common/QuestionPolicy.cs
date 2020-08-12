using Microsoft.AspNetCore.Authorization;

namespace Web.Common
{
    public static class QuestionPolicy
    {
        public static readonly IAuthorizationRequirement[] Requirements =
        {
            new SameAuthorRequirement()
        };
    }
}