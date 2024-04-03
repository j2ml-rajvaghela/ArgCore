using ArgCore.Helpers;
using ArgCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArgCore.Controllers
{
    public class ResearchController : Controller
    {
        public IActionResult Index(int? companyId)
        {
            var researchItems = new Models.ResearchItems();
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return Redirect(Common.MyRoot + "Account/Login");

                //}

                //var _companyId = Convert.ToInt32(companyId);
                researchItems.CompanyId = Arg.DataAccess.ActiveClient.Info.CompanyId;
                if (researchItems.SearchOptions != null && researchItems.CompanyId > 0)
                {
                    researchItems.SearchOptions.CompanyId = researchItems.CompanyId;
                }

                LoadResearchData(researchItems);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }

            return View(researchItems);
        }

        public void LoadResearchData(ResearchItems researchItems)
        {
            researchItems.CommonObjects.TopHeading = "Research";
            researchItems.CommonObjects.Heading = "Research Items";
            if (researchItems.SearchOptions == null)
            {
                researchItems.SearchOptions = new Arg.DataModels.SearchOptions();
            }

            //data.CompanyId = arg.DataAccess.ActiveClient.Info.CompanyId;

            var companies = Common.ArgClients.GetArgClients(Common.CurrentUserId);
            researchItems.Companies = new SelectList(companies, "CompanyId", "Name");

            var regions = Common.Regions.GetRegions(0, "", researchItems.CompanyId);
            researchItems.Regions = new SelectList(regions, "Region", "Region");

            var auditor = Common.AspNetUsers.GetAspNetUsers(researchItems.CompanyId);
            researchItems.Auditors = new SelectList(auditor, "Id", "UserName");

            var reasonCodes = Common.ResearchItems.GetResearchReasonCodes(researchItems.CompanyId);
            researchItems.ReasonCodes = new SelectList(reasonCodes, "ResearchReasonCode", "ResearchReasonCode");

            researchItems.Status = Common.StatusOptions.Select((r, index) => new SelectListItem { Text = r, Value = r });
        }

        [HttpPost]
        public IActionResult Index(ResearchItems researchModel)
        {
            var data = researchModel;
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return Redirect(Common.MyRoot + "Account/Login");
                //}

                researchModel.CompanyId = Arg.DataAccess.ActiveClient.Info.CompanyId;

                LoadResearchData(data);

                if (data.CompanyId > 0)
                {
                    data.SearchOptions.CompanyId = data.CompanyId;
                }

                var currentUserId = Common.CurrentUserId;
                //if (Common.CurrentUserInfo.IsARGAnalyst || Common.CurrentUserInfo.IsARGSalesAnalyst)
                //{
                //    currentUserId = Common.CurrentUserId;
                //}
                List<Arg.DataModels.ResearchItems> researchItems;
                if (data.SearchOptions != null)
                {
                    if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("Ceva"))
                    {
                        researchItems = Common.ResearchItems.GetResearchItemsCeva(data.SearchOptions);
                    }
                    else
                    {
                        researchItems = Common.ResearchItems.GetResearchItems(data.SearchOptions);
                    }
                }
                else
                {
                    researchItems = Common.ResearchItems.GetResearchItems(currentUserId, 0);
                }
                if (researchItems != null && researchItems.Any())
                {
                    data.ResearchItemsList = researchItems;
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return View(data);
        }

        [HttpGet]
        public IActionResult Save(int? researchId)
        {
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return Redirect(Common.MyRoot + "Account/Login");
                //}

                var researchItems = new ResearchItems();
                researchItems.CommonObjects.TopHeading = "Research";
                //var customers = Common.Customers.GetCustomers();
                //data.Customers = new SelectList(customers, "CustomerId", "ContactName");

                var companies = Common.ArgClients.GetArgClients(Common.CurrentUserId);
                researchItems.Companies = new SelectList(companies, "CompanyId", "Name");
                researchItems.Status = Common.StatusOptions.Select((r, index) => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = r, Value = r });
                //var statuses = Common.ResearchItems.GetDistinctStatus();//arg.DataAccess.ActiveClient.Info.CompanyId (Confirm CompanyId)
                //data.Status = new SelectList(statuses, "Status", "Status");

                var regions = Common.Regions.GetRegions().DistinctBy(x => x.Region);
                researchItems.Regions = new SelectList(regions, "Region", "Region");

                var auditor = Common.AspNetUsers.GetAspNetUsers(0);
                researchItems.Auditors = new SelectList(auditor, "Id", "UserName");

                var reasonCodes = Common.RSReasonCodes.GetReasonCodes();//pending
                researchItems.ReasonCodes = new SelectList(reasonCodes, "ReasonCode", "ReasonCode");

                //var reasonCodes = Common.ResearchItems.GetResearchReasonCodes(0);//pending
                //data.ReasonCodes = new SelectList(reasonCodes, "ResearchReasonCode", "ResearchReasonCode");

                researchItems.ResearchItemDetail = new Arg.DataModels.ResearchItems();
                var _resId = Convert.ToInt32(researchId);
                if (_resId > 0)
                {
                    researchItems.CommonObjects.Heading = "Edit Research Item";
                    researchItems.ResearchItemDetail = Common.ResearchItems.GetResearchItem(_resId, Common.GetActiveClientId());

                    var userDetails = Common.AspNetUsers.GetAspNetUser(researchItems.ResearchItemDetail.LastModifiedBy);
                    if (userDetails != null && !string.IsNullOrWhiteSpace(userDetails.Id))
                    {
                        if (!string.IsNullOrWhiteSpace(userDetails.FirstName))
                            researchItems.ResearchItemDetail.LastModifiedBy = userDetails.FirstName + " " + userDetails.LastName;
                        else
                            researchItems.ResearchItemDetail.LastModifiedBy = userDetails.UserName;
                    }
                    if (researchItems.ResearchItemDetail == null || researchItems.ResearchItemDetail.ResearchId <= 0)
                    {
                        return RedirectToAction("Research", new { m = "Items not found or deleted" });
                    }
                }
                else
                {
                    researchItems.CommonObjects.Heading = "Add Research Item";
                    var userDetails = Common.AspNetUsers.GetAspNetUser(Common.CurrentUserId);
                    if (userDetails != null && !string.IsNullOrWhiteSpace(userDetails.Id))
                    {
                        if (!string.IsNullOrWhiteSpace(userDetails.FirstName))
                            researchItems.ResearchItemDetail.LastModifiedBy = userDetails.FirstName + " " + userDetails.LastName;
                        else
                            researchItems.ResearchItemDetail.LastModifiedBy = userDetails.UserName;
                    }
                    researchItems.ResearchItemDetail.LastModified = DateTime.UtcNow;
                }
                //data.Companies = new SelectList(Common.Clients.GetClients(), "CompanyId", "CompanyInfo");
                return View(researchItems);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return null;
        }

        [HttpPost]
        public JsonResult Save(ResearchItems researchItems)
        {
            try
            {
                researchItems.ResearchItemDetail.LastModifiedBy = Common.CurrentUserId;
                Common.ResearchItems.SaveResearchItem(researchItems.ResearchItemDetail);

                if (researchItems.ResearchItemDetail.ResearchId > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Saved, researchItems.ResearchItemDetail.CompanyId, "Research Items");
                    return Json(researchItems.ResearchItemDetail.ResearchId) /*JsonRequestBehavior.AllowGet*/;
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return Json("Error") /*JsonRequestBehavior.AllowGet*/;
        }

        public IActionResult Delete(int researchId)
        {
            try
            {
                var result = Common.ResearchItems.DeleteResearchItem(researchId);
                if (result > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Deleted, Common.GetActiveClientId(), "Research Items");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return RedirectToAction("Index");
        }

        public IActionResult AddResearchReasonCode()
        {
            return View();
        }

        public IActionResult StatusDetails(int researchId)
        {
            var researchStatusDetails = new ResearchStatusDetails();
            try
            {
                var researchInfo = Common.ResearchItems.GetResearchItem(researchId, Common.GetActiveClientId());
                if (researchInfo != null && researchInfo.ResearchId > 0)
                {
                    researchStatusDetails.ResearchInfo = researchInfo;
                    researchStatusDetails.Statuses = Common.StatusOptions.Select((r, index) => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = r, Value = r });
                    //var statuses = Common.ResearchItems.GetDistinctStatus();//arg.DataAccess.ActiveClient.Info.CompanyId (Confirm CompanyId)
                    //data.Statuses = new SelectList(statuses, "Status", "Status");
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.ToString());
            }
            return View(researchStatusDetails);
        }

        public class AjaxResult
        {
            public string Message { get; set; }
            public string CodeName { get; set; }
            public SelectList PullReasonCodes { get; set; }
        }

        public JsonResult UpdateStatus(int researchId, string oldStatus, string newStatus)
        {
            var ajaxResult = new AjaxResult();
            try
            {
                var researchInfo = Common.ResearchItems.GetResearchItem(researchId, Common.GetActiveClientId());
                if (oldStatus == "Open" && newStatus == "Closed")
                {
                    researchInfo.Status = newStatus;
                    researchInfo.LastModified = DateTime.Now;
                    researchInfo.LastModifiedBy = Common.CurrentUserId;

                    Common.ResearchItems.SaveResearchItem(researchInfo);

                    if (researchInfo.ResearchId > 0)
                    {
                        Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Updated, researchInfo.CompanyId, "Research Items");
                        ajaxResult.Message = "Status Updated!";
                    }
                }
                else
                {
                    ajaxResult.Message = "You cannot change the status from " + oldStatus + " to " + newStatus;
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                ajaxResult.Message = ex.ToString();
            }
            return Json(ajaxResult);
        }
    }
}
