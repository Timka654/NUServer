using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NUServer.Data;
using NUServer.Managers;

namespace NUServer.Utils.Filters
{
    public class PublishSignFilterAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager>();
            var dbContext = context.HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();

            if (!context.HttpContext.Request.Headers.TryGetValue("signToken", out var tokens) ||
                !context.HttpContext.Request.Headers.TryGetValue("uid", out var uids) ||
                !Guid.TryParse(uids.First(), out var uid) ||
                !await userManager.TryPublishSign(dbContext, uid, tokens.First())
                )
            {
                context.Result = new StatusCodeResult(401);
                return;
            }

            await next();
        }
    }
}
