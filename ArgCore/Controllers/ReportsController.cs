using ArgCore.Attributes;
using ArgCore.Helpers;
using ArgCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Reporting.WebForms;
using System.Diagnostics;

namespace ArgCore.Controllers
{
    [Authorize]
    [AuthorizeUser]
    public class ReportsController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ReportsController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index(string q)
        {
            var reports = new Reports();
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}


                reports.CommonObjects.TopHeading = "Reports";
                reports.CommonObjects.Heading = "Reports";
                var SSISReports = Common.SSISReports.OrderBy(i => i.Text).ToList();
                reports.SSISReports = new SelectList(SSISReports, "Value", "Text");
                var companies = Common.ArgClients.GetArgClients();
                if (companies != null && companies.Any())
                {
                    companies = companies.OrderBy(i => i.Name).ToList();
                }
                reports.Companies = new SelectList(companies, "CompanyId", "Name");
                return View(reports);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return null;
        }

        public string GenerateReport(string client = "", string analyst = "", string reportType = "", string type = "")
        {
            try
            {
                string extension = type == "PDF" ? ".pdf" : ".xlsx";
                string fileName = reportType + "_" + DateTime.Now.Ticks + extension;
                string fullPath = Path.Combine(_hostingEnvironment.WebRootPath, "BalanceDues", fileName);
                string reportPath = "/ARG/" + reportType;

                if (!string.IsNullOrEmpty(client) && reportType == "ARG_Management_Snapshot")
                {
                    var actions = Common.AppActions.GetActionsAssignedToCurrentRole(Common.CurrentUserRoleId, "Reports." + reportType);
                    if (actions != null && actions.Any(x => x.ActionName == "Reports." + reportType))
                    {
                        List<ReportParameter> reportParameters = new List<ReportParameter>
                    {
                        new ReportParameter("Client", client)
                    };
                        type = type == "PDF" ? "PDF" : "EXCELOPENXML";
                        Common.GenerateReport(reportPath, reportParameters, type, fullPath);
                        return fullPath.Replace(Common.MyAppRoot, Common.MyRoot);
                    }
                    else
                    {
                        return "false";
                    }
                }
                else if (!string.IsNullOrEmpty(analyst) && (reportType == "ARG_Pending_Balance_Dues" || reportType == "RevenueByClientAnalyst" || reportType == "Revenue_Analyst_Productivity"))
                {
                    var actions = Common.AppActions.GetActionsAssignedToCurrentRole(Common.CurrentUserRoleId, "Reports." + reportType);
                    if (actions != null && actions.Any(x => x.ActionName == "Reports." + reportType))
                    {
                        List<ReportParameter> reportParameters = new List<ReportParameter>
                    {
                        new ReportParameter("Analyst", analyst)
                    };
                        type = type == "PDF" ? "PDF" : "EXCELOPENXML";
                        Common.GenerateReport(reportPath, reportParameters, type, fullPath);
                        return fullPath.Replace(Common.MyAppRoot, Common.MyRoot);
                    }
                    else
                    {
                        return "false";
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                Common.Log.Error(ex);
            }
            return null;
        }
    }
}
