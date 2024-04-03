using Arg.DataModels;
using ArgCore.Attributes;
using ArgCore.Helpers;
using ArgCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArgCore.Controllers
{
    [Authorize]
    [AuthorizeUser]
    public class BdErrorCodesController : Controller
    {
        public IActionResult Index(string q)
        {
            var bdErrorCodes = new Models.BdErrorCodes();
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                bdErrorCodes.CommonObjects.TopHeading = "Error Codes";
                bdErrorCodes.CommonObjects.Heading = "Error Codes";
                var companies = Common.ArgClients.GetArgClients(Common.CurrentUserId);
                bdErrorCodes.Companies = new SelectList(companies, "CompanyId", "Name");
                bdErrorCodes.SearchOptions = new SearchOptions();
                var errorCodes = Common.BDErrorCodes.GetErrorCodes(0);
                if (errorCodes != null && errorCodes.Any())
                {
                    bdErrorCodes.ErrorCodes = errorCodes;
                }

            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return View(bdErrorCodes);
        }

        [HttpPost]
        public IActionResult Index(Models.BdErrorCodes bdErrorCodesModel)
        {
            try
            {
                var errorCodes = new List<Arg.DataModels.BdErrorCodes>();
                var companies = Common.ArgClients.GetArgClients(Common.CurrentUserId);
                bdErrorCodesModel.Companies = new SelectList(companies, "CompanyId", "Name");
                if (bdErrorCodesModel.SearchOptions != null)
                {
                    errorCodes = Common.BDErrorCodes.GetErrorCodes(bdErrorCodesModel.SearchOptions.CompanyId);
                }
                if (errorCodes != null && errorCodes.Any())
                {
                    bdErrorCodesModel.ErrorCodes = errorCodes;
                }
                else
                {
                    bdErrorCodesModel.SearchOptions.Message = "No results found related to your search!";
                }
                return View(bdErrorCodesModel);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return null;
        }

        [HttpGet]
        public IActionResult Save(int? errorCodeId)
        {
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                var bdErrorCodes = new Models.BdErrorCodes();
                bdErrorCodes.CommonObjects.TopHeading = "Error Codes";

                var companies = Common.ArgClients.GetArgClients(Common.CurrentUserId);

                bdErrorCodes.Companies = new SelectList(companies, "CompanyId", "Name");
                bdErrorCodes.ErrorCodeDetail = new Arg.DataModels.BdErrorCodes();

                var _errorCodeId = Convert.ToInt32(errorCodeId);
                if (errorCodeId > 0)
                {
                    bdErrorCodes.CommonObjects.Heading = "Edit Error Code";
                    bdErrorCodes.ErrorCodeDetail = Common.BDErrorCodes.GetErrorCode(_errorCodeId, 0);
                    if (bdErrorCodes.ErrorCodeDetail == null || bdErrorCodes.ErrorCodeDetail.ErrorCodeId <= 0)
                    {
                        return RedirectToAction("BdErrorCodes", new { m = "Error Codes not found or deleted" });
                    }

                }
                else
                {
                    bdErrorCodes.CommonObjects.Heading = "Add BD Error Code";
                }
                return View(bdErrorCodes);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return null;
        }

        [HttpPost]
        public IActionResult Save(Models.BdErrorCodes errorCodes)
        {
            try
            {
                var ErrorCodesExist = Common.BDErrorCodes.ErrorCodesExist(errorCodes.ErrorCodeDetail.CompanyId, errorCodes.ErrorCodeDetail.BdErrorCode, errorCodes.ErrorCodeDetail.ErrorCodeId);
                if (ErrorCodesExist.Count > 0)
                {
                    var companies = Common.ArgClients.GetArgClients(Common.CurrentUserId);
                    errorCodes.Companies = new SelectList(companies, "CompanyId", "Name", errorCodes.ErrorCodeDetail.CompanyId);
                    errorCodes.ErrorMessage = "Error Code Already Exist";
                    return View(errorCodes);
                }
                Common.BDErrorCodes.SaveBdErrorCode(errorCodes.ErrorCodeDetail);
                if (errorCodes.ErrorCodeDetail.ErrorCodeId > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Saved, Common.GetActiveClientId(), "BdErrorCodes");
                    RedirectToAction("Index", "BdErrorCodes");
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return RedirectToAction("Index", "BdErrorCodes");
        }

        public IActionResult Delete(int errorCodeId)
        {
            try
            {
                var result = Common.BDErrorCodes.DeleteBdErrorCode(errorCodeId);
                if (result > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Deleted, Common.GetActiveClientId(), "BD Error Codes");
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
