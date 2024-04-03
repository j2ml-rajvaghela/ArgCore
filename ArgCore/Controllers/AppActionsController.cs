using ArgCore.Attributes;
using ArgCore.Helpers;
using ArgCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ArgCore.Controllers
{
    [Authorize]
    [AuthorizeUser]
    public class AppActionsController : Controller
    {
        public IActionResult Index()
        {
            var data = new AppActions();
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                data.CommonObjects.TopHeading = "App Actions";
                data.CommonObjects.Heading = "App Actions";

                var appActions = Common.AppActions.GetAppActions();
                if (appActions != null && appActions.Any())
                {
                    data.AppActionsList = appActions;
                }

            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                Common.Log.Error(ex);
            }
            return View(data);
        }

        public IActionResult Assign(string roleId)
        {
            var data = new AppActions();
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                data.CommonObjects.TopHeading = "Assign App Actions";
                data.CommonObjects.Heading = "Assign App Actions";
                data.RoleId = Convert.ToString(roleId);

                var appActions = Common.AppActions.GetAppActions();
                if (appActions != null && appActions.Any())
                {
                    data.AppActionsList = appActions.OrderBy(z => z.ActionName).ToList();
                }

                var assignedActions = Common.AppActions.GetActionRoleRels(data.RoleId);
                if (assignedActions != null && assignedActions.Any())
                {
                    data.AssignedActions = assignedActions.OrderBy(x => x.ActionName).ToList();
                    data.AppActionsList.RemoveAll(item => data.AssignedActions.Exists(y => y.AppActionId == item.AppActionId));
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                Common.Log.Error(ex);
            }
            return View(data);
        }

        [HttpPost]
        public void AssignAppAction(string roleId, int appActionId)
        {
            try
            {
                Common.AppActionRoleRels.AssignAppAction(roleId, appActionId);
                Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Saved, 0, "App Action RoleRels");
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
        }

        [HttpPost]
        public void RemoveAssignedAppAction(string roleId, int appActionId)
        {
            try
            {
                Common.AppActionRoleRels.RemoveAssignedAppAction(roleId, appActionId);
                Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Deleted, 0, "App Action RoleRels");
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
        }

        [HttpGet]
        public IActionResult Save(int? appActionId)
        {
            var appActions = new AppActions();
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                appActions.CommonObjects.TopHeading = "App Actions";
                appActions.CommonObjects.Heading = "App Actions";
                appActions.AppActionDetail = new Arg.DataModels.AppActions();

                var _appActionId = Convert.ToInt32(appActionId);
                if (_appActionId > 0)
                {
                    appActions.CommonObjects.Heading = "Edit App Action";
                    appActions.AppActionDetail = Common.AppActions.GetAppAction(_appActionId, "");
                    if (appActions.AppActionDetail == null || appActions.AppActionDetail.AppActionId <= 0)
                    {
                        return RedirectToAction("Index", new { m = "App Actions not found or deleted" });
                    }

                }
                else
                {
                    appActions.CommonObjects.Heading = "Add App Action";
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return View(appActions);
        }

        [HttpPost]
        public IActionResult Save(AppActions appActions)
        {
            try
            {
                Common.AppActions.SaveAppAction(appActions.AppActionDetail);
                if (appActions.AppActionDetail.AppActionId > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Saved, 0, "App Actions");
                    RedirectToAction("Index", "AppActions");
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return RedirectToAction("Index","AppActions");
        }

        public IActionResult Delete(int appActionId)
        {
            try
            {
                var result = Common.AppActions.DeleteAppAction(appActionId);
                if (result > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Deleted, 0, "App Actions");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return RedirectToAction("Index");
        }
    }
}
