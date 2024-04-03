using Arg.DataModels;
using ArgCore.Attributes;
using ArgCore.Helpers;
using ArgCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArgCore.Controllers
{
    //[Authorize]
    public class ActivityStatsController : Controller
    {
        //[AuthorizeUser]
        public ActionResult Index()
        {
            var activityStats = new Models.ActivityStats();
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                LoadData(activityStats);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return View(activityStats);
        }

        public void LoadData(Models.ActivityStats activityStats)
        {
            activityStats.CommonObjects.TopHeading = "User Audit Trail";
            activityStats.CommonObjects.Heading = "User Audit Trail";

            if (activityStats.SearchOptions == null)
            {
                activityStats.SearchOptions = new SearchOptions();
            }

            var clients = Common.ArgClients.GetArgClients(Common.CurrentUserId);
            activityStats.Clients = new SelectList(clients, "CompanyId", "Name");

            var webPages = Common.ActivityStats.GetActivityStatWebPages(Common.GetActiveClientId());
            activityStats.WebPages = new SelectList(webPages.OrderBy(i => i.WebPage), "WebPage", "WebPage");

            var ipAddress = Common.ActivityStats.GetActivityStatIpAddress(Common.GetActiveClientId());
            activityStats.IPAddresses = new SelectList(ipAddress.OrderBy(i => i.IpAddress), "IpAddress", "IpAddress");

            var currentUserId = "";
            bool argManager = false;
            //if (data.SearchOptions.UserIds == null || !data.SearchOptions.UserIds.Any())
            //{
            //if (Common.CurrentUserInfo.IsARGManager)
            //{
            //    argManager = true;
            //    currentUserId = Common.CurrentUserId;
            //}
            //}
            var users = Common.AspNetUsers.GetActivityUsers(currentUserId, argManager).Where(x => x.FullName.Length > 3);

            activityStats.Users = new SelectList(users.OrderBy(i => i.FullName), "Id", "FullName");

            var roles = Common.AspNetRoles.GetActivityUserRoles();

            activityStats.UserRoles = new SelectList(roles.OrderBy(i => i.Name), "Id", "Name");
        }

        [HttpPost]
        public ActionResult Index(Models.ActivityStats activityStatsModel)
        {
            try
            {
                //TEMP FIX FOR USERS
                //data.SearchOptions.UserIds = data.SearchOptions.UserIds.Where(x => x.Length < 5).ToList();
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                LoadData(activityStatsModel);

                var currentUserId = "";
                bool argManager = false;

                if (Common.CurrentUserInfo.IsARGManager)
                {
                    argManager = true;
                    currentUserId = Common.CurrentUserId;
                }
                List<Arg.DataModels.ActivityStats> activityStats;

                if (activityStatsModel.SearchOptions != null)
                {
                    activityStats = Common.ActivityStats.GetActivityStats(activityStatsModel.SearchOptions, currentUserId, argManager);
                }
                else
                {
                    activityStats = Common.ActivityStats.GetActivityStats(new SearchOptions(), currentUserId, argManager);
                }
                if (activityStats.Any())
                {
                    activityStatsModel.ActivityStatsList = activityStats.Take(500).ToList();
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return View(activityStatsModel);
        }
    }
}
