using Arg.DataModels;
using ArgCore.Attributes;
using ArgCore.Helpers;
using ArgCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArgCore.Controllers
{
    //[Authorize]
    //[AuthorizeUser]
    public class CommissionsController : Controller
    {
        public IActionResult Index(int? companyId)
        {

            var commissions = new Models.Commissions();
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                commissions.CompanyId = Arg.DataAccess.ActiveClient.Info.CompanyId;
                //data.CompanyId = Convert.ToInt32(companyId);

                if (commissions.SearchOptions != null && commissions.CompanyId > 0)
                {
                    commissions.SearchOptions.CompanyId = commissions.CompanyId;
                }

                LoadCommissionsData(commissions);

                commissions.CommissionDetail = new Arg.DataModels.Commissions();
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return View(commissions);
        }

        [HttpPost]
        public IActionResult Index(Models.Commissions model)
        {
            try
            {
                var data = model;
                model.CompanyId = Arg.DataAccess.ActiveClient.Info.CompanyId;

                LoadCommissionsData(model);

                if (data.CompanyId > 0)
                {
                    data.SearchOptions.CompanyId = data.CompanyId;
                }

                var currentUserId = Common.CurrentUserId;
                //bool argManager = false;
                ////if (Common.CanRunAction.GetCommissionsForArgManager)
                //if (Common.CurrentUserInfo.IsARGManager)
                //{
                //    argManager = true;
                //    currentUserId = Common.CurrentUserId;
                //}
                ////Commission screen (SalesAnalyst): Allows albertobenki@argglobal.net to search All companies, even though he doesn't have access to ANY companies.  Also, he should only be allowed to see his commissions....only ARGManagers can see all commissions assigned to their companies.
                //if (Common.CurrentUserInfo.IsARGAnalyst || Common.CurrentUserInfo.IsARGSalesAnalyst)
                //{
                //    currentUserId = Common.CurrentUserId;
                //}
                List<Arg.DataModels.Commissions> commissions;
                //if (commissionsModel.SearchOptions != null)
                //{
                //    commissions = Common.Commissions.GetCommissions(commissionsModel.SearchOptions, currentUserId, argManager);
                //}
                //else
                commissions = Common.Commissions.GetCommissions(model.SearchOptions ?? new SearchOptions(), currentUserId);
                if (commissions != null && commissions.Any())
                {
                    data.CommissionsList = commissions;

                    decimal totalCommissions = data.CommissionsList.Select(x => x.AmountDueUSD).Sum();
                    data.TotalCommissions = String.Format("{0:n}", totalCommissions);

                    decimal totalCommissionsPaid = data.CommissionsList.Where(x => x.InvoiceStatus == "Closed").Select(x => x.AmountDueUSD).Sum();
                    data.TotalCommissionsPaid = String.Format("{0:n}", totalCommissionsPaid);

                    decimal totalAmountDue = data.CommissionsList.Where(x => x.InvoiceStatus == "Open").Select(x => x.AmountDueUSD).Sum();
                    data.TotalAmountDue = String.Format("{0:n}", totalAmountDue);
                }
                else
                {
                    data.Message = "No results found related to your search!";
                }
                return View(data);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return null;
        }

        public void LoadCommissionsData(Models.Commissions commissions)
        {
            commissions.CommonObjects.TopHeading = "Commissions";
            commissions.CommonObjects.Heading = "Commissions";

            if (commissions.SearchOptions == null)
            {
                commissions.SearchOptions = new SearchOptions();
            }

            var assignedRoles = new List<AspNetRoles>();
            var allRoles = Common.AspNetRoles.GetActivityUserRoles();

            if (commissions.SearchOptions.Roles == null)
            {
                commissions.SearchOptions.Roles = new List<string>();

                //roles.OrderBy(i => i.Name), "Id", "Name"

                if (Common.CanRunAction.ViewOtherUserRoleCommissionsClientManager)
                {
                    commissions.SearchOptions.Roles.Add(Common.AppActions.GetRoleAssignedToAction("Client Manager").ToString());
                    if (allRoles.Any(role => role.Name != null && role.Name.Contains("Client")))
                    {
                        assignedRoles.Add(allRoles.First(x => x.Name.Contains("Client")));
                    }
                }

                if (Common.CanRunAction.ViewOtherUserRoleCommissionsARGSalesAnalyst)
                {
                    commissions.SearchOptions.Roles.Add(Common.AppActions.GetRoleAssignedToAction("ARGSalesAnalyst").ToString());
                    if (allRoles.Any(role => role.Name != null && role.Name.Contains("ARGSales")))
                    {
                        assignedRoles.Add(allRoles.First(x => x.Name.Contains("ARGSales")));
                    }
                }

                if (Common.CanRunAction.ViewOtherUserRoleCommissionsARGManager)
                {
                    commissions.SearchOptions.Roles.Add(Common.AppActions.GetRoleAssignedToAction("ARGManager").ToString());
                    if (allRoles.Any(role => role.Name != null && role.Name.Contains("ARGMana")))
                    {
                        assignedRoles.Add(allRoles.First(x => x.Name.Contains("ARGMana")));
                    }
                }

                if (Common.CanRunAction.ViewOtherUserRoleCommissionsAuditManager)
                {
                    string auditManagerRole = Common.AppActions.GetRoleAssignedToAction("AuditManager");
                    if (!string.IsNullOrEmpty(auditManagerRole))
                    {
                        commissions.SearchOptions.Roles.Add(auditManagerRole);
                    }
                    //commissions.SearchOptions.Roles.Add(Common.AppActions.GetRoleAssignedToAction("AuditManager").ToString());
                    // assignedRoles.Add(allRoles.First(x => x.Name.Contains("AuditMan")));
                }

                if (Common.CanRunAction.ViewOtherUserRoleCommissionsInfoXRevenueAnalyst)
                {
                    commissions.SearchOptions.Roles.Add(Common.AppActions.GetRoleAssignedToAction("Info-X Revenue Analyst").ToString());
                    if (allRoles.Any(role => role.Name != null && role.Name.Contains("Info-X Reve")))
                    {
                        assignedRoles.Add(allRoles.First(x => x.Name.Contains("Info-X Reve")));
                    }
                }

                if (Common.CanRunAction.ViewOtherUserRoleCommissionsInfoXAuditManage)
                {
                    commissions.SearchOptions.Roles.Add(Common.AppActions.GetRoleAssignedToAction("Info-X Audit Manager").ToString());
                    if (allRoles.Any(role => role.Name != null && role.Name.Contains("Info-X Aud")))
                    {
                        assignedRoles.Add(allRoles.First(x => x.Name.Contains("Info-X Aud")));
                    }
                }

                if (Common.CanRunAction.ViewOtherUserRoleCommissionsRevenueAnalyst)
                {
                    commissions.SearchOptions.Roles.Add(Common.AppActions.GetRoleAssignedToAction("Revenue Analyst").ToString());
                    if (allRoles.Any(role => role.Name != null && role.Name.Contains("Revenue Ana")))
                    {
                        assignedRoles.Add(allRoles.First(x => x.Name.Contains("Revenue Ana")));
                    }
                }
            }
            else
            {
                foreach (string item in commissions.SearchOptions.Roles)
                {
                    assignedRoles.Add(allRoles.First(x => x.Id == item));
                }
            }
            //else
            //{
            //    foreach (var item in data.SearchOptions.Roles)
            //    {
            //        assignedRoles.AddRange(item);
            //    }

            //}
            var _companyId = Convert.ToInt32(commissions.CompanyId);

            var regions = Common.Regions.GetRegions(0, "", _companyId);
            commissions.Regions = new SelectList(regions, "Region", "Region");

            var invNos = Common.Commissions.GetDistinctInvoiceNos(_companyId);
            commissions.InvoiceNos = new SelectList(invNos, "InvoiceNo", "InvoiceNo");

            var companies = Common.ArgClients.GetArgClients(Common.CurrentUserId);
            commissions.Companies = new SelectList(companies, "CompanyId", "Name", commissions.CompanyId);

            var currentUserId = Common.CurrentUserId;

            commissions.UserRoles = new SelectList(assignedRoles.OrderBy(i => i.Name), "Id", "Name");

            // bool argManager = false;
            //if (Common.CurrentUserInfo.IsARGManager)
            //{
            //    argManager = true;
            //    currentUserId = Common.CurrentUserId;
            //}
            var users = Common.Commissions.GetCommissionUsers(currentUserId);
            if (assignedRoles.Count > 0)
            {
                users = users.Where(x => assignedRoles.Any(i => i.Name == x.RoleName)).ToList();
            }

            // var selUsers = users.Where(x=>x.RoleName IN (assignedRoles))
            commissions.Users = new SelectList(users.OrderBy(x => x.FullName), "Id", "FullName");

            commissions.InvoiceStatus = Common.StatusOptions.Select((r, index) => new SelectListItem { Text = r, Value = r });
        }

        [HttpGet]
        public IActionResult Save(int? commisionId, int? companyId)
        {
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                var commissions = new Models.Commissions();

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
        public IActionResult Save(Models.Commissions commissions)
        {
            try
            {
                commissions.CommissionDetail.UserId = Common.CurrentUserId;
                Common.Commissions.SaveCommission(commissions.CommissionDetail);

                if (commissions.CommissionDetail.CommissionId > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Saved, commissions.CommissionDetail.CompanyId, "Commissions");
                    RedirectToAction("Index", "Commissions");
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return RedirectToAction("Index", "Commissions");
        }

        public IActionResult Delete(int commissionId)
        {
            try
            {
                var result = Common.Commissions.DeleteCommission(commissionId);
                if (result > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Deleted, Common.GetActiveClientId(), "Commissions");
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
