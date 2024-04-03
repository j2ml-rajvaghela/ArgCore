using Arg.DataModels;
using ArgCore.Attributes;
using ArgCore.Helpers;
using ArgCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Reporting.WebForms;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ArgCore.Controllers
{
    [Authorize]
    public class ArgInvoicesBDController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ArgInvoicesBDController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [AuthorizeUser]
        public IActionResult Index(string companyId)
        {
            try
            {
                var argInvoicesBD = new ArgInvoicesBD();

                if (companyId != "null" && companyId != null)
                {
                    HttpContext.Session.SetString("ActiveMultipleClient", null);
                    argInvoicesBD.CompanyIds = companyId.Split(',').ToList();
                    HttpContext.Session.SetString("ActiveMultipleClient", companyId);
                }

                if (companyId == "null")
                {
                    HttpContext.Session.SetString("ActiveMultipleClient", null);
                }

                var str = Convert.ToString(HttpContext.Session.GetString("ActiveMultipleClient"));

                if (!string.IsNullOrWhiteSpace(str) && companyId == null)
                {
                    companyId = Convert.ToString(HttpContext.Session.GetString("ActiveMultipleClient"));
                    argInvoicesBD.CompanyIds = companyId.Split(',').ToList();
                }

                if (argInvoicesBD.SearchOptions != null && argInvoicesBD.CompanyIds != null)
                {
                    argInvoicesBD.SearchOptions.CompIds = argInvoicesBD.CompanyIds;
                }

                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                LoadDataInModel(argInvoicesBD);

                argInvoicesBD.ArgInvoicesBDDetail = new ArgInvoices_BalanceDues();

                return View(argInvoicesBD);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                Common.Log.Error(ex);
            }

            return null;
        }

        [HttpPost]
        public JsonResult LoadInvoiceNos(List<string> companyIds, List<string> regions, List<string> invoiceTypes)
        {
            var invoiceNo = Common.ArgInvoices.GetDistinctInvoiceNo(companyIds, regions, invoiceTypes);
            return Json(new SelectList(invoiceNo, "Invoice", "InvoiceTextField"));
        }

        public void LoadDataInModel(ArgInvoicesBD argInvoicesBD)
        {
            argInvoicesBD.CommonObjects.TopHeading = "Client Invoices";
            argInvoicesBD.CommonObjects.Heading = "Client Invoices";

            var companyId = string.Empty;

            if (argInvoicesBD.SearchOptions == null)
            {
                argInvoicesBD.SearchOptions = new SearchOptions();

            }

            if (argInvoicesBD.CompanyIds != null)
            {
                companyId = string.Join(",", argInvoicesBD.CompanyIds);
                argInvoicesBD.SearchOptions.CompIds = argInvoicesBD.CompanyIds;
            }

             var companies = Common.ArgClients.GetArgClients(Common.CurrentUserId);
            argInvoicesBD.Companies = new SelectList(companies, "CompanyId", "Name");

            var invoiceTypes = Common.BalanceDues.GetDistinctInvoiceTypesMultiple(companyId);
            argInvoicesBD.InvoiceTypes = new SelectList(invoiceTypes, "InvoiceType", "InvoiceType");

            var regions = Common.Regions.GetRegionsMultiple(0, "", companyId).OrderBy(x => x.Region);
            argInvoicesBD.Regions = new SelectList(regions, "Region", "Region");

            var status = Common.ArgInvoices.GetDistinctInvoiceStatusMultiple(companyId);
            argInvoicesBD.Status = new SelectList(status, "InvoiceStatus", "InvoiceStatus");

            var invoiceNo = Common.ArgInvoices.GetDistinctInvoiceNoMultiple(companyId);
            argInvoicesBD.InvoiceNo = new SelectList(invoiceNo, "Invoice", "InvoiceTextField");

            //ViewBag.Invoices = new SelectList(invoiceNo, "Invoice", "InvoiceTextField");
            //var invoiceTypes = Common.BalanceDues.GetDistinctInvoiceTypes(data.CompanyId);
            //data.InvoiceTypes = new SelectList(invoiceTypes, "InvoiceType", "InvoiceType");

            //var regions = Common.Regions.GetRegions(0, "", data.CompanyId).OrderBy(x => x.Region);
            //data.Regions = new SelectList(regions, "Region", "Region");

            //var status = Common.ArgInvoices.GetDistinctInvoiceStatus(data.CompanyId);
            //data.Status = new SelectList(status, "InvoiceStatus", "InvoiceStatus");

            //var invoiceNo = Common.ArgInvoices.GetDistinctInvoiceNo(data.CompanyId);
            //data.InvoiceNo = new SelectList(invoiceNo, "Invoice", "InvoiceTextField");
        }

        public JsonResult GetBDCountInfo(string companyId, string region, string invoiceType, string invoiceNo)
        {
            var bDCountInfo = new BDCountInfo();

            try
            {
                List<BalanceDue> balanceDues;
                List<string> regionList = new List<string>();

                if (region != null)
                {
                    regionList = region.Split(',').ToList();
                }

                List<string> invoiceTypeList = new List<string>();

                if (invoiceType != null)
                {
                    invoiceTypeList = invoiceType.Split(',').ToList();
                }

                List<string> invoiceList = new List<string>();

                if (invoiceList != null)
                {
                    invoiceList = invoiceNo.Split(',').ToList();
                }

                ArgInvoice argInvoice = Common.ArgInvoices.GetArgMultipleInvoice(0, invoiceList);
                balanceDues = Common.BalanceDues.GetMultipleBalanceDues(companyId, regionList, invoiceTypeList);

                string bdIds = string.Join(",", balanceDues.Select(x => x.BalanceId));
                bDCountInfo.BdIds = bdIds;

                var bolCount = balanceDues.Select(x => x.Bol).Count();//check

                bDCountInfo.TotalBol = bolCount;
                bDCountInfo.TotalPaymentAmount = balanceDues.Select(x => x.PaymentAmount).Sum();
                bDCountInfo.Currency = balanceDues.FirstOrDefault()?.PaymentCurrency;

                var invoiceTypes = Common.BalanceDues.GetDistinctInvoiceTypesByStatus(companyId, regionList, "Paid");
                bDCountInfo.InvoiceTypes = new SelectList(invoiceTypes, "InvoiceType", "InvoiceType");
                bDCountInfo.ConversationRate = Common.CurrencyConversionRates.GetConversionRate(bDCountInfo.Currency, argInvoice.InvoiceDate);
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
            return Json(bDCountInfo);
        }

        //public JsonResult GetBDCountInfo(int companyId, string region)
        //{
        //    var result = new BDCountInfo();
        //    try
        //    {
        //        List<Arg.DataModels.BalanceDue> balanceDues;
        //        balanceDues = Common.BalanceDues.GetBalanceDues(companyId, region);
        //        string bdIds = string.Join(",", balanceDues.Select(x => x.BalanceId));
        //        result.BdIds = bdIds;
        //        var bolCount = balanceDues.Select(x => x.Bol).Count();//check
        //        result.TotalBol = bolCount;
        //        result.TotalPaymentAmount = balanceDues.Select(x => x.PaymentAmount).Sum();
        //        var invoiceTypes = Common.BalanceDues.GetDistinctInvoiceTypesByStatus(companyId, region, "Paid");
        //        result.InvoiceTypes = new SelectList(invoiceTypes, "InvoiceType", "InvoiceType");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(ex);
        //    }
        //    return Json(result);
        //}

       

        //public JsonResult GenerateInvoicePDF(string invoiceNo)
        //{
        //    var serverPath = Server.MapPath("~/ClientInvoices/");
        //    var result = CreatePdfFile(serverPath, "InvoicePDF", new { invoiceNo = invoiceNo });
        //    return Json(result);
        //}

        public static int GetNoOfPagesPDF(string fileName)
        {
            int result = 0;
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            StreamReader streamReader = new StreamReader(fileStream);
            string pdfText = streamReader.ReadToEnd();

            Regex regx = new Regex(@"/Type\s*/Page[^s]");
            MatchCollection matches = regx.Matches(pdfText);
            result = matches.Count;
            return result;
        }

        /// <summary>
        /// Example Call for testing: http://localhost:26744/ArgInvoicesBD/GenerateInvoicePDF?invoiceNo=10207214&generatePdf=True
        /// </summary>
        /// <param name="invoiceNo"></param>
        /// <param name="generatePdf"></param>
        /// <returns></returns>
        
        [AuthorizeUser]
        [HttpGet]
        public IActionResult GenerateInvoicePDF(string invoiceNo, string companyId, bool generatePdf = false, bool displayDetails = false)
        {
            var argInvoicesBDPDF = new ArgInvoicesBDPDF();
            try
            {
                List<string> invoiceNoList = new List<string>();

                if (invoiceNo != null)
                {
                    invoiceNoList = invoiceNo.Split(',').ToList();
                }

                //var invoiceInfo = Common.ArgInvoices.GetArgInvoiceInfo(invoiceNo, Common.CurrentUserId, arg.DataAccess.ActiveClient.Info.CompanyId);
                var invoiceInfo = Common.ArgInvoices.GetArgInvoiceMultipleInfo(invoiceNoList, Common.CurrentUserId, companyId);

                if (invoiceInfo != null && invoiceInfo.InvoiceId > 0)
                {
                    argInvoicesBDPDF.InvoiceInfo = invoiceInfo;
                }

                var bdInvoices = Common.ArgInvoicesBD.GetPDFInvoiceMultiple(invoiceNoList, Common.CurrentUserId, companyId);

                //var bdInvoices = Common.ArgInvoicesBD.GetPDFInvoice(invoiceNoList, Common.CurrentUserId, arg.DataAccess.ActiveClient.Info.CompanyId);
                if (bdInvoices != null && bdInvoices.Any())
                {
                    argInvoicesBDPDF.BDInvoices = bdInvoices;
                }

                if (argInvoicesBDPDF.InvoiceInfo != null && argInvoicesBDPDF.InvoiceInfo.InvoiceId > 0)
                {
                    argInvoicesBDPDF.AmountDue = Common.ArgInvoicesBD.GetAmountDueUSDMultiple(invoiceNoList, companyId);
                    argInvoicesBDPDF.AmountPaid = Common.BalanceDuesPayments.GetAmountPaidMultiple(invoiceNoList, companyId);
                    //data.AmountDue = Common.ArgInvoicesBD.GetAmountDueUSD(invoiceNo, arg.DataAccess.ActiveClient.Info.CompanyId);
                    //data.AmountPaid = Common.BalanceDuesPayments.GetAmountPaid(invoiceNo, arg.DataAccess.ActiveClient.Info.CompanyId);
                }

                if (displayDetails)
                {
                    return View("InvoiceDetails", argInvoicesBDPDF);
                }
                else if (generatePdf && argInvoicesBDPDF.InvoiceInfo != null)
                {
                    string footer = Path.Combine(_hostingEnvironment.WebRootPath, "Views", "ArgInvoicesBD", "InvoicePDFFooter.html");
                    var invDate = argInvoicesBDPDF.InvoiceInfo.InvoiceDate.ToString("yyyy-MM-dd");
                    var fullPath = Path.Combine(_hostingEnvironment.ContentRootPath, "ClientInvoices", $"ARG_{argInvoicesBDPDF.InvoiceInfo.Company}_{argInvoicesBDPDF.InvoiceInfo.Region}_{argInvoicesBDPDF.InvoiceInfo.Invoice}_{invDate}.pdf");

                    var reportPath = "/ARG/ARG_Invoice";

                    List<ReportParameter> reportParameters = new List<ReportParameter>
                    {
                        new ReportParameter("Client", companyId),
                        new ReportParameter("InvoiceNumber", invoiceNo)
                    };

                    Common.GenerateReport(reportPath, reportParameters, "PDF", fullPath);

                    var result = new GenerateInvoicePDF_Result();
                    result.ServerPath = fullPath;
                    result.InvoiceUrl = fullPath.Replace(Common.MyAppRoot, Common.MyRoot);

                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return View("InvoicePDF", argInvoicesBDPDF);
        }

        [AuthorizeUser]
        [HttpGet]
        public IActionResult InvoicePDF(ArgInvoicesBDPDF data)
        {
            return View(data);
        }

        /// <summary>
        /// -Populate: ARGInvoices.BalanceDues

        //AmountDueUSD
        //a)-----If Balancedues.Currency= 'USD' AmountDueUSD equals -----> sum(BalanceDues.Payments.PaymentAmount) * ArgClients.CommissionRate
        //b)----If Balancedues.Currency <> 'USD'  AmountDueUSD equals sum(BalanceDues.Payments.PaymentAmount) * ArgClients.CommissionRate * (CurrencyConversionRates.ConversionRate ///// matching currencyconversionrate based on ConversionDate = ARGInvoicesInvoicesDate + CurrencyConvertedTo =BalanceDues.Currency
        //AmountDueLocalCurrency: Equals  sum(BalanceDues.Payments.PaymentAmount) * ArgClients.CommissionRate
        //LocalCurrency: BalanceDues.Currency

        /// </summary>
        /// <param name="bdIds"></param>
        /// <returns></returns>

        public JsonResult AddBalanceDuesToInvoice(string bdIds, string invoiceNo, string invoiceType, string companyId, decimal amount)
        {
            var bDCountInfo = new BDCountInfo();
            try
            {
                var addedInvoicesBD = 0; var addedInvoicesCommission = 0;
                var addedCollectionComments = 0; var updatedBD = 0;

                List<string> invoiceNoList = new List<string>();

                if (invoiceNo != null)
                {
                    invoiceNoList = invoiceNo.Split(',').ToList();
                }

                var invoiceInfo = Common.ArgInvoices.GetArgMultipleInvoice(0, invoiceNoList);

                //var invoiceInfo = Common.ArgInvoices.GetArgInvoice(0, invoiceNo);
                if (invoiceInfo.InvoiceStatus != "Open")
                {
                    bDCountInfo.Message = "Invoice status must be open to add Balance Dues to Invoice!";
                    return Json(bDCountInfo);
                }

                List<int> bdIdsList = bdIds.Split(',').Select(int.Parse).ToList();
                List<string> invoiceTypeList = invoiceType.Split(',').ToList();

                if (bdIdsList != null && bdIdsList.Any())
                {
                    foreach (var bdId in bdIdsList)
                    {
                        var bdInfo = Common.BalanceDues.GetMultipleBalanceDue(bdId, "", companyId, invoiceTypeList);
                        // var bdInfo = Common.BalanceDues.GetBalanceDue(bdId, "", Common.GetActiveClientId(), invoiceTypeList);
                        if (bdInfo != null)

                        {
                            //Invoices BD
                            var invoiceBD = new ArgInvoices_BalanceDues();
                            var bdPayment = Common.BalanceDuesPayments.GetBalanceDuesPayments(bdInfo);
                            var client = Common.ArgClients.GetArgClient(bdInfo.CompanyId, "");
                            decimal paymentAmountSum = bdPayment.Select(x => x.PaymentAmount).Sum();

                            var paymentCurrency = bdPayment.FirstOrDefault()?.Currency;
                            if (paymentCurrency.Equals("USD") || string.IsNullOrEmpty(paymentCurrency))//todo to be done later... currency should not be empty
                            {
                                if (bdInfo.InvoiceType.Contains("BOL Overcharge"))
                                {
                                    invoiceBD.AmountDueUSD = client.OverchargeFee;
                                }
                                else
                                {
                                    invoiceBD.AmountDueUSD = paymentAmountSum * client.UnderBillingCommissionRate; //check
                                }
                                //if (bdInfo.InvoiceType.Contains("BOL Overcharge"))
                                //    invoiceBD.AmountDueUSD = client.OverchargeFee;
                                //else
                                //    invoiceBD.AmountDueUSD = client.UnderBillingCommissionRate; //check
                            }
                            else if (!paymentCurrency.Equals("USD"))
                            {
                                var currencyConversionRate = Common.CurrencyConversionRates.GetConversionRate(bdInfo.Currency, invoiceInfo.InvoiceDate);
                                var paymentAmountSumNonUsd = currencyConversionRate * paymentAmountSum;

                                if (bdInfo.InvoiceType.Contains("BOL Overcharge"))
                                {
                                    invoiceBD.AmountDueUSD = currencyConversionRate > 0 ? client.OverchargeFee * paymentAmountSumNonUsd : client.OverchargeFee;
                                }
                                else
                                {
                                    invoiceBD.AmountDueUSD = currencyConversionRate > 0 ? client.UnderBillingCommissionRate * paymentAmountSumNonUsd : client.UnderBillingCommissionRate;//check
                                }
                            }
                            var amntDueLocalCurrency = paymentAmountSum * client.UnderBillingCommissionRate;

                            if (bdInfo.InvoiceType.Contains("BOL Overcharge"))
                            {
                                invoiceBD.AmountDueLocalCurrency = client.OverchargeFee;
                            }
                            //if (bdInfo.Currency == "USD" || bdInfo.Currency == "")//todo to be done later... currency should not be empty
                            //{
                            //    if (bdInfo.InvoiceType.Contains("BOL Overcharge"))
                            //        invoiceBD.AmountDueUSD = paymentAmountSum + client.OverchargeFee;
                            //    else
                            //        invoiceBD.AmountDueUSD = paymentAmountSum * client.UnderBillingCommissionRate; //check
                            //}
                            //else if (bdInfo.Currency != "USD")
                            //{
                            //    var currencyConversionRate = Common.CurrencyConversionRates.GetConversionRate(bdInfo.Currency);//todo confirm currency
                            //    if (bdInfo.InvoiceType.Contains("BOL Overcharge"))
                            //        invoiceBD.AmountDueUSD = (paymentAmountSum + client.OverchargeFee) * currencyConversionRate;
                            //    else
                            //        invoiceBD.AmountDueUSD = paymentAmountSum * client.UnderBillingCommissionRate * currencyConversionRate;//check
                            //}
                            //var amntDueLocalCurrency = paymentAmountSum * client.UnderBillingCommissionRate;
                            //if (bdInfo.InvoiceType.Contains("BOL Overcharge"))
                            //    invoiceBD.AmountDueLocalCurrency = paymentAmountSum + client.OverchargeFee;
                            // else

                            foreach (var item in invoiceNoList)
                            {
                                invoiceBD.AmountDueLocalCurrency = amntDueLocalCurrency;
                                invoiceBD.LocalCurrency = bdInfo.Currency;
                                invoiceBD.CompanyId = invoiceInfo.CompanyId;
                                invoiceBD.Region = invoiceInfo.Region;
                                invoiceBD.InvoiceNo = item;
                                invoiceBD.InvoiceDate = invoiceInfo.InvoiceDate;
                                invoiceBD.CustomerId = bdInfo.CustomerId;
                                invoiceBD.CustomerLocationCode = bdInfo.CustomerLocationCode;
                                invoiceBD.BookingId = bdInfo.BookingId;
                                invoiceBD.BOLNo = bdInfo.Bol;
                                invoiceBD.BolExecutionDate = bdInfo.BolExecutionDate;
                                invoiceBD.BalanceDueInvoice = bdInfo.BalanceDueInvoice;

                                Common.ArgInvoicesBD.SaveArgInvBD(invoiceBD);

                                if (invoiceBD.InvoiceBDId > 0)
                                {
                                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Saved, invoiceBD.CompanyId, "ArgInvoices Balance Dues");
                                    addedInvoicesBD++;
                                    var commissionRates = Common.CommissionRates.GetMultipleRates(item);

                                    //var commissionRates = Common.CommissionRates.GetRates(invoiceNo);

                                    foreach (var rate in commissionRates)
                                    {
                                        //Invoices Commissions
                                        var invoiceCommissions = new Arg.DataModels.Commissions();
                                        // invoiceCommissions.AmountDueUSD = 0;
                                        if (rate.RateBasis == "All" || rate.UserId == bdInfo.RevenueAnalystAuditor)
                                        {
                                            invoiceCommissions.AmountDueUSD = invoiceBD.AmountDueUSD * rate.Rate;
                                            invoiceCommissions.UserId = rate.UserId;
                                            invoiceCommissions.CompanyId = invoiceInfo.CompanyId;
                                            invoiceCommissions.Region = invoiceInfo.Region;
                                            invoiceCommissions.InvoiceNo = item;
                                            invoiceCommissions.InvoiceDate = invoiceInfo.InvoiceDate;
                                            invoiceCommissions.CustomerId = bdInfo.CustomerId;
                                            invoiceCommissions.CustomerLocationCode = bdInfo.CustomerLocationCode;
                                            invoiceCommissions.BookingId = bdInfo.BookingId;
                                            invoiceCommissions.BOLNo = bdInfo.Bol;
                                            invoiceCommissions.BolExecutionDate = bdInfo.BolExecutionDate;
                                            Common.Commissions.SaveCommission(invoiceCommissions);
                                        }

                                        if (invoiceCommissions.CommissionId > 0)
                                        {
                                            Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Saved, invoiceCommissions.CompanyId, "ArgInvoices Commissions");
                                            addedInvoicesCommission++;
                                        }
                                    }
                                }
                            }

                            bdInfo.InvoiceStatus = "ARGInv";
                            Common.BalanceDues.SaveBalanceDue(bdInfo);
                            if (bdInfo.BalanceId > 0)
                            {
                                updatedBD++;
                                Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Updated, bdInfo.CompanyId, "Balance Dues");
                                var collComm = AddCollectionComment(bdInfo, "Invoice status changed from 'Paid' to 'ARGIn'");
                                if (collComm.CollectionId > 0)
                                {
                                    addedCollectionComments++;
                                }
                            }
                        }
                    }

                    bDCountInfo.InvoicesBDCount = addedInvoicesBD;
                    bDCountInfo.InvoicesCommissionCount = addedInvoicesCommission;
                    bDCountInfo.CollectionCommentCount = addedCollectionComments;
                    bDCountInfo.UpdatedBDCount = updatedBD;
                }
                return Json(bDCountInfo);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                bDCountInfo.Message = ex.ToString();
                return Json(bDCountInfo);
            }
        }

        [AuthorizeUser]
        public CollectionComment AddCollectionComment(BalanceDue bdInfo, string comments)
        {
            try
            {
                var collectionCommentInfo = new CollectionComment
                {
                    Region = bdInfo.Region,
                    CustomerId = bdInfo.CustomerId,
                    CompanyId = bdInfo.CompanyId,
                    Bol = bdInfo.Bol,
                    BolExecutionDate = bdInfo.BolExecutionDate,
                    DateTime = DateTime.Now,
                    Collector = Common.CurrentUserId,
                    CustomerLocationCode = bdInfo.CustomerLocationCode,
                    BookingId = bdInfo.BookingId
                };
                collectionCommentInfo.Comments = comments;

                Common.CollectionComments.SaveCollectionComment(collectionCommentInfo);

                if (collectionCommentInfo.CollectionId > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Saved, bdInfo.CompanyId, "Collection Comment");
                }

                return collectionCommentInfo;
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return null;
        }

        [HttpPost]
        public IActionResult Index(ArgInvoicesBD balanceDueModel)
        {
            try
            {
                var model = balanceDueModel;
                if (balanceDueModel.SearchOptions.CompIds != null)
                {
                    balanceDueModel.CompanyIds = balanceDueModel.SearchOptions.CompIds;
                }

                balanceDueModel.CompanyId = Arg.DataAccess.ActiveClient.Info.CompanyId;

                LoadDataInModel(balanceDueModel);

                if (model.CompanyId > 0)
                {
                    model.SearchOptions.CompanyId = model.CompanyId;
                }

                //var currentUserId = "";
                //bool argManager = false;
                //if (Common.CurrentUserInfo.IsARGManager || Common.CurrentUserInfo.IsARGSalesAnalyst || Common.CurrentUserInfo.IsARGAnalyst)
                //{
                //    //argManager = true;
                //    currentUserId = Common.CurrentUserId;
                //}
                var searchOptions = balanceDueModel.SearchOptions;

                List<Arg.DataModels.ArgInvoice> invoicesBD;

                if (searchOptions != null)
                {
                    invoicesBD = Common.ArgInvoices.GetInvoices(searchOptions, Common.CurrentUserId);
                }
                else
                {
                    invoicesBD = Common.ArgInvoices.GetInvoices(new SearchOptions(), Common.CurrentUserId);
                }

                if (invoicesBD != null && invoicesBD.Any())
                {
                    model.ArgInvoicesList = invoicesBD;
                    //confirm all are also getting sum >> confirm all from sir once
                    decimal totalInvoices = model.ArgInvoicesList.Select(x => x.InvoiceAmountUSD).Sum();
                    model.TotalInvoices = string.Format("{0:n}", totalInvoices);
                    decimal totalPayments = model.ArgInvoicesList.Select(x => x.AmountPaid).Sum();
                    model.TotalPayments = string.Format("{0:n}", totalPayments);
                    decimal totalCommissions = model.ArgInvoicesList.Select(x => x.CommissionsAmountDueUSD).Sum();
                    model.TotalCommissions = string.Format("{0:n}", totalCommissions);
                    decimal amountOpen = totalInvoices - totalCommissions;//todo check
                    model.AmountOpen = string.Format("{0:n}", amountOpen);
                }
                else
                {
                    model.Message = "No results found related to your search!";
                }

                var str = Convert.ToString(HttpContext.Session.GetString("ActiveMultipleClient"));

                if (!string.IsNullOrWhiteSpace(str))
                {
                    model.SearchOptions.CompIds = new List<string>();
                    var companyId = Convert.ToString(HttpContext.Session.GetString("ActiveMultipleClient"));
                    model.SearchOptions.CompIds = companyId.Split(',').ToList();
                }
                //if (data.ArgInvoicesBDList != null && data.ArgInvoicesBDList.Any())
                //{
                //    var custCount = data.ArgInvoicesBDList.Select(x => x.CustomerId).Distinct().Count();
                //    data.CustomerCount = custCount;
                //    var bdCount = data.ArgInvoicesBDList.Count();
                //    data.TotalBDCount = bdCount;
                //    var totalBOL = data.ArgInvoicesBDList.Select(x => x.Bol).Distinct().Count();
                //    data.BOLCount = totalBOL;
                //    decimal totalBD = data.ArgInvoicesBDList.Select(x => x.BalanceDueAmount).Sum();
                //    data.TotalBD = String.Format("{0:n}", totalBD);
                //    decimal totalBDPaidAmnt = data.ArgInvoicesBDList.Select(x => x.AmountPaid).Sum();
                //    data.TotalBDPaidAmount = String.Format("{0:n}", totalBDPaidAmnt);
                //}

                return View(model);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                Common.Log.Error(ex);
            }
            return null;
        }

        [AuthorizeUser]
        [HttpGet]
        public IActionResult Save(int? invBdId)
        {
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //   return RedirectToAction("LogIn", "Account");
                //}

                var balanceDues = new BalanceDues();
                balanceDues.CommonObjects.TopHeading = "Invoices Balance Dues";

                var companies = Common.ArgClients.GetArgClients(Common.CurrentUserId);
                balanceDues.Companies = new SelectList(companies, "CompanyId", "Name");

                balanceDues.BalanceDuesDetail = new BalanceDue();

                var _balId = Convert.ToInt32(invBdId);
                if (_balId > 0)
                {
                    balanceDues.CommonObjects.Heading = "Edit Balance Due";
                    balanceDues.BalanceDuesDetail = Common.BalanceDues.GetBalanceDue(_balId, "", Common.GetActiveClientId());

                    if (balanceDues.BalanceDuesDetail == null || balanceDues.BalanceDuesDetail.BalanceId <= 0)
                    {
                        return RedirectToAction("BalanceDues", new { m = "Balance Dues not found or deleted" });
                    }
                }
                else
                {
                    balanceDues.CommonObjects.Heading = "Add Balance Due";
                }
                return View(balanceDues);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                Common.Log.Error(ex);
            }
            return null;
        }

        [HttpPost]
        public IActionResult Save(BalanceDues balanceDues)
        {
            try
            {
                balanceDues.BalanceDuesDetail.LastModified = DateTime.UtcNow;
                balanceDues.BalanceDuesDetail.LastModifiedBy = "Lorem";

                Common.BalanceDues.SaveBalanceDue(balanceDues.BalanceDuesDetail);

                if (balanceDues.BalanceDuesDetail.BalanceId > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Saved, balanceDues.BalanceDuesDetail.CompanyId, "Balance Dues");
                    RedirectToAction("Index" ,"ArgInvoicesBD");
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                Common.Log.Error(ex);
            }
            return RedirectToAction("Index", "ArgInvoicesBD");
        }

        [AuthorizeUser]
        public IActionResult InvoicePaymentAmtDetails(int invoiceId, int companyId)
        {
            var invoicePaymentAmtDetails = new InvoicePaymentAmtDetails();
            try
            {
                var invoiceInfo = Common.ArgInvoices.GetArgInvoice(invoiceId, "", companyId);

                //var invoiceInfo = Common.ArgInvoices.GetArgInvoice(invoiceId, "", arg.DataAccess.ActiveClient.Info.CompanyId);
                if (invoiceInfo != null && invoiceInfo.InvoiceId > 0)
                {
                    invoicePaymentAmtDetails.InvoiceInfo = invoiceInfo;
                    invoicePaymentAmtDetails.PaymentDate = DateTime.Now;

                    var payments = Common.ArgInvoicePayments.GetInvoicePayments(invoicePaymentAmtDetails.InvoiceInfo.Invoice, companyId);

                    //var payments = Common.ArgInvoicePayments.GetInvoicePayments(data.InvoiceInfo.Invoice, arg.DataAccess.ActiveClient.Info.CompanyId);
                    if (payments != null && payments.Any())
                    {
                        invoicePaymentAmtDetails.InvoicePayments = payments;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.ToString());
            }
            return View(invoicePaymentAmtDetails);
        }

        [AuthorizeUser]
        public IActionResult CommissionDetails(int invoiceId)
        {
            var commissionDetails = new CommissionDetails();
            commissionDetails.CommissionsSummeryList = new List<CommissionSummery>();

            try
            {
                var invoiceInfo = Common.ArgInvoices.GetArgInvoice(invoiceId);
                if (invoiceInfo != null && invoiceInfo.InvoiceId > 0)
                {
                    //data.InvoiceInfo = invoiceInfo;
                    var commissions = Common.Commissions.GetCommissions(invoiceInfo.Invoice);
                    if (commissions != null && commissions.Any())
                    {
                        commissionDetails.Commissions = commissions;
                        var groupbyUsers = commissionDetails.Commissions
                            .GroupBy(x => x.UserName)
                            .Select(g => new
                            {
                                UserName = g.Key,
                                AmountDueUSD = g.Sum(x => x.AmountDueUSD)
                            });

                        foreach (var item in groupbyUsers.OrderBy(i => i.UserName))
                        {
                            var commissionsummery = new CommissionSummery();
                            commissionsummery.AmountDueUSD = item.AmountDueUSD;
                            commissionsummery.UserName = item.UserName;
                            commissionDetails.CommissionsSummeryList.Add(commissionsummery);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.ToString());
            }
            return View(commissionDetails);
        }

        /// <summary>
        /// Payments will be in USD.
        //If sum([ArgInvoices.Payments].PaymentAmount)  is greater than or equal to [ArgInvoices.BalanceDues].AmountDueUSD then:
        //---Set ARGInvoices.InvoiceStatus ='Closed'
        //---BalanceDues.InvoiceStatus='Closed'
        //---Record in CollectionComments---> InvoiceStatus set from ARGInv to Closed
        /// </summary>
        /// <param name="invPaymentAmountDetails"></param>
        /// <returns></returns>
        
        [HttpPost]
        public JsonResult SaveInvoicePayments(InvoicePaymentAmtDetails invPaymentAmountDetails)
        {
            var ajaxResult = new AjaxResult();
            try
            {
                var invoiceInfo = Common.ArgInvoices.GetArgInvoice(invPaymentAmountDetails.InvoiceInfo.InvoiceId, "", invPaymentAmountDetails.InvoiceInfo.CompanyId) ?? new Arg.DataModels.ArgInvoice();

                //var invoiceInfo = Common.ArgInvoices.GetArgInvoice(invPaymentAmountDetails.InvoiceInfo.InvoiceId, "", arg.DataAccess.ActiveClient.Info.CompanyId) ?? new Arg.DataModels.ArgInvoice();
                var oldInvoiceStatus = invoiceInfo.InvoiceStatus;

                var invoiceBDInfo = Common.ArgInvoicesBD.GetInvoicesBD(0, invoiceInfo.Invoice, invoiceInfo.CompanyId) ?? new Arg.DataModels.ArgInvoices_BalanceDues();

                // var invoiceBDInfo = Common.ArgInvoicesBD.GetInvoicesBD(0, invoiceInfo.Invoice, arg.DataAccess.ActiveClient.Info.CompanyId) ?? new Arg.DataModels.ArgInvoices_BalanceDues();
                //var bdInfo = Common.BalanceDues.GetBalanceDue(0, invoiceBDInfo.Bol) ?? new Arg.DataModels.BalanceDue();
                var bdInfo = Common.BalanceDues.GetBalanceDuesByInvoice(invoiceInfo.Invoice, invoiceInfo.CompanyId) ?? new List<Arg.DataModels.BalanceDue>();

                // var bdInfo = Common.BalanceDues.GetBalanceDuesByInvoice(invoiceInfo.Invoice, arg.DataAccess.ActiveClient.Info.CompanyId) ?? new List<Arg.DataModels.BalanceDue>();
                //if (oldInvoiceStatus != "Open")
                //{
                //    ajaxResult.Message = "Invoice Status is not open, so payment amount cannot be included";
                //    return Json(ajaxResult);
                //}

                if (invPaymentAmountDetails.InvoicePaymentAmount > 0)
                {
                    var paymentInfo = new ArgInvoices_Payments
                    {
                        CompanyId = invoiceInfo.CompanyId,
                        Region = invoiceInfo.Region,
                        Invoice = invoiceInfo.Invoice,
                        InvoiceDate = invoiceInfo.InvoiceDate,
                        PaymentAmount = invPaymentAmountDetails.InvoicePaymentAmount,
                        PaymentDate = invPaymentAmountDetails.PaymentDate.Date,
                        PaymentMethod = invPaymentAmountDetails.PaymentMethod,
                        PaymentReference = invPaymentAmountDetails.PaymentReference
                    };

                    Common.ArgInvoicePayments.SaveInvoicesPayments(paymentInfo);

                    if (paymentInfo.PaymentId > 0)
                    {
                        Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Saved, invoiceInfo.CompanyId, "ArgInvoice Payments");
                        ajaxResult.InvoicePayments = paymentInfo;
                        ajaxResult.Message = "ArgInvoice Payment added!<br/>";
                    }

                    var payments = Common.ArgInvoicePayments.GetInvoicePayments(invoiceInfo.Invoice, invoiceInfo.CompanyId);
                    invPaymentAmountDetails.TotalPaymentAmount = payments.Select(x => x.PaymentAmount).Sum();
                    //invPaymentAmountDetails.TotalPaymentAmount += invPaymentAmountDetails.InvoicePaymentAmount;

                    if (invPaymentAmountDetails.TotalPaymentAmount >= invoiceBDInfo.AmountDueUSD)
                    {
                        //ArgInvoices
                        invoiceInfo.InvoiceStatus = "Closed";
                        Common.ArgInvoices.SaveArgInvoice(invoiceInfo);

                        if (invoiceInfo.InvoiceId > 0)
                        {
                            Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Updated, invoiceInfo.CompanyId, "ArgInvoices");
                            ajaxResult.Message += "ArgInvoice Updated!<br/>";
                        }

                        //BalanceDues
                        if (bdInfo != null && bdInfo.Any())
                        {
                            var idx = 0;

                            foreach (var item in bdInfo)
                            {
                                item.InvoiceStatus = "Closed";
                                Common.BalanceDues.SaveBalanceDue(item);

                                if (item.BalanceId > 0)
                                {
                                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Updated, item.CompanyId, "Balance Dues");
                                }

                                AddCollectionComment(item, "InvoiceStatus set from " + oldInvoiceStatus + " to " + invoiceInfo.InvoiceStatus);
                                idx++;
                            }
                            ajaxResult.Message += idx + " Balance Dues Updated!";
                        }
                        else
                        {
                            ajaxResult.Message += "No Balance Dues found with this Invoice!";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                ajaxResult.Message = ex.ToString();
            }
            return Json(ajaxResult);
        }

        public class InvoiceOutput
        {
        }

        public class ResultStats
        {
            public List<InvoiceOutput> InvoiceOutput { get; set; }
        }

        private class GenerateInvoicePDF_Result
        {
            public string ServerPath { get; set; }
            public string InvoiceUrl { get; set; }
        }

        public class AjaxResult
        {
            public string Message { get; set; }
            public ArgInvoices_Payments InvoicePayments { get; set; }
        }

        public class BDCountInfo
        {
            public int TotalBol { get; set; }
            public int CollectionCommentCount { get; set; }
            public int UpdatedBDCount { get; set; }
            public int InvoicesCommissionCount { get; set; }
            public int InvoicesBDCount { get; set; }
            public decimal TotalPaymentAmount { get; set; }
            public string Currency { get; set; }
            public string BdIds { get; set; }
            public string Message { get; set; }
            public SelectList InvoiceTypes { get; set; }
            public decimal ConversationRate { get; set; }
        }

    }
}
