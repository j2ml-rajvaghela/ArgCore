using ArgCore.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace ArgCore.Attributes
{
    public class AuthorizeUserAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public string MenuLink { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var isAuthorized = context.HttpContext.User.Identity.IsAuthenticated && context.HttpContext.User.Claims.Any(c => c.Type == ClaimTypes.Role);
            if (!isAuthorized)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (string.IsNullOrWhiteSpace(MenuLink))
                MenuLink = context.HttpContext.Request.Path;

            var currentUserHasAccessToMenuItem = Common.MenuItems.CurrentUserHasAccessToMenuItem(Common.CurrentUserRoleId, MenuLink);

            if (!currentUserHasAccessToMenuItem)
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}
