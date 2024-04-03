using ArgCore.Helpers;
using ArgCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ArgCore.Controllers
{
    [Authorize]
    public class RSReasonCodesController : Controller
    {
        public IActionResult Index()
        {
            var model = new RSReasonCodes();
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                model.CommonObjects.TopHeading = "Research Reason Codes";
                model.CommonObjects.Heading = "Research Reason Codes";

                var rsReasonCodes = Common.RSReasonCodes.GetReasonCodes();
                if (rsReasonCodes != null && rsReasonCodes.Any())
                {
                    model.ReasonCodes = rsReasonCodes;
                }

            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                Common.Log.Error(ex);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Save(int? reasonCodeId)
        {
            var rsReasonCodes = new RSReasonCodes();
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                rsReasonCodes.CommonObjects.TopHeading = "Research Reason Codes";
                rsReasonCodes.CommonObjects.Heading = "Research Reason Codes";
                rsReasonCodes.RSReasonCodeDetail = new Arg.DataModels.RSReasonCodes();

                var _reasonCodeId = Convert.ToInt32(reasonCodeId);
                if (_reasonCodeId > 0)
                {
                    rsReasonCodes.CommonObjects.Heading = "Edit Reason Code";
                    rsReasonCodes.RSReasonCodeDetail = Common.RSReasonCodes.GetReasonCode(_reasonCodeId);
                    if (rsReasonCodes.RSReasonCodeDetail == null || rsReasonCodes.RSReasonCodeDetail.ReasonCodeId <= 0)
                    {
                        return RedirectToAction("Index", new { m = "Research Reason Codes not found or deleted" });
                    }

                }
                else
                {
                    rsReasonCodes.CommonObjects.Heading = "Add Reason Code";
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return View(rsReasonCodes);
        }

        [HttpPost]
        public IActionResult Save(RSReasonCodes rsReasonCodes)
        {
            try
            {
                var rscodeExist = Common.RSReasonCodes.ReasonCodeExist(rsReasonCodes.RSReasonCodeDetail.ReasonCode, rsReasonCodes.RSReasonCodeDetail.ReasonCodeId);
                if (rscodeExist.Count > 0)
                {
                    rsReasonCodes.ErrorMessage = "RS Reason Code Already Exists";
                    return View(rsReasonCodes);
                }

                Common.RSReasonCodes.SaveRSReasonCode(rsReasonCodes.RSReasonCodeDetail);

                if (rsReasonCodes.RSReasonCodeDetail.ReasonCodeId > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Saved, 0, "Research Reason Codes");
                    return RedirectToAction("Index", "RSReasonCodes");
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return RedirectToAction("Index" , "RSReasonCodes");
        }

        public IActionResult Delete(int reasonCodeId)
        {
            try
            {
                var result = Common.RSReasonCodes.DeleteReasonCode(reasonCodeId);
                if (result > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Deleted, 0, "RS Reason Codes");
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
