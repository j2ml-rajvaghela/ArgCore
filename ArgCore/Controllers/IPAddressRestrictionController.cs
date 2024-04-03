using ArgCore.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace ArgCore.Controllers
{
    public class IPAddressRestrictionController : Controller
    {
        public IActionResult Index()
        {
            var iPAddressRestrication = new Models.IPAddressRestriction();

            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                iPAddressRestrication.CommonObjects.TopHeading = "IP Address Restriction";
                iPAddressRestrication.CommonObjects.Heading = "IP Address Restriction";

                var companies = Common.ArgClients.GetArgClients(Common.CurrentUserId);
                iPAddressRestrication.Companies = new SelectList(companies, "CompanyId", "Name");

                iPAddressRestrication.IPAddressRestrictionList = new List<Arg.DataModels.IPAddressRestriction>();
                var ipAddressRestrictionList = Common.IPAddressRestriction.GetIPAddresses(0);

                if (ipAddressRestrictionList != null && ipAddressRestrictionList.Any())
                {
                    iPAddressRestrication.IPAddressRestrictionList = ipAddressRestrictionList;
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }

            return View(iPAddressRestrication);
        }

        [HttpPost]
        public IActionResult Index(Models.IPAddressRestriction iPAddressRestriction)
        {
            iPAddressRestriction.CommonObjects.TopHeading = "IP Address Restriction";
            iPAddressRestriction.CommonObjects.Heading = "IP Address Restriction";

            var clientNames = Common.ArgClients.GetArgClients(Common.CurrentUserId);
            iPAddressRestriction.Companies = new SelectList(clientNames, "CompanyId", "Name");

            if (iPAddressRestriction.IPAddressRestrictionDetail != null)
            {
                iPAddressRestriction.IPAddressRestrictionList = Common.IPAddressRestriction.GetIPAddresses(iPAddressRestriction.IPAddressRestrictionDetail.CompanyId,
                iPAddressRestriction.IPAddressRestrictionDetail.BeginningIp, iPAddressRestriction.IPAddressRestrictionDetail.EndingIp);
            }

            return View(iPAddressRestriction);
        }

        [HttpGet]
        public IActionResult Save(int? iPAddressRestrictionId)
        {
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                var ipAddressRestriction = new Models.IPAddressRestriction();
                ipAddressRestriction.CommonObjects.Heading = "IP Address Restriction";

                var clientNames = Common.ArgClients.GetArgClients(Common.CurrentUserId);
                ipAddressRestriction.Companies = new SelectList(clientNames, "CompanyId", "Name");

                if (iPAddressRestrictionId > 0)
                {
                    ipAddressRestriction.CommonObjects.Heading = "Edit IP Address Restriction";
                    ipAddressRestriction.IPAddressRestrictionDetail = Common.IPAddressRestriction.GetIPAddresses(0, null, null, iPAddressRestrictionId).FirstOrDefault();

                    if (ipAddressRestriction.IPAddressRestrictionDetail == null || ipAddressRestriction.IPAddressRestrictionDetail.IPAddressRestrictionId <= 0)
                    {
                        return RedirectToAction("IPAddressRestriction", new { m = "IP Address Restriction not found or deleted" });
                    }
                }
                else
                {
                    ipAddressRestriction.CommonObjects.Heading = "Add IP Address Restriction";
                    ipAddressRestriction.IPAddressRestrictionDetail = new Arg.DataModels.IPAddressRestriction();
                }

                return View(ipAddressRestriction);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return null;
        }

        [HttpPost]
        public IActionResult Save(Models.IPAddressRestriction iPAddressRestriction)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Arg.DataModels.IPAddressRestriction iPAddress = new Arg.DataModels.IPAddressRestriction
                    {
                        IPAddressRestrictionId = iPAddressRestriction.IPAddressRestrictionDetail.IPAddressRestrictionId,
                        BeginningIp = iPAddressRestriction.IPAddressRestrictionDetail.BeginningIp,
                        EndingIp = iPAddressRestriction.IPAddressRestrictionDetail.EndingIp,
                        CompanyId = iPAddressRestriction.IPAddressRestrictionDetail.CompanyId
                    };

                    Common.IPAddressRestriction.SaveIPAddress(iPAddress);

                    if (iPAddressRestriction.IPAddressRestrictionDetail.CompanyId > 0)
                    {
                        Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Saved, 0, "IP Address Restriction");
                        return RedirectToAction("Index", "IPAddressRestriction");
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }

            return RedirectToAction("Index", "IPAddressRestriction");
        }

        public IActionResult Delete(int iPAddressRestrictionId)
        {
            try
            {
                var result = Common.IPAddressRestriction.DeleteIPAddress(iPAddressRestrictionId);

                if (result > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Deleted, 0, "Templates");
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
    }
}
