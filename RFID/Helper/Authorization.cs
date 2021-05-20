using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RFID.Helper
{
    public class Authorization
    {
           
    }

    public class Token: IAuthorizationRequirement
    {

    }

    public class UserTokenHandler: AuthorizationHandler<Token>
    {
        private readonly IHttpContextAccessor contextAccessor = null;

        public UserTokenHandler(IHttpContextAccessor _contextAccessor)
        {
            this.contextAccessor = _contextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, Token requirement)
        {
            var authorizationFilterContext = context.Resource as AuthorizationFilterContext;

            if(!context.User.HasClaim(x => x.Type == "X-Token") || !contextAccessor.HttpContext.Request.Headers.Where(x => x.Key == "X-Token").Any())
            {
                context.Fail();
                return Task.CompletedTask;
            }
            else
            {
                string TokenSession = context.User.FindFirst(x => x.Type == "X-Token").Value;
                string TokenHeader = contextAccessor.HttpContext.Request.Headers.Where(x => x.Key == "X-Token").FirstOrDefault().Value;
                if(TokenSession == TokenHeader)
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }

                return Task.CompletedTask;
            }
        }
    }
   
}
