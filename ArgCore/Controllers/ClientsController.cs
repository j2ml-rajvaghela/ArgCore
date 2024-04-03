using Arg.DataModels;
using ArgCore.Attributes;
using ArgCore.Helpers;
using ArgCore.Models;
using ClosedXML.Excel;
using Codaxy.WkHtmlToPdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using System.Diagnostics;



namespace ArgCore.Controllers
{
    public class ClientsController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ClientsController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index(Clients clientsModel)
        {
            var data = new Clients();

            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                data.CommonObjects.TopHeading = "Clients";
                data.CommonObjects.Heading = "ARG Clients";

                var clientNames = Common.ArgClients.GetArgClients(Common.CurrentUserId);

                data.ClientNames = new SelectList(clientNames, "CompanyId", "Name", Arg.DataAccess.ActiveClient.Info.CompanyId);
                List<ArgClient> clients;

                if (clientsModel.SearchOptions != null)
                {
                    clients = new Arg.DataAccess.ArgClientsImpl().GetArgClients(clientsModel.SearchOptions.CompanyId, clientsModel.SearchOptions.Name,
                    Arg.DataAccess.Common.WildCardSearchToNormal(clientsModel.SearchOptions.Email), clientsModel.SearchOptions.Location,
                    clientsModel.SearchOptions.Contact, clientsModel.SearchOptions.LastModifiedStartDate,
                    clientsModel.SearchOptions.LastModifiedEndDate, Common.CurrentUserId);
                }
                else
                {
                    clients = Common.ArgClients.GetArgClients(Common.CurrentUserId);
                }

                if (clients != null && clients.Any())
                {
                    data.ClientsList = clients;
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return View(data);
        }

        public void SaveAllXLSXFilesToXLS()
        {
            string folderPath = Common.ClientFilesPath;
            string[] filesFolder = Directory.GetFiles(folderPath, "*.xlsx", SearchOption.AllDirectories);

            foreach (var file in filesFolder)
            {
                try
                {
                    using (var workBook = new XLWorkbook(file))
                    {
                        var worksheet = workBook.Worksheet(0);
                        var fileNameWithoutExt = Path.GetFileNameWithoutExtension(file);
                        var newFilePath = Path.Combine(Common.ClientFilesPath, fileNameWithoutExt + ".xls");
                        workBook.SaveAs(newFilePath);
                        RenameFile(file);
                    }                 
                }
                catch (Exception ex)
                {
                    Common.Log.Error(ex.ToString());
                }
            }
        }

        public void RenameFile(string file)
        {
            var renamedFile = file + ".done";
            if (!System.IO.File.Exists(renamedFile))
            {
                System.IO.File.Move(file, renamedFile);
            }
        }

        public void UnZipAllFiles()
        {
            try
            {
                string folderPath = Common.ClientFilesPath;
                string[] filesFolder = Directory.GetFiles(folderPath, "*.zip", SearchOption.AllDirectories);

                foreach (var file in filesFolder)
                {
                    try
                    {
                        var fileExtension = Path.GetExtension(file);
                        var fileName = Path.GetFileName(file);
                        var fileNameWithoutExt = Path.GetFileNameWithoutExtension(file);
                        var sourcePath = folderPath + fileName;
                        Trace.TraceInformation("Source Path: " + sourcePath);
                        var destinationPath = folderPath + fileNameWithoutExt;
                        Trace.TraceInformation("Destination Path: " + destinationPath);
                        System.IO.Compression.ZipFile.ExtractToDirectory(sourcePath, destinationPath);
                        RenameFile(file);
                        Trace.TraceInformation(fileName + " File Extracted");
                    }
                    catch (Exception ex)
                    {
                        Common.Log.Error(ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.ToString());
            }
        }

        [AuthorizeUser]
        public IActionResult LoadClientData()
        {
            var clientData = new LoadClientData();

            try
            {
                clientData.CompanyId = Arg.DataAccess.ActiveClient.Info.CompanyId;
                clientData.CommonObjects.TopHeading = "Load Client Data";
                clientData.CommonObjects.Heading = "Load Client Data";

                var companies = Common.ArgClients.GetArgClients(Common.CurrentUserId);
                clientData.Companies = new SelectList(companies, "CompanyId", "Name");

                if (clientData.CompanyId > 0)
                {
                    List<string> fileNames = new List<string>();

                    try
                    {
                        //UnZipping ZIP files
                        if (Directory.GetFiles(Common.ClientFilesPath, "*.zip") != null)
                        {
                            UnZipAllFiles();
                        }

                        //Saving XLSX file as XLS
                        if (Directory.GetFiles(Common.ClientFilesPath, "*.xlsx") != null)
                        {
                            SaveAllXLSXFilesToXLS();
                        }

                        var dataFiles = Directory.GetFiles(Common.ClientFilesPath, "*.xls", SearchOption.AllDirectories).Where(s => s.EndsWith(".xls"));

                        foreach (var file in dataFiles)
                        {
                            var fileName = Path.GetFileName(file);
                            fileNames.Add(fileName);
                            //fileNames.Add(new FileInfo { FileName = fileName, FilePath = file });
                        }
                    }
                    catch (Exception ex)
                    {
                        Common.Log.Error(ex);
                    }
                    clientData.DataFiles = new SelectList(fileNames);

                    var tablesNames = Common.TableSettings.GetTableSettingByTruncateTable(true);
                    if (tablesNames != null && tablesNames.Any())
                    {
                        clientData.TruncateTables = new SelectList(tablesNames, "TableName", "TableName");
                    }
                    //data.DataFiles = new SelectList(fileNames, "FilePath", "FileName");
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return View(clientData);
        }

        [AuthorizeUser]
        [HttpGet]
        public IActionResult Save(int? clientId)
        {
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                var clients = new Clients();
                clients.CommonObjects.TopHeading = "Clients";
                clients.ClientDetail = new ArgClient();

                var _clientId = Convert.ToInt32(clientId);
                if (_clientId > 0)
                {
                    clients.CommonObjects.Heading = "Edit Client";
                    clients.ClientDetail = Common.ArgClients.GetArgClient(_clientId, "");
                    clients.IPAddress = Common.IPAddressRestriction.GetIPAddresses(_clientId);

                    if (clients.ClientDetail == null || clients.ClientDetail.CompanyId <= 0)
                    {
                        return RedirectToAction("Clients", new { m = "Clients not found or deleted" });
                    }

                }
                else
                {
                    clients.CommonObjects.Heading = "Add Client";
                }
                return View(clients);

            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return null;
        }

        [HttpPost]
        public IActionResult Save(Clients clients)
        {
            try
            {
                clients.ClientDetail.LastAccessDate = DateTime.Now;
                Common.ArgClients.SaveArgClient(clients.ClientDetail);

                if (clients.ClientDetail.CompanyId > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Saved, clients.ClientDetail.CompanyId, "Clients");
                    Common.IPAddressRestriction.DeleteIPAddress(clients.ClientDetail.CompanyId);

                    foreach (var item in clients.IPAddress)
                    {
                        Arg.DataModels.IPAddressRestriction iPAddress = new Arg.DataModels.IPAddressRestriction()
                        {
                            IPAddressRestrictionId = 0,
                            BeginningIp = item.BeginningIp,
                            EndingIp = item.EndingIp,
                            CompanyId = clients.ClientDetail.CompanyId
                        };
                        Common.IPAddressRestriction.SaveIPAddress(iPAddress);
                    }
                        RedirectToAction("Index", "Clients");
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return RedirectToAction("Index", "Clients");
        }

        [AuthorizeUser]
        public IActionResult Delete(int clientId)
        {
            try
            {
                var result = Common.ArgClients.DeleteArgClient(clientId);
                if (result > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Deleted, clientId, "Clients");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return RedirectToAction("Index");
        }

        [AuthorizeUser]
        public IActionResult ManagementReports(int? companyId)
        {
            var reports = new ClientManagementReports();

            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}
                reports.CompanyId = Arg.DataAccess.ActiveClient.Info.CompanyId;

                reports.SearchOptions = new SearchOptions();

                if (reports.SearchOptions != null && reports.CompanyId > 0)
                {
                    reports.SearchOptions.CompanyId = reports.CompanyId;
                }
                LoadDataInModel(reports);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return View(reports);
        }

        [HttpPost]
        public IActionResult ManagementReports(ClientManagementReports reports)
        {
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}
                reports.CompanyId = Arg.DataAccess.ActiveClient.Info.CompanyId;

                LoadDataInModel(reports);

                if (reports.CompanyId > 0)
                {
                    reports.SearchOptions.CompanyId = reports.CompanyId;
                }

                if (reports.SearchOptions != null)
                {
                    GetData(reports);
                    //var revenueCollectedForPeriod = Common.BalanceDuesPayments.GetRevenueCollectedForPeriod(data.SearchOptions.ClientStartDate, data.SearchOptions.ClientEndDate);
                    //if (revenueCollectedForPeriod != null && revenueCollectedForPeriod.Any())
                    //    data.RevenueCollectedForPeriod = revenueCollectedForPeriod;
                    //var revenueCollectedForYTD = Common.BalanceDuesPayments.GetRevenueCollectedForYearToDate();
                    //if (revenueCollectedForYTD != null && revenueCollectedForYTD.Any())
                    //    data.RevenueCollectedForYTD = revenueCollectedForYTD;
                    //var revenueCollectedForPastYear = Common.BalanceDuesPayments.GetRevenueCollectedForPastYear();
                    //if (revenueCollectedForPastYear != null && revenueCollectedForPastYear.Any())//!string.IsNullOrWhiteSpace(revenueCollectedForPastYear.RevenueRecovered))
                    //    data.RevenueCollectedForPastYear = revenueCollectedForPastYear;
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return View(reports);
        }

        public void GetData(ClientManagementReports reports)
        {
            if (reports.SearchOptions.CompanyId > 0)
            {
                var client = Common.ArgClients.GetArgClient(reports.SearchOptions.CompanyId, "");
                if (client != null && client.CompanyId > 0)
                {
                    reports.Company = client.Name;
                }
                else
                {
                    reports.Company = "";
                }
            }

            var revenueRecovered = new List<BalanceDues_Payments>();
            if (reports.SearchOptions.ReportTypes.Contains("BOL Overcharge"))
            {
                revenueRecovered = Common.BalanceDuesPayments.GetRevenueRecoveredOvercharge(reports.SearchOptions.ClientStartDate, reports.SearchOptions.ClientEndDate, reports.SearchOptions.CompanyId, reports.SearchOptions.Regions, !Common.CurrentUserInfo.IsAdmin ? Common.CurrentUserId : "", reports.SearchOptions.ReportType);

                if (revenueRecovered != null && revenueRecovered.Any())
                {
                    reports.OverchargeRevenueRecovered = revenueRecovered;
                    foreach (var item in reports.OverchargeRevenueRecovered)
                    {
                        //REVENUE LOSS RATE (past 2 years)
                        var OverchargeRevenueLossRate = Common.BalanceDuesPayments.GetOverchargeRevenueLossRate(item.Region, reports.SearchOptions.CompanyId, reports.SearchOptions.ClientStartDate, reports.SearchOptions.ClientEndDate);
                        //data.RevenueLossRate = String.Format("{0:#,0.00}", revenueLossRate);
                        //revenueLossRate = (Common.BalanceDuesPayments.GetRevenueLossRatePaymentAmount(item.Region, data.SearchOptions.CompanyId) / Common.BalanceDuesPayments.GetRevenueLossRateScopeRevenue(item.Region, data.SearchOptions.CompanyId)) * 100;
                        reports.OverchargeRevenueLossRate = String.Format("{0:#,0.00}", OverchargeRevenueLossRate);
                        //REVENUE LOSS TREND (Line Chart)
                        //OLD Command
                        var revenueLossTrend = Common.BalanceDuesPayments.GetOverChargeLossTrend(item.Region, reports.SearchOptions.CompanyId, reports.SearchOptions.ClientStartDate, reports.SearchOptions.ClientEndDate);
                        //NEW Command
                        //var revenueLossTrend = Common.BalanceDuesPayments.GetRevenueLossTrendRate(item.Region, data.SearchOptions.CompanyId);
                        reports.OverchargeRevenueLossTrend = revenueLossTrend;
                        //REVENUE LOSS BY BD ERROR CODE (Pie Chart)
                        //todo confirm currency
                        var revLossByBDErrorCode = Common.BalanceDuesPayments.GetOverchargeByBDErrorCode(item.Region, reports.SearchOptions.CompanyId, reports.SearchOptions.ClientStartDate, reports.SearchOptions.ClientEndDate);
                        reports.OverchargeRevLossByBDErrorCode = revLossByBDErrorCode;

                        //REVENUE RECOVERED BY ORIGIN (Bar Chart)
                        var revRecoveredByOrigin = Common.BalanceDuesPayments.GetOverchargeByOrigin(item.Region, reports.SearchOptions.CompanyId, reports.SearchOptions.ClientStartDate, reports.SearchOptions.ClientEndDate);
                        reports.OverchargeRevRecoveredByOrigin = revRecoveredByOrigin;

                        //REVENUE BY CUSTOMER (Pie Chart)
                        var revByCustomer = Common.BalanceDuesPayments.GetOverchargeByCustomer(item.Region, reports.SearchOptions.CompanyId, reports.SearchOptions.ClientStartDate, reports.SearchOptions.ClientEndDate);
                        reports.OverchargeRevByCustomer = revByCustomer;
                    }
                }
                else
                {
                    reports.SearchOptions.Message = "No reports found!";
                }
            }
            if (reports.SearchOptions.ReportTypes.Contains("BOL Under-billing"))
            {
                revenueRecovered = Common.BalanceDuesPayments.GetRevenueRecovered(reports.SearchOptions.ClientStartDate, reports.SearchOptions.ClientEndDate, reports.SearchOptions.CompanyId, reports.SearchOptions.Regions, !Common.CurrentUserInfo.IsAdmin ? Common.CurrentUserId : "", reports.SearchOptions.ReportType);
                if (revenueRecovered != null && revenueRecovered.Any())
                {
                    reports.RevenueRecovered = revenueRecovered;
                    foreach (var item in reports.RevenueRecovered)
                    {
                        //CURRENT OPEN BALANCE
                        var currentOpenBal = Common.BalanceDues.GetCurrentOpenBalance(item.Region, reports.SearchOptions.CompanyId, reports.SearchOptions.ReportTypes);
                        reports.CurrentOpenBal = currentOpenBal;

                        //COLLECTION RATE
                        //var collectionRate = Common.BalanceDuesPayments.GetCollectionRate(item.Region, data.SearchOptions.CompanyId);
                        //decimal collecRateValue = (collectionRate.PaymentAmount / collectionRate.BalanceDueAmount) * 100;
                        decimal collecRateValue =Common.BalanceDuesPayments.GetCollectionRatePaymentAmount(item.Region,reports.SearchOptions.CompanyId) * 100; /// Common.BalanceDuesPayments.GetCollectionRateBalanceDueAmount(item.Region, data.SearchOptions.CompanyId)) * 100;
                        reports.CollectionRate = String.Format("{0:#,0.00}", collecRateValue);

                        //REVENUE LOSS RATE (past 12 months)
                        var revenueLossRate = Common.BalanceDuesPayments.GetRevenueLossRate(item.Region, reports.SearchOptions.CompanyId, reports.SearchOptions.ClientStartDate, reports.SearchOptions.ClientEndDate);
                        //data.RevenueLossRate = String.Format("{0:#,0.00}", revenueLossRate);
                        //revenueLossRate = (Common.BalanceDuesPayments.GetRevenueLossRatePaymentAmount(item.Region, data.SearchOptions.CompanyId) / Common.BalanceDuesPayments.GetRevenueLossRateScopeRevenue(item.Region, data.SearchOptions.CompanyId)) * 100;
                        reports.RevenueLossRate = String.Format("{0:#,0.00}", revenueLossRate);

                        //REVENUE LOSS TREND (Line Chart)
                        //OLD Command
                        var revenueLossTrend = Common.BalanceDuesPayments.GetRevenueLossTrend(item.Region, reports.SearchOptions.CompanyId, reports.SearchOptions.ClientStartDate, reports.SearchOptions.ClientEndDate);
                        //NEW Command
                        //var revenueLossTrend = Common.BalanceDuesPayments.GetRevenueLossTrendRate(item.Region, data.SearchOptions.CompanyId);
                        reports.RevenueLossTrend = revenueLossTrend;
                        //var xAxisLabel = Common.BalanceDuesScope.GetScopeDates();
                        //data.XAxisLabel = xAxisLabel;

                        //REVENUE LOSS BY BD ERROR CODE (Pie Chart)
                        //todo confirm currency
                        var revLossByBDErrorCode = Common.BalanceDuesPayments.GetRevLossByBDErrorCode(item.Region, reports.SearchOptions.ClientStartDate, reports.SearchOptions.ClientEndDate);
                        reports.RevLossByBDErrorCode = revLossByBDErrorCode;

                        //REVENUE RECOVERED BY ORIGIN (Bar Chart)
                        var revRecoveredByOrigin = Common.BalanceDuesPayments.GetRevRecoveredByOrigin(item.Region, reports.SearchOptions.ClientStartDate, reports.SearchOptions.ClientEndDate);
                        reports.RevRecoveredByOrigin = revRecoveredByOrigin;

                        //REVENUE BY CUSTOMER (Pie Chart)
                        var revByCustomer = Common.BalanceDuesPayments.GetRevByCustomer(item.Region, reports.SearchOptions.CompanyId, reports.SearchOptions.ClientStartDate, reports.SearchOptions.ClientEndDate);
                        reports.RevByCustomer = revByCustomer;
                    }
                }
                else
                {
                    reports.SearchOptions.Message = "No reports found!";
                }
            }
        }

        public void LoadDataInModel(ClientManagementReports reports)
        {
            reports.CommonObjects.TopHeading = "Client Report";
            reports.CommonObjects.Heading = "Client Management Reports";

            var companies = Common.ArgClients.GetArgClients(Common.CurrentUserId);
            reports.Companies = new SelectList(companies, "CompanyId", "Name");

            var regions = Common.Regions.GetRegions(0, "", reports.CompanyId);
            reports.Regions = new SelectList(regions, "Region", "Region");

            var reportTypes = new List<SelectListItem>
            {   new SelectListItem {Text = "Under-billing Snapshot", Value = "BOL Under-billing"},
                new SelectListItem {Text = "Overcharge Snapshot", Value = "BOL Overcharge"}
            };

            reports.ReportTypes = reportTypes;
        }

        [AuthorizeUser]
        public IActionResult PrintManagementReport(int? companyId, List<string> regions, string startDate, string endDate)
        {
            var reports = new ClientManagementReports();

            try
            {
                var _companyId = Convert.ToInt32(companyId);
                reports.SearchOptions = new SearchOptions();
                reports.SearchOptions.CompanyId = _companyId;
                reports.SearchOptions.Regions = regions;
                reports.SearchOptions.ClientStartDate = startDate;
                reports.SearchOptions.ClientEndDate = endDate;
                GetData(reports);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return View(reports);
        }

        #region PDF

        public FileResult pdf()
        {
            try
            {
                var fullPath = Path.Combine(_hostingEnvironment.WebRootPath, "ClientMngReports", "ARG_Management_Snapshot.pdf");
                PdfConvert.ConvertHtmlToPdf(new PdfDocument
                {
                    //Url = "http://www.neil-redfern.com/"
                    Url = "http://localhost:26744/ARGManagementSnapshot.html"
                    //Url = "http://localhost:26744/Clients/GenerateManagementReportPDF?companyId=1005&startDate=01-01-2017&endDate=03-06-2017&regions=null&generatePdf=false"
                    //Url = "http://autos.maxabout.com/"
                    //Url = "https://argatlasocean.net/Clients/GenerateManagementReportPDF?companyId=1005&startDate=01-01-2017&endDate=03-06-2017&regions=null&generatePdf=false"
                }, new PdfOutput
                {
                    OutputFilePath = fullPath
                });

                byte[] fileBytes = System.IO.File.ReadAllBytes(fullPath);
                string fileName = "export.pdf";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Pdf, fileName);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return null;
        }

        public IActionResult GenerateManagementReportPDF(int companyId, string startDate, string endDate, List<string> regions, List<string> reportTypes, bool generatePdf = false)
        {
            var reports= new ClientManagementReports();
            var returnView = "ReportPDF";

            try
            {
                reports.SearchOptions = new SearchOptions();
                reports.SearchOptions.CompanyId = companyId;
                reports.SearchOptions.ClientStartDate = startDate;
                reports.SearchOptions.ClientEndDate = endDate;
                reports.SearchOptions.ReportTypes = new List<string>();
                reports.SearchOptions.ReportTypes = reportTypes[0].Split(',').ToList();
                reports.SearchOptions.Regions = new List<string>();
                reports.SearchOptions.Regions = regions.Where(x => x != "null").ToList();

                GetData(reports);

                if (generatePdf && ((reports.RevenueRecovered != null && reports.RevenueRecovered.Any()) || (reports.OverchargeRevenueRecovered != null && reports.OverchargeRevenueRecovered.Any())))
                {
                    var clientName = "";

                    if (companyId > 0)
                    {
                        clientName = Common.ArgClients.GetArgClient(companyId, "").Name;
                        reports.Company = clientName;
                    }

                    string footer = Path.Combine(_hostingEnvironment.ContentRootPath, "Views", "Clients", "ReportPDFFooter.html");
                    string customSwitches = $"--footer-left {DateTime.Now.Date.ToString("MM/dd/yyyy")} --footer-right \" [page]/[topage]\" --header-spacing \"0\" --footer-spacing \"0\" --javascript-delay 2000";

                    var fullPath = Path.Combine(_hostingEnvironment.ContentRootPath, "ClientMngReports", $"ARG_Management_Snapshot_{clientName}_{Arg.Core.Utility.GenerateRandomInvoiceNo()}.pdf");
                    var folder = Path.GetDirectoryName(fullPath);

                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    var actionResult = new ViewAsPdf();

                    if (reports.SearchOptions.ReportTypes.Contains("BOL Overcharge") && reports.SearchOptions.ReportTypes.Contains("BOL Under-billing"))
                    {
                        actionResult = new ViewAsPdf("BothReportPDF", reports) { CustomSwitches = customSwitches };
                        returnView = "BothReportPDF";
                    }
                    else
                    {
                        {
                            if (reports.SearchOptions.ReportTypes.Contains("BOL Overcharge"))
                            {
                                actionResult = new ViewAsPdf("OverchargeReportPDF", reports) { CustomSwitches = customSwitches };
                                returnView = "OverchargeReportPDF";
                            }
                            if (reports.SearchOptions.ReportTypes.Contains("BOL Under-billing"))
                            {
                                actionResult = new ViewAsPdf("ReportPDF", reports) { CustomSwitches = customSwitches };
                                returnView = "ReportPDF";
                            }
                        }
                    }


                    var byteArray = actionResult.BuildFile(ControllerContext).Result;
                    System.IO.File.WriteAllBytes(fullPath, byteArray);

                    var result = new GeneratePDFResult();
                    result.ServerPath = fullPath;
                    result.InvoiceUrl = fullPath.Replace(Common.MyAppRoot, Common.MyRoot);

                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }

            return View(returnView, reports);
        }

        [HttpGet]
        public IActionResult ReportPDF(ClientMngmntReportPDF reportPDF)
        {
            return View(reportPDF);
        }

        [HttpGet]
        public IActionResult OverchargeReportPDF(ClientMngmntReportPDF reportPDF)
        {
            return View(reportPDF);
        }

        [HttpGet]
        public IActionResult BothReportPDF(ClientMngmntReportPDF reportPDF)
        {
            return View(reportPDF);
        }

        private class GeneratePDFResult
        {
            public string ServerPath { get; set; }
            public string InvoiceUrl { get; set; }
        }

        #endregion
    }
}
