using Arg.DataModels;
using ArgCore.Attributes;
using ArgCore.Helpers;
using ArgCore.Models;
using Microsoft.AspNetCore.Mvc;
using NPOI.POIFS.Crypt.Dsig;

namespace ArgCore.Controllers
{
    public class AdminController : _baseMVCController
    {
        public IActionResult Index()
        {
            var dashBoard = new Dashboard();

            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //   return RedirectToAction("LogIn", "Account");
                //}

                dashBoard.CommonObjects.TopHeading = "Dashboard";
                dashBoard.CommonObjects.Heading = "Dashboard";

                //Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.LoggedIn, 0);

                var userId = "";
                bool argManager = false;

                userId = Common.CurrentUserId;
                List<Arg.DataModels.ActivityStats> activityStats = null;
                if (Common.CanRunAction.ClientDashboard)
                {
                    var revenueCollectedPastYear = Common.BalanceDues.GetRevenueCollectedPastYear(Common.CurrentUserId);
                    if (revenueCollectedPastYear != null && revenueCollectedPastYear.Any())
                    {
                        dashBoard.RevenueCollectedPastYear = revenueCollectedPastYear;
                    }

                    var revenueCollectedForYearToDate = Common.BalanceDues.GetRevenueCollectedForYearToDate(Common.CurrentUserId);
                    if (revenueCollectedForYearToDate != null && revenueCollectedForYearToDate.Any())
                    {
                        dashBoard.RevenueCollectedForYearToDate = revenueCollectedForYearToDate;
                    }

                    var openInvoices = Common.ArgInvoicesBD.GetOpenInvoicesForClient(Common.CurrentUserId);
                    if (openInvoices != null && openInvoices.Any())
                    {
                        dashBoard.ClientOpenInvoices = openInvoices;
                    }

                    activityStats = Common.ActivityStats.GetActivityStats(Common.CurrentUserId, null, 0, null, DateTime.MinValue, DateTime.MinValue, null, "LoggedIn");
                    if (activityStats.Any())
                    {
                        dashBoard.ActivityStats = activityStats.Take(20).ToList();
                    }
                }
                else
                {
                    if (Common.CanRunAction.ViewAllWeeklyBalanceDuesCollected)
                    {
                        dashBoard.WeeklyBDSetUp = new List<BalanceDue>();
                        List<BalanceDue> underBillinglist = new List<BalanceDue>();
                        List<BalanceDue> otherBillinglist = new List<BalanceDue>();

                        var weeklyBDSetUp = Common.BalanceDues.GetBDSetUp(Arg.DataAccess.BalanceDuesImpl.EnumDashboard.Weekly, userId);
                        if (weeklyBDSetUp != null && weeklyBDSetUp.Any())
                        {
                            underBillinglist = weeklyBDSetUp.Where(x => x.InvoiceType.Contains("Under-billing")).ToList();
                        }
                        otherBillinglist = weeklyBDSetUp.Where(x => !underBillinglist.Contains(x)).ToList();

                        if (underBillinglist != null && underBillinglist.Any())
                        {
                            dashBoard.WeeklyBDSetUp.AddRange(underBillinglist);
                        }

                        if (otherBillinglist != null && otherBillinglist.Any())
                        {
                            dashBoard.WeeklyBDSetUp.AddRange(otherBillinglist);
                        }
                        var weeklyBDCollected = Common.BalanceDues.GetBDCollected(Arg.DataAccess.BalanceDuesImpl.EnumDashboard.Weekly, userId);
                        if (weeklyBDCollected != null && weeklyBDCollected.Any())
                        {
                            dashBoard.WeeklyBDCollected = weeklyBDCollected;
                        }
                    }
                    activityStats = Common.ActivityStats.GetActivityStats((Common.CurrentUserId), null, 0, null, DateTime.MinValue, DateTime.MinValue, null);
                    if (activityStats.Any())
                    {
                        dashBoard.ActivityStats = activityStats.Take(20).ToList();
                    }
                    if (Common.CanRunAction.ViewAllClientPendingBalanceDues)
                    {
                        dashBoard.PendingBD = Common.BalanceDues.GetPendingBD(userId, "Pending");
                    }
                    if (Common.CanRunAction.ViewAllClientPendingApprovalBalanceDues)
                    {
                        dashBoard.PendingApprovalBD = Common.BalanceDues.GetPendingBD(userId, "Pending_Approval");
                    }
                    if (Common.CanRunAction.ViewOpenInvoices)
                    {
                        var openInvoices = Common.BalanceDues.GetOpenInvoices(userId);
                        if (openInvoices != null && openInvoices.Any())
                        {
                            dashBoard.OpenInvoices = openInvoices;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return View(dashBoard);
        }

        [AuthorizeUser]
        public IActionResult ManageClients()
        {
            return View();
        }

        [AuthorizeUser]
        public IActionResult ManageGroups()
        {
            return View();
        }

        [AuthorizeUser]
        public IActionResult ManageMembers(int id)
        {
            try
            {
                ViewBag.groupid = id;
                return View();
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                return null;
            }
        }

        [AuthorizeUser]
        public IActionResult ManageMemberships(int id)
        {
            // pass requested userid to view data
            try
            {
                ViewBag.userid = id;
                return View();
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                return null;
            }
        }

        [AuthorizeUser]
        public IActionResult ManageRegions()
        {
            return View();
        }

        [AuthorizeUser]
        public IActionResult ManagePermissions(int id = 0)
        {
            // pass requested userid to view data
            try
            {
                ViewBag.clientid = id;
                return View();
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                return null;
            }
        }
    }
}
