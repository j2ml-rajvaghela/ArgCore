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
    public class RegionsController : Controller
    {
        public IActionResult Index(string q)
        {

            var model = new Regions();
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                model.CommonObjects.TopHeading = "Regions";

                var regions = Common.Regions.GetRegions(0, (!string.IsNullOrWhiteSpace(q) ? q : ""), 0);
                if (regions != null && regions.Any())
                {
                    model.RegionsList = regions;
                    var Distregions = Common.Regions.GetDistictRegions();
                    model.RegionsDropdownList = new SelectList(Distregions, "Region", "Region");
                }
                var companies = Common.Regions.GetRegionClients();
                model.Companies = new SelectList(companies, "CompanyId", "Company");

            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Save(int? regionId)
        {
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                var regions = new Regions();
                regions.CommonObjects.TopHeading = "Regions";

                var customers = Common.Customers.GetCustomers();
                regions.Customers = new SelectList(customers, "CustomerId", "ContactName");

                var companies = Common.ArgClients.GetArgClients(Common.CurrentUserId);
                regions.Companies = new SelectList(companies, "CompanyId", "Name");
                regions.RegionDetail = new Arg.DataModels.Regions();

                var _regionId = Convert.ToInt32(regionId);
                if (_regionId > 0)
                {
                    regions.CommonObjects.Heading = "Edit Region";
                    regions.RegionDetail = Common.Regions.GetRegion(_regionId, 0);
                    if (regions.RegionDetail == null || regions.RegionDetail.RegionId <= 0)
                    {
                        return RedirectToAction("Regions", new { m = "Regions not found or deleted" });
                    }

                }
                else
                {
                    regions.CommonObjects.Heading = "Add Region";
                }
                return View(regions);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return null;
        }

        [HttpPost]
        public IActionResult Save(Regions regions)
        {
            try
            {
                var regionExist = Common.Regions.RegionExist(regions.RegionDetail.CompanyId, regions.RegionDetail.Region, regions.RegionDetail.RegionId);
                if (regionExist.Count > 0)
                {
                    var companies = Common.ArgClients.GetArgClients(Common.CurrentUserId);
                    regions.Companies = new SelectList(companies, "CompanyId", "Name", regions.RegionDetail.CompanyId);
                    regions.ErrorMessage = "Region already exists for this client";
                    return View(regions);
                }

                Common.Regions.SaveRegion(regions.RegionDetail);

                if (regions.RegionDetail.RegionId > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Saved, regions.RegionDetail.CompanyId, "Regions");
                    RedirectToAction("Index", "Regions");
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }

           return RedirectToAction("Index", "Regions");
        }

        public IActionResult Delete(int regionId)
        {
            try
            {
                var result = Common.Regions.DeleteRegion(regionId);
                if (result > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Deleted, Common.GetActiveClientId(), "Regions");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return RedirectToAction("Index");
        }

        public PartialViewResult LoadRegions(string clientId = "", string region = "", bool isGetRegion = false)
        {
            var data = new Regions();
            if (!isGetRegion)
            {
                try
                {
                    if (!string.IsNullOrEmpty(clientId) && !string.IsNullOrEmpty(region))
                    {
                        var regions = Common.Regions.GetRegions(0, region, Convert.ToInt32(clientId));
                        if (regions != null && regions.Any())
                        {
                            data.RegionsList = regions;
                        }
                    }
                    else if (!string.IsNullOrEmpty(clientId))
                    {
                        var regions = Common.Regions.GetRegions(0, region, Convert.ToInt32(clientId));
                        if (regions != null && regions.Any())
                        {
                            data.RegionsList = regions;
                        }
                    }
                    else if (!string.IsNullOrEmpty(region))
                    {
                        var regions = Common.Regions.GetRegions(0, region, 0);
                        if (regions != null && regions.Any())
                        {
                            data.RegionsList = regions;
                        }
                    }
                    else
                    {
                        var regions = Common.Regions.GetRegions(0, "", 0);
                        if (regions != null && regions.Any())
                        {
                            data.RegionsList = regions;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Common.Log.Error(ex);
                }
            }
            else
            {
                data.IsGetRegion = true;
                if (string.IsNullOrEmpty(clientId))
                {
                    var Distregions = Common.Regions.GetDistictRegions();
                    data.RegionsDropdownList = new SelectList(Distregions, "Region", "Region");
                }
                else
                {
                    var regions = Common.Regions.GetRegions(0, "", Convert.ToInt32(clientId));
                    if (regions != null && regions.Any())
                    {
                        data.RegionsDropdownList = new SelectList(regions, "Region", "Region");
                    }
                }
            }
            return PartialView(data);
        }
    }
}
