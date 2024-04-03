using ArgCore.Helpers;
using ArgCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArgCore.Controllers
{
    [Authorize]
    public class CommissionRatesController : Controller
    {
        public IActionResult Index(int? companyId)
        {
            var commissionRates = new CommissionRates();
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return Redirect(Common.MyRoot + "Account/Login");
                //}

                var _companyId = Convert.ToInt32(companyId);
                commissionRates.CompanyId = _companyId;
                //LoadCommissionsData(data);
                commissionRates.CommissionRateDetail = new Arg.DataModels.CommissionRates();
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return View(commissionRates);
        }

        [HttpPost]
        public IActionResult Index(CommissionRates commissionRatesModel)
        {
            try
            {
                var model = commissionRatesModel;
                LoadCommissionsData(commissionRatesModel);
                if (model.CompanyId > 0)
                {
                    model.SearchOptions.CompanyId = model.CompanyId;
                }

                List<Arg.DataModels.CommissionRates> commissionRates;
                //if (commissionRatesModel.SearchOptions != null)
                //{
                //    commissions = Common.Commissions.GetCommissions(commissionRatesModel.SearchOptions);
                //}
                //else
                commissionRates = Common.CommissionRates.GetCommissionRates();
                if (commissionRates != null && commissionRates.Any())
                {
                    model.CommissionRatesList = commissionRates;
                }
                else
                {
                    model.Message = "No results found related to your search!";
                }
                return View(model);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return null;
        }

        public void LoadCommissionsData(CommissionRates commissionRates)
        {
            var _companyId = Convert.ToInt32(commissionRates.CompanyId);

            commissionRates.CommonObjects.TopHeading = "Commissions";
            commissionRates.CommonObjects.Heading = "Commissions";

            var regions = Common.Regions.GetRegions(0, "", _companyId);
            commissionRates.Regions = new SelectList(regions, "Region", "Region");

            var invNos = Common.Commissions.GetDistinctInvoiceNos(_companyId);
            commissionRates.InvoiceNos = new SelectList(invNos, "InvoiceNo", "InvoiceNo");

            var companies = Common.ArgClients.GetArgClients(Common.CurrentUserId);
            commissionRates.Companies = new SelectList(companies, "CompanyId", "Name");

            var users = Common.AspNetUsers.GetAspNetUsers(false);

            commissionRates.Users = new SelectList(users.OrderBy(x => x.UserName), "Id", "UserName");
            commissionRates.InvoiceStatus = Common.StatusOptions.Select((r, index) => new SelectListItem { Text = r, Value = r });
        }

        [HttpGet]
        public IActionResult Save(int? commisionId, int? companyId)
        {
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return Redirect(Common.MyRoot + "Account/Login");
                //}

                var commissions = new Commissions();

                var _companyId = Convert.ToInt32(companyId);

                commissions.CommonObjects.TopHeading = "Commissions";
                commissions.CommissionDetail = new Arg.DataModels.Commissions();

                var companies = Common.ArgClients.GetArgClients(Common.CurrentUserId);
                commissions.Companies = new SelectList(companies, "CompanyId", "Name");

                var customers = Common.Customers.GetCustomers();
                commissions.Customers = new SelectList(customers, "CustomerId", "CustomerName");

                var regions = Common.Regions.GetRegions(0, "", _companyId);
                commissions.Regions = new SelectList(regions, "Region", "Region");

                if (_companyId > 0)
                {
                    commissions.CommissionDetail.CompanyId = _companyId;
                }

                var _commissionId = Convert.ToInt32(commisionId);
                if (_commissionId > 0)
                {
                    commissions.CommonObjects.Heading = "Edit Commission";
                    commissions.CommissionDetail = Common.Commissions.GetCommission(_commissionId);
                    if (commissions.CommissionDetail == null || commissions.CommissionDetail.CommissionId <= 0)
                    {
                        return RedirectToAction("Index", new { m = "Commissions not found or deleted" });
                    }
                }
                else
                {
                    commissions.CommonObjects.Heading = "Add Commission";
                }
                return View(commissions);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return null;
        }

        [HttpPost]
        public IActionResult Save(Commissions commissions)
        {
            try
            {
                commissions.CommissionDetail.UserId = Common.CurrentUserId;
                Common.Commissions.SaveCommission(commissions.CommissionDetail);
                if (commissions.CommissionDetail.CommissionId > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Saved, commissions.CommissionDetail.CompanyId, "Commissions");
                    Redirect(Common.MyRoot + "Commissions/Index");
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return Redirect(Common.MyRoot + "Commissions/Index");
        }

        public IActionResult Delete(int commissionId)
        {
            try
            {
                var result = Common.Commissions.DeleteCommission(commissionId);
                if (result > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Deleted, Common.GetActiveClientId(), "Commission Rates");
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

