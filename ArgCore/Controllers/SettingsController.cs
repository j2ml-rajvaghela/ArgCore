using ArgCore.Helpers;
using ArgCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace ArgCore.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        public IActionResult Index()
        {
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                var settings = new Settings();
                settings.CommonObjects.TopHeading = "Settings";
                settings.SettingsList = new List<Arg.DataModels.Settings>();
                settings.SettingsList = Common.Settings.GetSettings(0);

                return View(settings);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return null;
        }

        [HttpGet]
        public IActionResult Save(int? settingId)
        {
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                var settings = new Settings();
                settings.CommonObjects.TopHeading = "Settings";

                var _settingId = Convert.ToInt32(settingId);
                settings.SettingDetail = new Arg.DataModels.Settings();

                if (_settingId > 0)
                {
                    settings.CommonObjects.Heading = "Edit Setting";
                    settings.SettingDetail = Common.Settings.GetSetting(_settingId);
                    if (settings.SettingDetail == null || settings.SettingDetail.SettingId <= 0)
                    {
                        return RedirectToAction("Index", new { m = "Setting not found or deleted" });
                    }
                }
                else
                {
                    settings.CommonObjects.Heading = "Add Setting";
                }

                settings.SettingGroups = new SelectList(Common.SettingGroups.GetSettingGroups(""), "GroupId", "Name");

                return View(settings);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return null;
        }

        [HttpPost]
        public IActionResult Save(Settings setting)
        {
            try
            {
                setting.SettingDetail.GroupId = (setting.SettingDetail.GroupId > 0 ? setting.SettingDetail.GroupId : 0);

                Common.Settings.SaveSetting(setting.SettingDetail);

                if (setting.SettingDetail.SettingId > 0)
                {
                    return RedirectToAction("Index", "Settings");
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return RedirectToAction( "Index","Settings");
        }

        public IActionResult Delete(int settingId)
        {
            try
            {
                var result = Common.Settings.DeleteSetting(settingId);
                if (result > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Deleted, 0, "Settings");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AuthSetting()
        {
            var settings = new Settings();
            try
            {
                settings.CommonObjects.TopHeading = "Settings";
                settings.CommonObjects.Heading = "Auth Setting";
                return View(settings);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return View(settings);
        }

        [HttpPost]
        public IActionResult AuthSetting(string keys)
        {
            try
            {
                var url = Common.UserCredential;
                return RedirectToAction("AuthSetting");
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return RedirectToAction("AuthSetting");
        }


    }
}
