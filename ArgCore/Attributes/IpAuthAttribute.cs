using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using ArgCore.Helpers;

namespace ArgCore.Attributes
{
    public class IpAuthAttribute :  ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            string ip = Arg.DataAccess.Common.GetIPAddress();
            Common.Log.Error(ip);

            int companyId = Common.GetActiveClientId();
            if (companyId == 0)
            {
                if (filterContext.HttpContext.Request.Query.TryGetValue("companyId", out var companyIdValue))
                {
                    if (!string.IsNullOrEmpty(companyIdValue))
                        companyId = Convert.ToInt32(companyIdValue);
                }
            }

            if (!Common.IPAddressRestriction.IsInRange(companyId, ip))
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                    { "action", "AccessDenied" },
                    { "controller", "Account" },
                    { "companyId", companyId }
                    });
            }
        }
    }
}
