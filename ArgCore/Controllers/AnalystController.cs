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
    public class AnalystController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

       [AuthorizeUser]
        public IActionResult Performance()
        {
            var analystPerformance = new AnalystPerformance();
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                analystPerformance.CompanyId = Arg.DataAccess.ActiveClient.Info.CompanyId;
                analystPerformance.CommonObjects.TopHeading = "Analyst";
                analystPerformance.CommonObjects.Heading = "Analyst Performance";

                if (analystPerformance.SearchOptions != null && analystPerformance.CompanyId > 0)
                {
                    analystPerformance.SearchOptions.CompanyId = analystPerformance.CompanyId;
                }

                if (analystPerformance.CompanyId > 0 && Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientCeva"))
                {
                    return RedirectToAction("Performance_v1");
                }

                LoadData(analystPerformance);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return View(analystPerformance);
        }

        [HttpPost]
        public IActionResult Performance(AnalystPerformance analystInfo)
        {
            var data = analystInfo;
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                analystInfo.CompanyId = Arg.DataAccess.ActiveClient.Info.CompanyId;

                if (data.CompanyId > 0)
                {
                    data.SearchOptions.CompanyId = data.CompanyId;
                }

                LoadData(data);

                if (data.SearchOptions != null)
                {
                    data.SearchOptions.UserId = Common.CurrentUserId;
                }

                if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientAgility"))
                {
                    data.AnalystList = Common.ActivityStats.GetAgilityAnalystPerformance(data.SearchOptions);
                }
                else
                {
                    data.AnalystList = Common.ActivityStats.GetAnalystPerformance(data.SearchOptions);
                }

                var detList = data.AnalystList.GroupBy(u => u.ClientName).Select(u => u.FirstOrDefault());
                var random = new Random();
                foreach (var item in data.AnalystList.GroupBy(u => u.ClientName).Select(u => u.FirstOrDefault()))
                {
                    var UserName = item.ClientName;

                    var color = String.Format("#{0:X6}", random.Next(0x1000000)); // = "#A197B9"
                    foreach (var item1 in data.AnalystList.Where(i => i.ClientName == UserName))
                    {
                        item1.Color = color;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return View(data);
        }

        public void LoadData(AnalystPerformance data)
        {
            data.CommonObjects.TopHeading = "Analyst";
            data.CommonObjects.Heading = "Analyst Performance";

            if (data.SearchOptions == null)
            {
                data.SearchOptions = new SearchOptions();
            }
               
            //data.CompanyId = arg.DataAccess.ActiveClient.Info.CompanyId;
            var companies = Common.ArgClients.GetArgClients(Common.CurrentUserId);
            data.Companies = new SelectList(companies, "CompanyId", "Name");

            if (data.CompanyId > 0)
            {
                if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientCeva"))
                {
                    var shipper = new List<Arg.Ceva.DataAccess.BookingHeader.BookingHeaderImp>();
                    shipper = Common.BookingHeaders.GetCustomer();
                    data.Shippers = new SelectList(shipper, "DEBTOR", "Customer");
                }
                else if (Arg.DataAccess.ActiveClient.Info.DBName.Contains("ClientAgility"))
                {
                    var shipper = new List<BOLHeader>();
                    shipper = Common.BOLHeader.GetAgilityDistinctShipper();
                    data.Shippers = new SelectList(shipper, "ShipperID", "Shipper");
                }
                else
                {
                    var shipper = new List<BOLHeader>();
                    shipper = Common.BOLHeader.GetDistinctShipper();
                    data.Shippers = new SelectList(shipper, "ShipperID", "Shipper");
                }
            }
            else
            {
                var shipper = new List<BOLHeader>();

                data.Shippers = new SelectList(shipper, "ShipperID", "Shipper");
            }
            var analyst = Common.ActivityStats.GetAnalystList();
            data.Analyst = new SelectList(analyst, "UserId", "ClientName");
        }

        public IActionResult Performance_v1()
        {
            var analystPerformance = new AnalystPerformance();
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                analystPerformance.CompanyId = Arg.DataAccess.ActiveClient.Info.CompanyId;
                analystPerformance.CommonObjects.TopHeading = "Analyst";
                analystPerformance.CommonObjects.Heading = "Analyst Performance";

                if (analystPerformance.SearchOptions != null && analystPerformance.CompanyId > 0)
                {
                    analystPerformance.SearchOptions.CompanyId = analystPerformance.CompanyId;
                }

                LoadData(analystPerformance);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return View(analystPerformance);
        }

        [HttpPost]
        public IActionResult Performance_v1(AnalystPerformance analystInfo)
        {
            var data = analystInfo;
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return Redirect(Common.MyRoot + "Account/Login");
                //}

                analystInfo.CompanyId = Arg.DataAccess.ActiveClient.Info.CompanyId;

                if (data.CompanyId > 0)
                {
                    data.SearchOptions.CompanyId = data.CompanyId;
                }
                LoadData(data);

                if (data.SearchOptions != null)
                {
                    data.SearchOptions.UserId = Common.CurrentUserId;
                }

                data.AnalystList = Common.ActivityStats.GetAnalystPerformanceCeva(data.SearchOptions);

                var detList = data.AnalystList.GroupBy(u => u.ClientName).Select(u => u.FirstOrDefault());
                var random = new Random();
                foreach (var item in data.AnalystList.GroupBy(u => u.ClientName).Select(u => u.FirstOrDefault()))
                {
                    var UserName = item.ClientName;

                    var color = String.Format("#{0:X6}", random.Next(0x1000000)); // = "#A197B9"
                    foreach (var item1 in data.AnalystList.Where(i => i.ClientName == UserName))
                    {
                        item1.Color = color;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return View(data);
        }
    }
}
