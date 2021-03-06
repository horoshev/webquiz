﻿using System.Threading.Tasks;
using Application.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Web.Common
{
    public class SameAuthorRequirement : AuthorizationHandler<SameAuthorRequirement, Question>, IAuthorizationRequirement 
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SameAuthorRequirement requirement, Question resource)
        {
            if (context.User.GetSubjectIdentifier() == resource.AuthorId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}