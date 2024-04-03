using ArgCore.Attributes;
using ArgCore.Helpers;
using ArgCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace ArgCore.Controllers
{
    [AuthorizeUser]
    public class SettingGroupsController : Controller
    {
        public IActionResult Index(string q)
        {
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                var settingGroups = new SettingGroups();
                settingGroups.CommonObjects.TopHeading = "Group Settings";
                settingGroups.SettingGroupsList = new List<Arg.DataModels.SettingGroups>();
                settingGroups.SettingGroupsList = Common.SettingGroups.GetSettingGroups(q);

                return View(settingGroups);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return null;
        }

        [HttpGet]
        public IActionResult Save(int? groupId)
        {
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                var settingGroups = new SettingGroups();
                settingGroups.CommonObjects.TopHeading = "Group Settings";

                var _groupId = Convert.ToInt32(groupId);
                settingGroups.SettingGroupDetail = new Arg.DataModels.SettingGroups();

                if (_groupId > 0)
                {
                    settingGroups.CommonObjects.Heading = "Edit Group Setting";
                    settingGroups.SettingGroupDetail = Common.SettingGroups.GetSettingGroup(_groupId);
                    if (settingGroups.SettingGroupDetail == null || settingGroups.SettingGroupDetail.GroupId <= 0)
                    {
                        return RedirectToAction("Index", new { m = "Setting Group not found or deleted" });
                    }

                }
                else
                {
                    settingGroups.CommonObjects.Heading = "Add Group Setting";
                }
                return View(settingGroups);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return null;
        }

        [HttpPost]
        public IActionResult Save(SettingGroups settingGroup)
        {
            try
            {
                Common.SettingGroups.SaveSettingGroup(settingGroup.SettingGroupDetail);

                if (settingGroup.SettingGroupDetail.GroupId > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Saved, 0, "Setting Groups");
                    return RedirectToAction("Index", "SettingGroups");
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return RedirectToAction("Index", "SettingGroups");
        }

        public IActionResult ShowGroups(int groupId)
        {
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                var settingGroups = new SettingGroups();
                settingGroups.CommonObjects.TopHeading = "Setting Groups";
                settingGroups.SettingsList = new List<Arg.DataModels.Settings>();
                settingGroups.SettingsList = Common.Settings.GetSettings(groupId);

                return View(settingGroups);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return null;
        }

        [HttpPost]
        public IActionResult ShowGroups(List<Arg.DataModels.Settings> setting)
        {
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                if (setting != null && setting.Any())
                {
                    foreach (var s in setting)
                    {
                        var mySetting = Common.Settings.GetSetting(s.SettingId);
                        mySetting.Value = s.Value;
                        Common.Settings.SaveSetting(mySetting);
                    };
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Saved, 0, "SMTP Settings");
                    return RedirectToAction("Index", "Admin");
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return RedirectToAction("Index", "Admin");
            //return Redirect(Common.MyRoot + "SettingGroups/ShowGroups");
        }

        public IActionResult Delete(int groupId)
        {
            try
            {
                var result = Common.SettingGroups.DeleteSettingGroup(groupId);
                if (result > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Deleted, 0, "Setting Groups");
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
