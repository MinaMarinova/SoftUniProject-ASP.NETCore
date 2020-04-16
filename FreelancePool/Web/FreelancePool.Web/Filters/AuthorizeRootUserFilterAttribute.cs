namespace FreelancePool.Web.Filters
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using FreelancePool.Services.Data;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

    public class AuthorizeRootUserFilterAttribute : Attribute, IAsyncActionFilter
    {
        private readonly IConfiguration configuration;
        private readonly IUsersService usersService;

        public AuthorizeRootUserFilterAttribute(
            IConfiguration configuration,
            IUsersService usersService)
        {
            this.configuration = configuration;
            this.usersService = usersService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userEmail = this.usersService.GetUserEmailById(userId);

            if (userEmail != this.configuration["RootUser:Email"])
            {
                context.Result = new ForbidResult();
            }
            else
            {
                await next();
            }
        }
    }
}
