using ArgCore.Helpers;
using ArgCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArgCore.Controllers
{
    //[Authorize]
    //[AuthorizeUser]
    public class BdOtherChargeCodesController : Controller
    {
        private List<object> GetChargeCodesWithDescriptions(IEnumerable<Arg.DataModels.BdOtherChargeCodes> chargeCodes)
        {
            return chargeCodes.Select(code => new
            {
                code.BDOtherChargeCodeId,
                code.CompanyId,
                code.ChargeCode,
                code.Description,
                ShortDescription = $"{code.ChargeCode} - {code.Description.Substring(0, Math.Min(20, code.Description.Length))}",
                code.Company,
                code.ChargeCodes
            }).Cast<object>().ToList();
        }

        public ActionResult Index(string q)
        {
            var bdOtherChargeCodes = new BdOtherChargeCodes();
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                bdOtherChargeCodes.CommonObjects.TopHeading = "BD Other Charge Codes";
                bdOtherChargeCodes.CommonObjects.Heading = "BD Other Charge Codes";

                var companies = Common.ArgClients.GetArgClients(Common.CurrentUserId);
                bdOtherChargeCodes.Companies = new SelectList(companies, "CompanyId", "Name");

                var chargeCodes = Common.bdOtherChargeCodes.GetOtherChargeCodes(0);
                var chargeCodesWithDescriptions = GetChargeCodesWithDescriptions(chargeCodes);
                bdOtherChargeCodes.ChargeCodesList = new SelectList(chargeCodesWithDescriptions, "BDOtherChargeCodeId", "ShortDescription");

                if (chargeCodes != null && chargeCodes.Any())
                {
                    bdOtherChargeCodes.ChargeCodes = chargeCodes;

                }
                else
                {
                    bdOtherChargeCodes.SearchOptions.Message = "No results found related to your search!";
                }

            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return View(bdOtherChargeCodes);
        }

        [HttpPost]
        public ActionResult Index(BdOtherChargeCodes bdOtherChargeCodesModel)
        {
            try
            {
                var companies = Common.ArgClients.GetArgClients(Common.CurrentUserId);
                bdOtherChargeCodesModel.Companies = new SelectList(companies, "CompanyId", "Name");

                var chargeCodes = Common.bdOtherChargeCodes.GetOtherChargeCodes(0);
                var chargeCodesWithDescriptions = GetChargeCodesWithDescriptions(chargeCodes);

                bdOtherChargeCodesModel.ChargeCodesList = new SelectList(chargeCodesWithDescriptions, "BDOtherChargeCodeId", "ShortDescription");

                if (bdOtherChargeCodesModel.SearchOptions != null)
                {
                    var otherCharges = Common.bdOtherChargeCodes.GetOtherChargeCodes(
                        bdOtherChargeCodesModel.SearchOptions.CompanyId,
                        bdOtherChargeCodesModel.SearchOptions.BDOtherChargeCodeId);

                    bdOtherChargeCodesModel.ChargeCodes = otherCharges.Any() ? otherCharges : null;
                    bdOtherChargeCodesModel.SearchOptions.Message = otherCharges.Any() ? null : "No results found related to your search!";
                }

                if (bdOtherChargeCodesModel.SearchOptions.CompanyId > 0)
                {
                    chargeCodes = Common.bdOtherChargeCodes.GetOtherChargeCodes(bdOtherChargeCodesModel.SearchOptions.CompanyId);
                    chargeCodesWithDescriptions = GetChargeCodesWithDescriptions(chargeCodes);
                    bdOtherChargeCodesModel.ChargeCodesList = new SelectList(chargeCodesWithDescriptions, "BDOtherChargeCodeId", "ShortDescription");
                }

                return View(bdOtherChargeCodesModel);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                // Consider returning an error view or redirecting to an error page
                return View("Error"); // Update with your error handling strategy
            }
        }

        public JsonResult SetOtherChargeCodesByClient(int companyId)
        {
            try
            {
                if (companyId > 0)
                {
                    var bdOtherChargeCodesInfo = Common.bdOtherChargeCodes.GetOtherChargeCodes(companyId);
                    if (bdOtherChargeCodesInfo != null)
                    {
                        var chargeCodesWithDescriptions = GetChargeCodesWithDescriptions(bdOtherChargeCodesInfo);
                        return Json(chargeCodesWithDescriptions);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                return Json("Error");
            }
            return Json(null);
        }

        [HttpGet]
        public ActionResult Save(int? otherChargeCodeId)
        {
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                var bdOtherChargeCodes = new BdOtherChargeCodes();
                bdOtherChargeCodes.CommonObjects.TopHeading = "BD Other Charge Codes";

                var companies = Common.ArgClients.GetArgClients(Common.CurrentUserId);
                bdOtherChargeCodes.Companies = new SelectList(companies, "CompanyId", "Name");
                bdOtherChargeCodes.ChargeCodeDetail = new Arg.DataModels.BdOtherChargeCodes();

                var _otherChargeCodeId = Convert.ToInt32(otherChargeCodeId);
                if (_otherChargeCodeId > 0)
                {
                    bdOtherChargeCodes.CommonObjects.Heading = "Edit Charge Code";
                    bdOtherChargeCodes.ChargeCodeDetail = Common.bdOtherChargeCodes.GetOtherChargeCode(_otherChargeCodeId, 0);
                    if (bdOtherChargeCodes.ChargeCodeDetail == null || bdOtherChargeCodes.ChargeCodeDetail.BDOtherChargeCodeId <= 0)
                    {
                        return RedirectToAction("BDOtherChargeCodes", new { m = "Other Charge Codes not found or deleted" });
                    }

                }
                else
                {
                    bdOtherChargeCodes.CommonObjects.Heading = "Add Charge Code";
                }
                return View(bdOtherChargeCodes);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return null;
        }

        [HttpPost]
        public ActionResult Save(BdOtherChargeCodes chargeCodes)
        {
            try
            {
                var OtherChargeCodeExist = Common.bdOtherChargeCodes.OtherChargeCodeExist(chargeCodes.ChargeCodeDetail.CompanyId, chargeCodes.ChargeCodeDetail.ChargeCode, chargeCodes.ChargeCodeDetail.BDOtherChargeCodeId);
                if (OtherChargeCodeExist.Count > 0)
                {
                    var companies = Common.ArgClients.GetArgClients(Common.CurrentUserId);
                    chargeCodes.Companies = new SelectList(companies, "CompanyId", "Name", chargeCodes.ChargeCodeDetail.CompanyId);
                    chargeCodes.ErrorMessage = "Other Charge Code Already Exist";
                    return View(chargeCodes);
                }

                Common.bdOtherChargeCodes.SaveBdOtherChargeCode(chargeCodes.ChargeCodeDetail);

                if (chargeCodes.ChargeCodeDetail.BDOtherChargeCodeId > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Saved, chargeCodes.ChargeCodeDetail.CompanyId, "BDOtherChargeCodes");
                    RedirectToAction("Index", "BDOtherChargeCodes");
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return RedirectToAction("Index", "BDOtherChargeCodes");
        }

        public ActionResult Delete(int otherChargeCodeId)
        {
            try
            {
                var result = Common.bdOtherChargeCodes.DeleteBdOtherChargeCode(otherChargeCodeId);
                if (result > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Deleted, Common.GetActiveClientId(), "BD Other Charge Codes");
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
