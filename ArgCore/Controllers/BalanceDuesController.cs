using Arg.DataModels;
using ArgCore.Attributes;
using ArgCore.Helpers;
using ArgCore.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Reporting.WebForms;
using Rotativa.AspNetCore;
using SmartFormat;
using System.Diagnostics;
using System.Text;

namespace ArgCore.Controllers
{
    [AuthorizeUser]
    public class BalanceDuesController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public BalanceDuesController( IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index_v1(string q)
        {
            var balanceDues = new BalanceDues();

            try
            {
                if (HttpContext.Session.GetString("IsSessionActive") == null)
                {
                    return RedirectToAction("LogIn", "Account");
                }
                balanceDues.CommonObjects.TopHeading = "Balance Dues";

                var balDues = new Arg.DataAccess.BalanceDuesImpl().GetBalanceDues(0, (!string.IsNullOrWhiteSpace(q) ? q : ""), "");
                if (balDues != null && balDues.Any())
                {
                    balanceDues.BalanceDuesList = balDues;
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return View(balanceDues);
        }

        public IActionResult CollectionStatusDetails(int balanceId)
        {
            var statusDetails = new CollectionStatusDetails();

            try
            {
                var bdInfo = Common.BalanceDues.GetBalanceDue(balanceId, "", Common.GetActiveClientId());

                if (bdInfo != null && bdInfo.BalanceId > 0)
                {
                    statusDetails.BalanceDueInfo = bdInfo;
                    var customerInfo = Common.Customers.GetCustomer(0, Common.GetActiveClientId(), bdInfo.CustomerId);

                    if (customerInfo != null && !string.IsNullOrWhiteSpace(customerInfo.CustomerId))
                    {
                        statusDetails.CustomerName = customerInfo.CustomerName;
                    }
                }

                var collStatuses = Common.CollectionStatuses.GetStatuses(Arg.DataAccess.ActiveClient.Info.CompanyId);
                if (statusDetails.BalanceDueInfo.CollectionStatus != "Closed")
                {
                    collStatuses = collStatuses.Where(x => x.CollectionStatus != "Closed").ToList();
                }

                statusDetails.CollectionStatuses = new SelectList(collStatuses, "CollectionStatus", "CollectionStatus");
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.ToString());
            }
            return View(statusDetails);
        }

        public PartialViewResult LoadCollectionComments(int balanceId)
        {
            var comments = new CollectionComments();

            try
            {
                comments.BalanceId = balanceId;

                var bdInfo = Common.BalanceDues.GetBalanceDue(balanceId, "", Common.GetActiveClientId());
                if (bdInfo != null && bdInfo.BalanceId > 0)
                {
                    var collectionComments = Common.CollectionComments.GetCollectionComments(0, "", bdInfo.Bol).OrderByDescending(x => x.DateTime).ToList();
                    if (collectionComments != null && collectionComments.Any())
                    {
                        comments.CollectionCommentsList = collectionComments;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.ToString());
            }
            return PartialView(comments);
        }

        public IActionResult InvoiceStatusDetails(int balanceId)
        {
            var statusDetails = new InvoiceStatusDetails();

            try
            {
                var bdInfo = Common.BalanceDues.GetBalanceDue(balanceId, "", Common.GetActiveClientId());

                if (bdInfo != null && bdInfo.BalanceId > 0)
                {
                    statusDetails.BalanceDueInfo = bdInfo;
                    var customerInfo = Common.Customers.GetCustomer(0, Common.GetActiveClientId(), bdInfo.CustomerId);

                    if (customerInfo != null && !string.IsNullOrWhiteSpace(customerInfo.CustomerId))
                    {
                        statusDetails.CustomerName = customerInfo.CustomerName;
                    }

                    var invoiceStatuses = Common.BalanceDues.GetDistinctInvoiceStatus(0);
                    statusDetails.InvoiceStatuses = new SelectList(invoiceStatuses, "InvoiceStatus", "InvoiceStatus");
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.ToString());
            }
            return View(statusDetails);
        }

        public IActionResult ClientGLStatusDetails(int balanceId)
        {
            var gLStatusDetails = new ClientGLStatusDetails();

            try
            {
                var bdInfo = Common.BalanceDues.GetBalanceDue(balanceId, "", Common.GetActiveClientId());

                if (bdInfo != null && bdInfo.BalanceId > 0)
                {
                    gLStatusDetails.BalanceDueInfo = bdInfo;
                    var customerInfo = Common.Customers.GetCustomer(0, Common.GetActiveClientId(), bdInfo.CustomerId);

                    if (customerInfo != null && !string.IsNullOrWhiteSpace(customerInfo.CustomerId))
                    {
                        gLStatusDetails.CustomerName = customerInfo.CustomerName;
                    }

                    var collectionComments = Common.CollectionComments.GetCollectionComments(0, "", bdInfo.Bol).OrderByDescending(x => x.DateTime).ToList();

                    if (collectionComments != null && collectionComments.Any())
                    {
                        gLStatusDetails.CollectionComments = collectionComments;
                    }

                    var clientGlStatuses = Common.BalanceDues.GetDistinctClientGLStatus(bdInfo.CompanyId);
                    gLStatusDetails.ClientGLStatuses = new SelectList(clientGlStatuses, "ClientGlStatus", "ClientGlStatus");
                    var closeReasonCodes = Common.BalanceDues.GetDistinctCloseReasonCodes(bdInfo.CompanyId);
                    gLStatusDetails.CloseReasonCodes = new SelectList(closeReasonCodes, "CloseReasonCode", "CloseReasonCode");
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.ToString());
            }
            return View(gLStatusDetails);
        }

        public IActionResult BDDetails(int balanceId)
        {
            var bDDetails = new BDDetails();

            try
            {
                var bdInfo = Common.BalanceDues.GetBalanceDue(balanceId);

                if (bdInfo != null && bdInfo.BalanceId > 0)
                {
                    bDDetails.BalanceDueInfo = bdInfo;

                    if (Common.CanRunAction.EditBDRevenueAnalystFields)
                    {
                        var revAnalAuditor = Common.BalanceDues.GetDistinctRevenueAnalystAuditor(bDDetails.BalanceDueInfo.CompanyId);
                        bDDetails.RevenueAnalystAuditors = new SelectList(revAnalAuditor, "RevenueAnalystAuditor", "FullName");
                        var revAnalCollectors = Common.BalanceDues.GetDistinctRevenueAnalystCollector(bDDetails.BalanceDueInfo.CompanyId);
                        bDDetails.RevenueAnalystCollectors = new SelectList(revAnalCollectors, "RevenueAnalystCollector", "FullName");
                    }

                    var bdErrorCodeInfo = Common.BDErrorCodes.GetErrorCode(0, 0, bdInfo.BDErrorCode);
                    if (bdErrorCodeInfo != null && bdErrorCodeInfo.ErrorCodeId > 0)
                    {
                        bDDetails.BdErrorCodeDesc = bdErrorCodeInfo.Description;
                    }

                    var bdItems = Common.BOLHeader.GetBalanceDueItems(bDDetails.BalanceDueInfo.Bol, bDDetails.BalanceDueInfo.CompanyId);
                    if (bdItems != null && bdItems.Any())
                    {
                        bDDetails.BDItems = bdItems;
                    }

                    var bdOtherCharges = Common.BalanceDuesOtherCharges.GetBalanceDuesOtherChargesWithDesc(bDDetails.BalanceDueInfo.Bol, bDDetails.BalanceDueInfo.CompanyId);
                    if (bdOtherCharges != null && bdOtherCharges.Any())
                    {
                        bDDetails.BDOtherCharges = bdOtherCharges;
                    }

                    decimal totalCharges = 0;
                    var desc = new StringBuilder();
                    desc.AppendFormat("Supplemental Billing for BOL {0}:", bDDetails.BalanceDueInfo.Bol);
                    desc.AppendFormat("\n\n{0}. Corrected charges are below.", bDDetails.BdErrorCodeDesc);

                    if (bdItems != null && bdItems.Any())
                    {
                        foreach (var item in bdItems)
                        {
                            desc.AppendFormat("\nOcean charge (Commodity Code: {2}; Container {1}): {0}", item.AmountDue, item.Container, item.Commodity.Trim());
                            totalCharges += item.AmountDue;
                        }
                    }

                    if (bdOtherCharges != null && bdOtherCharges.Any())
                    {
                        foreach (var item in bdOtherCharges)
                        {
                            desc.AppendFormat("\n{0}: {1}", item.ChargeCodeDesc, item.AmountDue);
                            totalCharges += item.AmountDue;
                        }
                    }

                    desc.AppendFormat("\n\nTotal charge: {0}", totalCharges);
                    //decimal amountPaid = Common.BalanceDues.GetBDpaymentAmount(data.BalanceDueInfo.Bol);
                    decimal amountPaid = 0;
                    var bdPaymentAmnt = Common.BalanceDuesPayments.GetBalanceDuesPaymentAmnt(bDDetails.BalanceDueInfo.Bol, bdInfo.CompanyId);
                    amountPaid = bdPaymentAmnt;
                    desc.AppendFormat("\nAmount paid: {0}", amountPaid);//data.BalanceDueInfo.BalanceDueAmountPaid
                    var amntDue = totalCharges - amountPaid;
                    desc.AppendFormat("\nAmount due: {0}", amntDue);//data.BalanceDueInfo.BalanceDueAmount
                    bDDetails.BDDesc = desc;
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.ToString());
            }
            return View(bDDetails);
        }

        public IActionResult BDPaymentAmountDetails(int balanceId)
        {
            var data = new BDPaymentAmountDetails();

            try
            {
                var bdInfo = Common.BalanceDues.GetBalanceDue(balanceId, "", Common.GetActiveClientId());

                if (bdInfo != null && bdInfo.BalanceId > 0)
                {
                    data.BalanceDueInfo = bdInfo;
                    data.Payor = bdInfo.CustomerId;
                    data.Currency = bdInfo.Currency;
                    data.PaymentDate = DateTime.Now;

                    var payments = Common.BalanceDuesPayments.GetBalanceDuesPayments(bdInfo.Bol, bdInfo.CompanyId, bdInfo.BalanceDueInvoice, bdInfo.BalanceDueInvoiceDate);
                    if (payments != null && payments.Any())
                    {
                        data.BDPayments = payments;
                    }

                    var invoiceStatuses = Common.BalanceDues.GetDistinctInvoiceStatus(0);//todo confirm companyid
                    if (invoiceStatuses != null)
                    {
                        data.InvoiceStatuses = new SelectList(invoiceStatuses, "InvoiceStatus", "InvoiceStatus");
                    }

                    var customers = Common.BalanceDuesPayments.GetBDPaymentCustomers();
                    data.Customers = new SelectList(customers, "CustomerId", "CustomerName");
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.ToString());
            }
            return View(data);
        }

        [HttpPost]
        public JsonResult SaveBDPayments(BDPaymentAmountDetails bdPaymentAmountDetails)
        {
            var ajaxResult = new AjaxResult();

            try
            {
                var bdInfo = Common.BalanceDues.GetBalanceDue(bdPaymentAmountDetails.BalanceDueInfo.BalanceId, "", Common.GetActiveClientId()) ?? new BalanceDue();
                var oldStatus = bdInfo.InvoiceStatus;
                var oldClientGLStatus = bdInfo.ClientGlStatus;

                if (bdPaymentAmountDetails.BDPaymentAmount > 0)
                {
                    var paymentInfo = new BalanceDues_Payments
                    {
                        CompanyID = bdInfo.CompanyId,
                        Region = bdInfo.Region,
                        CustomerID = bdInfo.CustomerId,
                        CustomerLocationCode = bdInfo.CustomerLocationCode,
                        BookingID = bdInfo.BookingId,
                        BOLNo = bdInfo.Bol,
                        Currency = bdInfo.Currency,
                        BOLExecutionDate = bdInfo.BolExecutionDate,
                        BalanceDueInvoiceNo = bdInfo.BalanceDueInvoice,
                        BalanceDueInvoiceDate = Convert.ToDateTime(bdInfo.BalanceDueInvoiceDate),
                        Payor = bdPaymentAmountDetails.Payor,
                        PaymentAmount = bdPaymentAmountDetails.BDPaymentAmount,
                        PaymentDate = bdPaymentAmountDetails.PaymentDate.Date,
                        PaymentType = bdPaymentAmountDetails.PaymentType,
                        PaymentReference = bdPaymentAmountDetails.PaymentReference
                    };

                    Common.BalanceDuesPayments.SaveBalanceDuesPayment(paymentInfo);

                    if (paymentInfo.PaymentId > 0)
                    {
                        Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Saved, paymentInfo.CompanyID, "Balance Dues Payments");
                        ajaxResult.BDPayments = paymentInfo;
                        var companyName = Common.ArgClients.GetArgClient(paymentInfo.CompanyID, "").Name;
                        ajaxResult.BDPayments.Company = companyName;
                    }

                    ajaxResult.Message = "Balance Due Payment added!<br/>";

                    var payments = Common.BalanceDuesPayments.GetBalanceDuesPayments(bdInfo.Bol, bdInfo.CompanyId, bdInfo.BalanceDueInvoice, bdInfo.BalanceDueInvoiceDate);
                    bdPaymentAmountDetails.TotalPaymentAmount = payments.Select(x => x.PaymentAmount).Sum();

                    if (bdPaymentAmountDetails.TotalPaymentAmount < bdInfo.BalanceDueAmount)
                    {
                        bdInfo.InvoiceStatus = "Short_Paid";
                        var result = UpdateBalanceDue(bdInfo);
                        if (result.BalanceId > 0)
                        {
                            AddCollectionComment(bdInfo, "InvoiceStatus changed from " + oldStatus + " to " + bdInfo.InvoiceStatus);
                            ajaxResult.Message += "Balance Due Updated!<br/>";
                        }
                    }
                    else if (bdPaymentAmountDetails.TotalPaymentAmount >= bdInfo.BalanceDueAmount)
                    {
                        bdInfo.InvoiceStatus = "Paid";
                        bdInfo.ClientGlStatus = "Closed";
                        bdInfo.CollectionStatus = "Closed";
                        bdInfo.CloseReasonCode = "Paid";
                        var result = UpdateBalanceDue(bdInfo);
                        if (result.BalanceId > 0)
                        {
                            ajaxResult.Message += "Balance Due Updated!";
                            AddCollectionComment(bdInfo, "InvoiceStatus changed from " + oldStatus + " to " + bdInfo.InvoiceStatus);
                            AddCollectionComment(bdInfo, "ClientGLStatus changed from " + oldClientGLStatus + " to " + bdInfo.ClientGlStatus);
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
                    BookingId = bdInfo.BookingId,
                    Comments = comments
                };
                Common.CollectionComments.SaveCollectionComment(collectionCommentInfo);
                if (collectionCommentInfo.CollectionId > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Saved, collectionCommentInfo.CompanyId, "Collection Comment");
                }
                return collectionCommentInfo;
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return null;
        }

        public BalanceDue UpdateBalanceDue(BalanceDue bdInfo)
        {
            try
            {
                Common.BalanceDues.SaveBalanceDue(bdInfo);
                if (bdInfo.BalanceId > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Updated, bdInfo.CompanyId, "Balance Dues");
                    return bdInfo;
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return null;
        }

        public JsonResult UpdateBDCollectionStatus(int balanceId, string status)//CollectionStatusDetails collStatDetails)
        {
            try
            {
                var bdInfo = Common.BalanceDues.GetBalanceDue(balanceId, "", Common.GetActiveClientId());
                bdInfo.CollectionStatus = status;
                var result = UpdateBalanceDue(bdInfo);

                if (result.BalanceId > 0)
                {
                     return Json("Status Updated!");
                } 
                else
                {
                    return Json("Problem updating status!");
                }
                    
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                return Json(ex);
            }
        }

        public JsonResult UpdateBDRevAnalyst(int balanceId, string revenueAnalyst, bool isAuditor)
        {
            try
            {
                var bdInfo = Common.BalanceDues.GetBalanceDue(balanceId, "", Common.GetActiveClientId());
                if (isAuditor)
                {
                    bdInfo.RevenueAnalystAuditor = revenueAnalyst;
                } 
                else
                {
                    bdInfo.RevenueAnalystCollector = revenueAnalyst;

                }
                    
                var result = UpdateBalanceDue(bdInfo);
                if (result.BalanceId > 0)
                {
                    return Json("RevenueAnalyst Updated!");
                }   
                else
                {
                    return Json("Problem updating RevenueAnalyst!");
                }
                    
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                return Json(ex);
            }
        }

        public JsonResult UpdateStatusByAmountPaid(int balanceId, string newStatus)
        {
            var ajaxResult = new AjaxResult();
            try
            {
                var bdInfo = Common.BalanceDues.GetBalanceDue(balanceId, "", Common.GetActiveClientId());
                if (bdInfo != null && bdInfo.BalanceId > 0)
                {
                    var oldStatus = bdInfo.InvoiceStatus;
                    if (oldStatus.ToLower() == "invoiced_nr" && newStatus.ToLower() == "invoiced_rec")
                    {
                        bdInfo.ClientGlStatus = "Open";
                        bdInfo.InvoiceStatus = newStatus;

                        var result = UpdateBalanceDue(bdInfo);
                        if (result.BalanceId > 0)
                        {
                            ajaxResult.Message = "Status Updated!";
                        }
                            
                        AddCollectionComment(bdInfo, "InvoiceStatus changed from " + oldStatus + " to " + newStatus);
                        AddCollectionComment(bdInfo, "ClientGLStatus changed from Closed to Open");
                    }
                    else
                    {
                        ajaxResult.Message = "You cannot change the status from " + oldStatus + " to " + newStatus + "!";
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                ajaxResult.Message = ex.ToString();
            }
            return Json(ajaxResult.Message);
        }

        public JsonResult UpdateBDInvoiceStatus(int balanceId, string oldStatus, string newStatus, string invoiceNo, bool useExisting = false)
        {
            var ajaxResult = new AjaxResult();

            try
            {
                var bdInfo = Common.BalanceDues.GetBalanceDue(balanceId, "", Common.GetActiveClientId());

                if (string.IsNullOrWhiteSpace(oldStatus))
                {
                    oldStatus = bdInfo.InvoiceStatus;
                }

                if (string.IsNullOrWhiteSpace(invoiceNo) && !(Arg.DataAccess.ActiveClient.Info.DBName.Contains("Ceva")))
                {
                    invoiceNo = Common.BalanceDues.GenerateInvNoFromInvSummaryBasedOnBOL(bdInfo.Bol);

                    if (string.IsNullOrWhiteSpace(invoiceNo))
                    {
                        Trace.TraceError("Invoice No. can't be empty");
                        ajaxResult.MissingInvoiceBOL = bdInfo.Bol;
                    }
                }

                var bdCloseReasonCode = "";
                ajaxResult.ShowCloseReasonCode = false;

                if (bdInfo != null && bdInfo.BalanceId > 0)
                {
                    bdCloseReasonCode = bdInfo.CloseReasonCode;

                    if (oldStatus.ToLower() == "pending" && newStatus.ToLower() == "closed")
                    {
                        bdInfo.InvoiceStatus = newStatus;
                        bdInfo.CollectionStatus = newStatus;
                        bdInfo.CloseReasonCode = "NBIL";
                        var result = UpdateBalanceDue(bdInfo);

                        if (result.BalanceId > 0)
                        {
                            ajaxResult.Message = "Status  Updated!";
                        }

                        AddCollectionComment(bdInfo, "InvoiceStatus set from 'Pending' to 'Closed'");
                        AddCollectionComment(bdInfo, "CollectionStatus set to 'Closed'");
                        ajaxResult.ChangesDone = "<b>Fields changed in BD:</b><br /><b>Invoice Status: </b>" + newStatus + "<br /><b>Collection Status: </b>" + newStatus + "<br /><b>Close Reason Code: </b>NBIL<br />Added 2 records in Collection Comments";
                    }
                    else if (oldStatus.ToLower() == "pending_approval" && newStatus.ToLower() == "closed")
                    {
                        bdInfo.InvoiceStatus = newStatus;
                        bdInfo.CollectionStatus = newStatus;
                        bdInfo.CloseReasonCode = "NBIL";
                        var result = UpdateBalanceDue(bdInfo);

                        if (result.BalanceId > 0)
                        {
                            ajaxResult.Message = "Status  Updated!";
                        }

                        AddCollectionComment(bdInfo, "InvoiceStatus set from 'Pending_Approval' to 'Closed'");
                        AddCollectionComment(bdInfo, "CollectionStatus set to 'Closed'");
                        ajaxResult.ChangesDone = "<b>Fields changed in BD:</b><br /><b>Invoice Status: </b>" + newStatus + "<br /><b>Collection Status: </b>" + newStatus + "<br /><b>Close Reason Code: </b>NBIL<br />Added 2 records in Collection Comments";
                    }
                    else if (oldStatus.ToLower() == "invoiced_nr" && newStatus.ToLower() == "closed")
                    {
                        bdInfo.InvoiceStatus = newStatus;
                        bdInfo.CollectionStatus = newStatus;
                        ajaxResult.ShowCloseReasonCode = true;
                        var result = UpdateBalanceDue(bdInfo);

                        if (result.BalanceId > 0)
                        {
                            ajaxResult.Message = "Status  Updated!";
                        }

                        AddCollectionComment(bdInfo, "InvoiceStatus set from 'Invoiced_NR' to 'Closed'");
                        AddCollectionComment(bdInfo, "CollectionStatus set to 'Closed'");
                        ajaxResult.ChangesDone = "<b>Fields changed in BD:</b><br /><b>Invoice Status: </b>" + newStatus + "<br /><b>Collection Status: </b>" + newStatus + "<br />Added 2 records in Collection Comments";
                    }
                    else if (oldStatus.ToLower() == "invoiced_rec" && newStatus.ToLower() == "closed")
                    {
                        bdInfo.InvoiceStatus = newStatus;
                        bdInfo.CollectionStatus = newStatus;
                        ajaxResult.ShowCloseReasonCode = true;
                        var result = UpdateBalanceDue(bdInfo);

                        if (result.BalanceId > 0)
                        {
                            ajaxResult.Message = "Status Updated!";
                        }

                        AddCollectionComment(bdInfo, "InvoiceStatus set from 'Invoiced_REC' to 'Closed'");
                        AddCollectionComment(bdInfo, "CollectionStatus set to 'Closed'");
                        ajaxResult.ChangesDone = "<b>Fields changed in BD:</b><br /><b>Invoice Status: </b>" + newStatus + "<br /><b>Collection Status: </b>" + newStatus + "<br />Added 2 records in Collection Comments";
                    }
                    else if (oldStatus.ToLower() == "arginv" && newStatus.ToLower() == "invoiced_nr")
                    {
                        var bdPaymentsCount = Common.BalanceDuesPayments.GetBDPayCount(bdInfo.CompanyId, bdInfo.Region, bdInfo.CustomerId, bdInfo.BalanceDueInvoice, bdInfo.Bol);
                        if (bdPaymentsCount > 0)
                        {
                            Common.BalanceDuesPayments.DeleteBalanceDuesPayment(bdInfo.CompanyId, bdInfo.Region, bdInfo.CustomerId, bdInfo.BalanceDueInvoice, bdInfo.Bol);
                        }

                        var commissionsCount = Common.Commissions.GetCommCount(bdInfo.CompanyId, bdInfo.Region, bdInfo.CustomerId, bdInfo.Bol);
                        if (commissionsCount > 0)
                        {
                            Common.Commissions.DeleteComm(bdInfo.CompanyId, bdInfo.Region, bdInfo.CustomerId, bdInfo.Bol);
                        }

                        var argInvBDCount = Common.ArgInvoicesBD.GetArgInvBDCount(bdInfo.CompanyId, bdInfo.Region, bdInfo.CustomerId, bdInfo.BalanceDueInvoice, bdInfo.Bol);
                        if (argInvBDCount > 0)
                        {
                            Common.ArgInvoicesBD.DeleteArgInvBD(bdInfo.CompanyId, bdInfo.Region, bdInfo.CustomerId, bdInfo.Bol);
                        }

                        bdInfo.InvoiceStatus = newStatus;
                        bdInfo.ClientGlStatus = "Closed";
                        var result = UpdateBalanceDue(bdInfo);

                        if (result.BalanceId > 0)
                        {
                            ajaxResult.Message = "Status Updated!";
                        }

                        AddCollectionComment(bdInfo, "InvoiceStatus set from 'ARGInv' to 'Invoiced_NR'");
                        ajaxResult.ChangesDone = bdPaymentsCount + " records deleted from BalanceDues.Payments<br/>" + commissionsCount + " records deleted from Commissions<br/>" + argInvBDCount + " records deleted from ArgInvoices.BalanceDues<br/><b>Fields changed in BD:</b><br /><b>Invoice Status: </b>" + newStatus + "<br /><b>ClientGL Status: Closed</b><br />Added 1 record in Collection Comments";
                    }
                    else if (oldStatus.ToLower() == "arginv" && newStatus.ToLower() == "invoiced_rec")
                    {
                        var bdPaymentsCount = Common.BalanceDuesPayments.GetBDPayCount(bdInfo.CompanyId, bdInfo.Region, bdInfo.CustomerId, bdInfo.BalanceDueInvoice, bdInfo.Bol);
                        if (bdPaymentsCount > 0)
                        {
                            Common.BalanceDuesPayments.DeleteBalanceDuesPayment(bdInfo.CompanyId, bdInfo.Region, bdInfo.CustomerId, bdInfo.BalanceDueInvoice, bdInfo.Bol);
                        }

                        var commissionsCount = Common.Commissions.GetCommCount(bdInfo.CompanyId, bdInfo.Region, bdInfo.CustomerId, bdInfo.Bol);
                        if (commissionsCount > 0)
                        {
                            Common.Commissions.DeleteComm(bdInfo.CompanyId, bdInfo.Region, bdInfo.CustomerId, bdInfo.Bol);
                        }

                        var argInvBDCount = Common.ArgInvoicesBD.GetArgInvBDCount(bdInfo.CompanyId, bdInfo.Region, bdInfo.CustomerId, bdInfo.BalanceDueInvoice, bdInfo.Bol);
                        if (argInvBDCount > 0)
                        {
                            Common.ArgInvoicesBD.DeleteArgInvBD(bdInfo.CompanyId, bdInfo.Region, bdInfo.CustomerId, bdInfo.Bol);
                        }

                        bdInfo.InvoiceStatus = newStatus;
                        bdInfo.ClientGlStatus = "Open";
                        var result = UpdateBalanceDue(bdInfo);

                        if (result.BalanceId > 0)
                        {
                            ajaxResult.Message = "Status Updated!";
                        }

                        AddCollectionComment(bdInfo, "InvoiceStatus set from 'ARGInv' to 'Invoiced_REC'");
                        ajaxResult.ChangesDone = bdPaymentsCount + " records deleted from BalanceDues.Payments<br/>" + commissionsCount + " records deleted from Commissions<br/>" + argInvBDCount + " records deleted from ArgInvoices.BalanceDues<br/><b>Fields changed in BD:</b><br /><b>Invoice Status: </b>" + newStatus + "<br /><b>ClientGL Status: Open</b><br />Added 1 record in Collection Comments";
                    }
                    else if (oldStatus.ToLower() == "arginv" && newStatus.ToLower() == "paid")
                    {
                        var commissionsCount = Common.Commissions.GetCommCount(bdInfo.CompanyId, bdInfo.Region, bdInfo.CustomerId, bdInfo.Bol);
                        if (commissionsCount > 0)
                        {
                            Common.Commissions.DeleteComm(bdInfo.CompanyId, bdInfo.Region, bdInfo.CustomerId, bdInfo.Bol);
                        }

                        var argInvBDCount = Common.ArgInvoicesBD.GetArgInvBDCount(bdInfo.CompanyId, bdInfo.Region, bdInfo.CustomerId, bdInfo.BalanceDueInvoice, bdInfo.Bol);
                        if (argInvBDCount > 0)
                        {
                            Common.ArgInvoicesBD.DeleteArgInvBD(bdInfo.CompanyId, bdInfo.Region, bdInfo.CustomerId, bdInfo.Bol);
                        }

                        bdInfo.InvoiceStatus = newStatus;
                        var result = UpdateBalanceDue(bdInfo);

                        if (result.BalanceId > 0)
                        {
                            ajaxResult.Message = "Status Updated!";
                        }

                        AddCollectionComment(bdInfo, "InvoiceStatus set from 'ARGInv' to 'Paid'");
                        ajaxResult.ChangesDone = commissionsCount + " records deleted from Commissions<br/>" + argInvBDCount + " records deleted from ArgInvoices.BalanceDues<br/><b>Fields changed in BD:</b><br /><b>Invoice Status: </b>" + newStatus + "<br />Added 1 record in Collection Comments";
                    }
                    else if (oldStatus.ToLower() == "short_paid" && newStatus.ToLower() == "paid")
                    {
                        bdInfo.InvoiceStatus = newStatus;
                        bdInfo.CollectionStatus = "Closed";
                        bdInfo.CloseReasonCode = "SPAID";
                        var result = UpdateBalanceDue(bdInfo);

                        if (result.BalanceId > 0)
                        {
                            ajaxResult.Message = "Status  Updated!";
                        }

                        AddCollectionComment(bdInfo, "InvoiceStatus set from 'Short_Paid' to 'Paid'");
                        AddCollectionComment(bdInfo, "CollectionStatus set to 'Closed'");
                        ajaxResult.ChangesDone = "<b>Fields changed in BD:</b><br /><b>Invoice Status: </b>" + newStatus + "<br /><b>Collection Status: </b>Closed<br /><b>Close Reason Code: </b>SPAID<br />Added 2 records in Collection Comments";
                    }
                    else if (newStatus.ToLower() == "invoiced_rec")
                    {
                        if (oldStatus.ToLower() == "pending")
                        {
                            bdInfo.CollectionStatus = "No_Response";
                            bdInfo.BalanceDueInvoice = invoiceNo;
                            bdInfo.BalanceDueInvoiceDate = DateTime.Now;
                        }

                        bdInfo.ClientGlStatus = "Open";
                        bdInfo.InvoiceStatus = newStatus;
                        var result = UpdateBalanceDue(bdInfo);

                        if (result.BalanceId > 0) 
                        { 
                            ajaxResult.Message = "Status  Updated!";
                        }

                        AddCollectionComment(bdInfo, "InvoiceStatus set from " + oldStatus + " to " + newStatus);
                        AddCollectionComment(bdInfo, "ARGClientGLStatus set from Closed to Open");
                        ajaxResult.ChangesDone = "<b>Fields changed in BD:</b><br />";

                        if (oldStatus.ToLower() == "pending")
                        {
                            ajaxResult.ChangesDone += "<b>Collection Status: </b>No_Response<br /><b>BalanceDue Invoice: </b>" + invoiceNo + "<br /><b>BalanceDue Invoice Date: </b>" + DateTime.Now + "<br />";
                        }
                        ajaxResult.ChangesDone += "<b>Invoice Status: </b>" + newStatus + "<br /><b>Client GLStatus: </b>Open<br />Added 2 records in CollectionComments";
                    }
                    else if (newStatus.ToLower() == "paid" && bdInfo.BDErrorCode.StartsWith("OC"))
                    {
                        if (oldStatus.ToLower() == "pending" || oldStatus.ToLower() == "invoiced_rec" || oldStatus.ToLower() == "invoiced_nr")
                        {
                            bdInfo.CollectionStatus = "No_Response";
                            bdInfo.BalanceDueInvoice = invoiceNo;
                            bdInfo.BalanceDueInvoiceDate = DateTime.Now;

                            if (oldStatus.ToLower() == "pending" && newStatus.ToLower() == "paid")
                            {
                                bdInfo.ClientGlStatus = "Closed";
                            }
                            else
                            {
                                bdInfo.ClientGlStatus = "Open";
                            }
                            bdInfo.InvoiceStatus = newStatus;

                            if (bdInfo.InvoiceType == "BOL Overcharge")
                            {
                                AddBalanceDuePayment(bdInfo);
                            }
                            var result = UpdateBalanceDue(bdInfo);

                            if (result.BalanceId > 0)
                            {
                                ajaxResult.Message = "Status Updated!";
                            }

                            AddCollectionComment(bdInfo, "InvoiceStatus set from " + oldStatus + " to " + newStatus);
                            AddCollectionComment(bdInfo, "ARGClientGLStatus set from Closed to Open");
                            ajaxResult.ChangesDone = "<b>Fields changed in BD:</b><br />";

                            if (oldStatus.ToLower() == "pending")
                            {
                                ajaxResult.ChangesDone += "<b>Collection Status: </b>No_Response<br /><b>BalanceDue Invoice: </b>" + invoiceNo + "<br /><b>BalanceDue Invoice Date: </b>" + DateTime.Now + "<br />";
                            }
                            ajaxResult.ChangesDone += "<b>Invoice Status: </b>" + newStatus + "<br /><b>Client GLStatus: </b>Open<br />Added 2 records in CollectionComments";
                        }
                    }
                    else if (newStatus.ToLower() == "invoiced_nr")
                    {
                        if (oldStatus.ToLower() == "pending")
                        {
                            bdInfo.CollectionStatus = "No_Response";
                            bdInfo.BalanceDueInvoice = invoiceNo ?? "";
                            bdInfo.BalanceDueInvoiceDate = DateTime.Now;
                        }

                        bdInfo.InvoiceStatus = newStatus;
                        var result = UpdateBalanceDue(bdInfo);

                        if (result.BalanceId > 0)
                        {
                            ajaxResult.Message = "Status Updated!";
                        }

                        AddCollectionComment(bdInfo, "InvoiceStatus set from " + oldStatus + " to " + newStatus);
                        ajaxResult.ChangesDone = "<b>Fields changed in BD:</b><br />";

                        if (oldStatus.ToLower() == "pending")
                        {
                            ajaxResult.ChangesDone += "<b>Collection Status: </b>No_Response<br /><b>BalanceDue Invoice: </b>" + invoiceNo + "<br /><b>BalanceDue Invoice Date: </b>" + DateTime.Now + "<br />";
                        }
                        ajaxResult.ChangesDone += "<b>Invoice Status: </b>" + newStatus + "<br />Added 1 record in CollectionComments";
                    }
                    else if (oldStatus.ToLower() == "pending" && newStatus.ToLower() == "pending_approval")
                    {
                        bdInfo.InvoiceStatus = newStatus;
                        bdInfo.LastModifiedBy = User.Identity.GetUserId();
                        bdInfo.LastModified = DateTime.Now;
                        var result = UpdateBalanceDue(bdInfo);

                        if (result.BalanceId > 0)
                        {
                            ajaxResult.Message = "Status  Updated!";
                        }

                        AddCollectionComment(bdInfo, "InvoiceStatus set from 'Pending' to 'Pending_Approval'");
                        ajaxResult.ChangesDone = "<b>Fields changed in BD:</b><br /><b>Invoice Status: </b>" + newStatus + "<br /><b>Close Reason Code: </b>NBIL<br />Added 1 record in Collection Comments";
                    }
                    else
                    {
                        ajaxResult.Message = "You cannot change the status from " + oldStatus + " to " + newStatus + "!";
                    }
                }
                if (ajaxResult.ShowCloseReasonCode)
                {
                    var closeReasonCode = Common.CloseReasonCode.GetSelectedCloseReasonCodes(Arg.DataAccess.ActiveClient.Info.CompanyId);
                    ajaxResult.SelectedCloseReasonCodes = new SelectList(closeReasonCode, "CloseReasonCode", "CloseReasonCodeWithDesc", bdCloseReasonCode);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                Common.Log.Error(ex);
                ajaxResult.Message = ex.ToString();
            }
            return Json(ajaxResult);
        }

        public JsonResult UpdateBDCloseReasonCode(int balanceId, string closeReasonCode)
        {
            var ajaxResult = new AjaxResult();
            try
            {
                var bdInfo = Common.BalanceDues.GetBalanceDue(balanceId, "", Common.GetActiveClientId());
                bdInfo.CloseReasonCode = closeReasonCode;

                var result = UpdateBalanceDue(bdInfo);
                if (result.BalanceId > 0)
                {
                    ajaxResult.Message = "CloseReasonCode Updated!";
                }
                  
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                ajaxResult.Message = ex.ToString();
            }
            return Json(ajaxResult);
        }

        public JsonResult UpdateBDClientGLStatus(int balanceId, string oldStatus, string newStatus)
        //public JsonResult UpdateBDClientGLStatus(ClientGLStatusDetails model)
        {
            var ajaxResult = new AjaxResult();
            try
            {
                var bdInfo = Common.BalanceDues.GetBalanceDue(balanceId, "", Common.GetActiveClientId());
                if (oldStatus == "Open" && newStatus == "Closed")
                {
                    var oldInvoiceStatus = bdInfo.InvoiceStatus;
                    if (bdInfo.InvoiceStatus.ToLower() == "invoiced_rec" || bdInfo.InvoiceStatus.ToLower() == "paid" || bdInfo.InvoiceStatus.ToLower() == "arginv")
                    {
                        bdInfo.ClientGlStatus = newStatus;
                        bdInfo.InvoiceStatus = newStatus;
                        var result = UpdateBalanceDue(bdInfo);
                        if (result.BalanceId > 0)
                        {
                            AddCollectionComment(bdInfo, "ClientGLStatus set from 'Open' to 'Closed'");
                            AddCollectionComment(bdInfo, "InvoiceStatus set from '" + oldInvoiceStatus + "' to 'Closed'");
                            ajaxResult.Message = "Status Updated!";
                        }
                    }
                    else
                    {
                        ajaxResult.Message = "Client GL Status is not updated as Invoice Status is not Invoiced_REC or Paid or ARGInv";
                    }
                }
                else if (bdInfo.InvoiceStatus.ToLower() == "invoiced_nr")
                {
                    if (oldStatus == "Closed" && newStatus == "Open")
                    {
                        bdInfo.ClientGlStatus = newStatus;
                        bdInfo.InvoiceStatus = "Invoiced_REC";

                        var result = UpdateBalanceDue(bdInfo);
                        if (result.BalanceId > 0)
                        {
                            AddCollectionComment(bdInfo, "Changed InvoiceStatus from Invoiced_NR to Invoiced_REC");
                            AddCollectionComment(bdInfo, "Changed ClientGLStatus from 'Closed' to 'Open'");
                            ajaxResult.Message = "Status Updated!";
                        }
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

        public IActionResult Index(int? companyId)
        {
            try
            {
                var balanceDues = new BalanceDues();
                balanceDues.CompanyId = Arg.DataAccess.ActiveClient.Info.CompanyId;
                //data.CompanyId = Convert.ToInt32(companyId);
                if (balanceDues.SearchOptions != null && balanceDues.CompanyId > 0)
                {
                    balanceDues.SearchOptions.CompanyId = balanceDues.CompanyId;
                }

                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                LoadDataInModel(balanceDues);
                balanceDues.BalanceDuesDetail = new BalanceDue();
                return View(balanceDues);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                Common.Log.Error(ex);
            }
            return null;
        }

        public void LoadDataInModel(BalanceDues balanceDues)
        {
            balanceDues.CommonObjects.TopHeading = "Balance Dues";
            balanceDues.CommonObjects.Heading = "Manage Balance Dues";

            if (balanceDues.SearchOptions == null)
            {
                balanceDues.SearchOptions = new SearchOptions();
            }
                
            var companies = Common.ArgClients.GetArgClients(Common.CurrentUserId);
            balanceDues.Companies = new SelectList(companies, "CompanyId", "Name");

            if (balanceDues.CompanyId > 0)
            {
                var emailTemplates = Common.Templates.GetTemplatesList();
                balanceDues.EmailTemplates = new SelectList(emailTemplates, "TemplateId", "Name");

                var smtpUsers = Common.ClientSMTPAccounts.GetSmtpAccounts(balanceDues.CompanyId);
                balanceDues.SMTPUsers = new SelectList(smtpUsers, "SMTPAccountId", "UserName");

                var customers = Common.Customers.GetBalanceDueCustomers(balanceDues.CompanyId);
                balanceDues.Customers = new SelectList(customers, "CustomerId", "Customer", balanceDues.CompanyId);

                var regions = Common.Regions.GetBalanceDueRegions(balanceDues.CompanyId);
                balanceDues.Regions = new SelectList(regions, "Region", "Region", balanceDues.SearchOptions.Regions ?? new List<string>());

                var bdErrorCodes = Common.BDErrorCodes.GetDistinctErrorCodes(balanceDues.CompanyId, true);
                balanceDues.BDErrorCodes = new SelectList(bdErrorCodes, "BdErrorCode", "ErrorCodes");

                var bdOtherChargeCodes = Common.BDOtherChargesCodes.GetDistinctChargeCodes(balanceDues.CompanyId, true);
                balanceDues.BDOtherChargeCodes = new SelectList(bdOtherChargeCodes, "ChargeCode", "ChargeCodes");

                var invoiceNo = Common.BalanceDues.GetDistinctInvoiceNo(balanceDues.CompanyId);
                balanceDues.InvoiceNo = new SelectList(invoiceNo, "BalanceDueInvoice", "BalanceDueInvoice");

                var invoiceTypes = Common.BalanceDues.GetDistinctInvoiceTypes(balanceDues.CompanyId);
                balanceDues.InvoiceTypes = new SelectList(invoiceTypes, "InvoiceType", "InvoiceType");

                var moveTypes = Common.BalanceDues.GetDistinctMoveTypes(balanceDues.CompanyId);
                balanceDues.MoveTypes = new SelectList(moveTypes, "MoveType", "MoveType");

                var closeReasonCodes = Common.BalanceDues.GetDistinctCloseReasonCodes(balanceDues.CompanyId);
                balanceDues.CloseReasonCodes = new SelectList(closeReasonCodes, "CloseReasonCode", "CloseReasonCode");

                var vessels = Common.BalanceDues.GetDistinctVessels(balanceDues.CompanyId);
                balanceDues.Vessels = new SelectList(vessels, "Vessel", "Vessel");

                var voyages = Common.BalanceDues.GetDistinctVoyages(balanceDues.CompanyId);
                balanceDues.Voyages = new SelectList(voyages, "Voyage", "Voyage");

                var invoiceStatus = Common.BalanceDues.GetDistinctInvoiceStatus(balanceDues.CompanyId);
                balanceDues.InvoiceStatus = new SelectList(invoiceStatus, "InvoiceStatus", "InvoiceStatus");

                var collectionStatus = Common.BalanceDues.GetDistinctCollectionStatus(balanceDues.CompanyId);
                balanceDues.CollectionStatus = new SelectList(collectionStatus, "CollectionStatus", "CollectionStatus");

                var clientGlStatus = Common.BalanceDues.GetDistinctClientGLStatus(balanceDues.CompanyId);
                balanceDues.ClientGlStatus = new SelectList(clientGlStatus, "ClientGlStatus", "ClientGlStatus");

                var originLocationCodes = Common.BalanceDues.GetDistinctOriginLocationCodes(balanceDues.CompanyId);
                balanceDues.OriginLocationCodes = new SelectList(originLocationCodes, "OriginLocationCode", "OriginLocationCode");

                var desLocationCodes = Common.BalanceDues.GetDistinctDestinationLocationCodes(balanceDues.CompanyId);
                balanceDues.DestinationLocationCodes = new SelectList(desLocationCodes, "DestinationLocationCode", "DestinationLocationCode");

                var pol = Common.BalanceDues.GetDistinctPOL(balanceDues.CompanyId);
                balanceDues.POL = new SelectList(pol, "PortOfLoading", "PortOfLoading");

                var pod = Common.BalanceDues.GetDistinctPOD(balanceDues.CompanyId);
                balanceDues.POD = new SelectList(pod, "PortOfDischarge", "PortOfDischarge");

                var revAnalAuditor = Common.BalanceDues.GetDistinctRevenueAnalystAuditor(balanceDues.CompanyId);
                balanceDues.RevenueAnalystAuditors = new SelectList(revAnalAuditor, "RevenueAnalystAuditor", "FullName");

                var revAnalCollectors = Common.BalanceDues.GetDistinctRevenueAnalystCollector(balanceDues.CompanyId);
                balanceDues.RevenueAnalystCollectors = new SelectList(revAnalCollectors, "RevenueAnalystCollector", "FullName");
                //}
            }
        }

        [AllowAnonymous]
        public JsonResult GetBDErrorCodes(string invoiceType)
        {
            var bdErrorCodes = Common.BDErrorCodes.GetDistinctErrorCodes(Common.GetActiveClientId(), false, "", invoiceType);
            return Json(bdErrorCodes);
        }

        [AllowAnonymous]
        public JsonResult GetBdDescriptions(string invoiceType)
        {
            var bdDescs = Common.BalanceDuesDescriptions.GetBalanceDuesDesc(Common.GetActiveClientId(), invoiceType);
            return Json(bdDescs);
        }

        private bool CreatePdfFile(string fullPath, string viewName, object data)
        {
            string footerPath = Path.Combine(_hostingEnvironment.ContentRootPath, "Views", "BalanceDues", "InvoicePDFFooter.html");//todo
            //string customSwitches = string.Format("--footer-html \"{0}\" --margin-bottom \"25mm\" ", footerPath);
            string customSwitches = $"--footer-html \"{footerPath}\" --margin-bottom \"25mm\" ";
            var folder = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            var pdfOptions = new ViewAsPdf(viewName, data) 
            { 
                CustomSwitches = customSwitches
            };

            Task<byte[]> pdfTask = pdfOptions.BuildFile(ControllerContext);
            byte[] byteArray = pdfTask.GetAwaiter().GetResult();


            using (var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            {
                fileStream.Write(byteArray, 0, byteArray.Length);
                fileStream.Close();
            }
            return true;
        }

        public CustomerContactInfo GenerateEmailDraft(int smtpAccountId, string customerId, int templateId, string attachFileServerPath, string invoiceNo, int balanceId)
        {
            var contactInfo = new CustomerContactInfo();

            try
            {
                var strEmails = "";
                var strNames = "";
                var bdInfo = Common.BalanceDues.GetBalanceDue(balanceId, "", Common.GetActiveClientId());

                if (bdInfo != null && bdInfo.BalanceId > 0)
                {
                    var client = Common.ArgClients.GetArgClient(bdInfo.CompanyId, "") ?? new Arg.DataModels.ArgClient();
                    var custContactInfo = Common.CustomerContacts.GetCustomerContactsForEmails(customerId, bdInfo.CustomerLocationCode, bdInfo.Region, bdInfo.CompanyId);

                    if (custContactInfo != null && custContactInfo.Any())
                    {
                        foreach (var item in custContactInfo)
                        {
                            var email = item.Email.Trim();
                            strEmails = email + ";" + strEmails;
                            strNames = item.FirstName + ", " + strNames.Trim();// (!string.IsNullOrWhiteSpace(item.LastName) ? " " + item.LastName : "") +
                        }

                        if (strEmails != null && strEmails.LastIndexOf(";") > 0)
                        {
                            strEmails = strEmails.Remove(strEmails.LastIndexOf(";"));
                        }
                        strNames = strNames.Remove(strNames.LastIndexOf(","));

                        var smtpAccountInfo = Common.ClientSMTPAccounts.GetSmtpAccount(smtpAccountId) ?? new Arg.DataModels.ClientSMTPAccounts();
                        var templateInfo = Common.Templates.GetTemplate(templateId, "");

                        var info = new
                        {
                            ContactName = strNames.Trim(),
                            InvoiceNo = invoiceNo,
                            SMTPClientName = smtpAccountInfo.FromName,
                            SMTPClientEmail = "<a href='mailto:" + smtpAccountInfo.FromEmail + "'>" + smtpAccountInfo.FromEmail + "</a>"
                        };

                        var subject = Smart.Format(templateInfo.EmailSubject, info);
                        var body = Smart.Format(templateInfo.Content, info);
                        Common.GmailUtilities.GenerateDraft(smtpAccountInfo.SMTPClient, smtpAccountInfo.Port, smtpAccountInfo.UserName, smtpAccountInfo.Password, smtpAccountInfo.FromEmail, subject, body, attachFileServerPath, strEmails);
                        contactInfo.Type = ContactResultTypeEnum.Found;
                        contactInfo.Emails = strEmails;
                    }
                    else
                    {
                        contactInfo.Type = ContactResultTypeEnum.Missing;
                    }

                    var custInfo = Common.Customers.GetCustomer(0, Common.GetActiveClientId(), customerId);
                    if (custInfo != null && custInfo.BdCustomerId > 0)
                    {
                        contactInfo.CustomerName = custInfo.CustomerName;
                    }

                    contactInfo.CustomerLocationCode = bdInfo.CustomerLocationCode;
                    contactInfo.CompanyName = client.Name;
                    contactInfo.CustomerId = customerId;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                Common.Log.Error(ex);
                contactInfo.Message = ex.ToString();
            }
            return contactInfo;
        }

        public IActionResult GenerateInvoice(List<BalanceDue> bdInfo, string customerId, string invoiceFullPath, bool createPdf = true)
        {
            var invoicePDF = new GenerateInvoicePDF();

            try
            {
                invoicePDF.BDItems = new List<BalanceDues_Item>();
                invoicePDF.BDOtherCharges = new List<BalanceDues_OtherCharges>();

                if (bdInfo != null && bdInfo.Any())
                {
                    invoicePDF.BalanceDueInfo = bdInfo;
                    var clientDetails = Common.ArgClients.GetArgClient(invoicePDF.BalanceDueInfo.First().CompanyId, "");
                    if (clientDetails != null && clientDetails.CompanyId > 0)
                    {
                        invoicePDF.ClientDetails = clientDetails;
                    }
                }

                var customerDetails = Common.Customers.GetCustomer(0, Common.GetActiveClientId(), customerId);
                if (customerDetails != null && !string.IsNullOrWhiteSpace(customerDetails.CustomerId))
                {
                    invoicePDF.CustomerDetails = customerDetails;
                }

                var custContactInfo = Common.CustomerContacts.GetContact(0, customerId, Common.GetActiveClientId());
                if (custContactInfo != null && custContactInfo.ContactId > 0)
                {
                    invoicePDF.CustomerContactDetails = custContactInfo;
                }

                if (invoicePDF.BalanceDueInfo != null && invoicePDF.BalanceDueInfo.Any())
                {
                    foreach (var b in invoicePDF.BalanceDueInfo)
                    {
                        invoicePDF.BolNo = b.Bol;
                        invoicePDF.DueDate = Convert.ToDateTime(b.BalanceDueInvoiceDate).AddDays(invoicePDF.ClientDetails.InvoiceTerms);//Convert.ToDateTime
                        var bdItems = Common.BOLHeader.GetBalanceDueItems(invoicePDF.BolNo, b.CompanyId);
                        if (bdItems != null && bdItems.Any())
                        {
                            invoicePDF.BDItems.AddRange(bdItems);
                        }
                        var bdOtherCharges = Common.BalanceDuesOtherCharges.GetBalanceDuesOtherChargesWithDesc(invoicePDF.BolNo, b.CompanyId);
                        if (bdOtherCharges != null && bdOtherCharges.Any())
                        {
                            invoicePDF.BDOtherCharges.AddRange(bdOtherCharges);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                Common.Log.Error(ex);
            }

            if (createPdf)
            {
                var serverPath = Path.Combine(_hostingEnvironment.WebRootPath, invoiceFullPath);
                CreatePdfFile(serverPath, "InvoicePDF", invoicePDF);
            }

            return View("InvoicePDF", invoicePDF);
        }

        [CheckActiveClient]
        [IpAuth]
        [HttpPost]
        public IActionResult Index(BalanceDues balanceDueModel)
        {
            try
            {
                var data = balanceDueModel;
                LoadDataInModel(balanceDueModel);
                balanceDueModel.CompanyId = Arg.DataAccess.ActiveClient.Info.CompanyId;

                if (data.CompanyId > 0)
                {
                    data.SearchOptions.CompanyId = data.CompanyId;
                }
                List<BalanceDue> balDues;
                if (balanceDueModel.SearchOptions != null)
                {
                    balDues = Common.BalanceDues.GetBalanceDues(balanceDueModel.SearchOptions);
                }
                else
                {
                    balDues = Common.BalanceDues.GetBalanceDues(0, "", "");
                }

                if (balDues != null && balDues.Any())
                {
                    //bdByStatus.BalanceDuesByStatusList = balDues.Where(x => x.InvoiceStatus == "Pending" || x.InvoiceStatus == "Invoiced_NR").ToList();
                    if (Common.CurrentUserInfo.IsARGAnalyst)
                    {
                        balDues = balDues.Where(x => x.RevenueAnalystAuditor == Common.CurrentUserId || x.RevenueAnalystCollector == Common.CurrentUserId).ToList();
                    }
                    data.BalanceDuesList = balDues;
                    //data.BalanceDuesByStatus = bdByStatus.BalanceDuesByStatusList;
                }
                else
                {
                    data.Message = "No results found related to your search!";
                }

                if (data.BalanceDuesList != null && data.BalanceDuesList.Any())
                {
                    var custCount = data.BalanceDuesList.Select(x => x.CustomerId).Distinct().Count();
                    data.CustomerCount = custCount;
                    var bdCount = data.BalanceDuesList.Count();
                    data.TotalBDCount = bdCount;
                    var totalBOL = data.BalanceDuesList.Select(x => x.Bol).Distinct().Count();
                    data.BOLCount = totalBOL;
                    decimal totalBD = data.BalanceDuesList.Select(x => x.BalanceDueAmount).Sum();
                    data.TotalBD = String.Format("{0:n}", totalBD);
                    decimal totalBDPaidAmnt = data.BalanceDuesList.Select(x => x.AmountPaid).Sum();
                    data.TotalBDPaidAmount = String.Format("{0:n}", totalBDPaidAmnt);
                }
                return View(data);

            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                Common.Log.Error(ex);
            }
            return View();
        }

        public JsonResult GetBDByStatus(string bdIds)
        {
            var bDByStatus = new BalanceDuesByStatus();

            try
            {
                if (bdIds.Substring(bdIds.Length - 1, 1) == ",")
                {
                    bdIds = bdIds.Remove(bdIds.Length - 1, 1);
                }
                List<int> bdIdsList = bdIds.Split(',').Select(int.Parse).ToList();
                bDByStatus.BalanceDuesByStatusList = new List<BalanceDue>();

                if (bdIdsList != null && bdIdsList.Any())
                {
                    foreach (var bdId in bdIdsList)
                    {
                        var bdInfo = Common.BalanceDues.GetBalanceDue(bdId, "", Common.GetActiveClientId());
                        if (bdInfo.InvoiceStatus == "Pending" || bdInfo.InvoiceStatus == "Invoiced_NR")
                            bDByStatus.BalanceDuesByStatusList.Add(bdInfo);
                    }
                }

                if (bDByStatus.BalanceDuesByStatusList != null && bDByStatus.BalanceDuesByStatusList.Any())
                {
                    bDByStatus.CustomerCount = bDByStatus.BalanceDuesByStatusList.Select(x => x.CustomerId).Distinct().Count();
                    bDByStatus.BDCount = bDByStatus.BalanceDuesByStatusList.Count();
                    decimal totalBD = bDByStatus.BalanceDuesByStatusList.Select(x => x.BalanceDueAmount).Sum();
                    bDByStatus.BDAmount = Convert.ToDecimal(String.Format("{0:n}", totalBD));
                    string bdIdsByStatus = string.Join(",", bDByStatus.BalanceDuesByStatusList.Select(x => x.BalanceId));
                    bDByStatus.BdIdsByStatus = bdIdsByStatus;
                }
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
            return Json(bDByStatus);
        }

        public string GenerateAuditReviews(SearchOptionsForExport searchOptionsForExport)
        {
            try
            {
                if (Arg.DataAccess.ActiveClient.Info.CompanyId <= 0)
                {
                    return "Client not active. Please <a href='" + Common.MyRoot + "Account/Login' title='Login'>re-login.</a>";
                }
                searchOptionsForExport.CompanyId = Arg.DataAccess.ActiveClient.Info.CompanyId;
                var fileName = Common.ActiveClient.GetName() + "_" + searchOptionsForExport.CompanyId + "_" + (searchOptionsForExport.Region == "%" ? "All" : searchOptionsForExport.Region.Replace(',', '-')) + "_Region_" + "_Audit_Review_" + DateTime.Now.Date.ToString("MMddyyy") + ".xlsx";
                var relativePath = "BalanceDues";
                var fullPath = Path.Combine(_hostingEnvironment.WebRootPath, relativePath, fileName);
                var reportPath = "/ARG/Audit_Review";

                List<ReportParameter> reportParameters = new List<ReportParameter>
            {
                    new ReportParameter("Client", searchOptionsForExport.CompanyId.ToString()),
                    new ReportParameter("CustomerID", searchOptionsForExport.CustomerId),
                    new ReportParameter("BeginInvoiceDate", searchOptionsForExport.BalanceDueInvoiceStartDate.ToDefaultStartDate()),
                    new ReportParameter("EndInvoiceDate", searchOptionsForExport.BalanceDueInvoiceEndDate.ToDefaultEndDate()),
                    new ReportParameter("BeginAuditDate", searchOptionsForExport.DateAddedStart.ToDefaultStartDate()),
                    new ReportParameter("EndAuditDate", searchOptionsForExport.DateAddedEnd.ToDefaultEndDate()),
                    new ReportParameter("CollectionStatus", string.Join(",", searchOptionsForExport.CollectionStatus)),
                    new ReportParameter("BDAmount", searchOptionsForExport.BalanceDueAmount.ToString()),
                    new ReportParameter("BDAmountSign", searchOptionsForExport.BalanceDueAmountSign.ToString()),
                    new ReportParameter("CloseReasonCode", searchOptionsForExport.CloseReasonCode),
                    new ReportParameter("Region", searchOptionsForExport.Region),
                    new ReportParameter("BeginBOLDate", searchOptionsForExport.BolExecutionStartDate.ToDefaultStartDate()),
                    new ReportParameter("EndBOLDate", searchOptionsForExport.BolExecutionEndDate.ToDefaultEndDate()),
                    new ReportParameter("InvoiceStatus", string.Join(",", searchOptionsForExport.InvoiceStatus)),
                    new ReportParameter("POL", string.Join(",", searchOptionsForExport.PortOfLoading)),
                    new ReportParameter("POD", string.Join(",", searchOptionsForExport.PortOfDischarge)),
                    new ReportParameter("BDReasonCode", string.Join(",", searchOptionsForExport.BDErrorCodes)),
                    new ReportParameter("InvoiceType", string.Join(",", searchOptionsForExport.InvoiceType)),
            };

                Common.GenerateReport(reportPath, reportParameters, "EXCELOPENXML", fullPath);

                return fullPath.Replace(Common.MyAppRoot, Common.MyRoot);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                Common.Log.Error(ex);
            }
            return null;
        }

        public JsonResult UpdateInvoiceStatus(string bdIds, string status)
        {
            try
            {
                if (bdIds.Substring(bdIds.Length - 1, 1) == ",")
                {
                    bdIds = bdIds.Remove(bdIds.Length - 1, 1);
                }

                var balDues = Common.BalanceDues.GetBalanceDues(bdIds);
                var customers = balDues.Select(x => x.CustomerId).Distinct();
                var result = new InvoiceStatusResult();
                result.Info = new List<InvoiceStatusResultInfo>();

                if (balDues != null && balDues.Any())
                {
                    var oldStatus = "";
                    var missingInvMessage = Common.MissingInvoiceMessage;

                    foreach (var cId in customers)
                    {
                        var useExisting = false;

                        foreach (var bdInfo in balDues.Where(x => x.CustomerId == cId))
                        {
                            oldStatus = bdInfo.InvoiceStatus;
                            var output = UpdateBDInvoiceStatus(bdInfo.BalanceId, oldStatus, status, "", useExisting);
                            useExisting = true;
                            result.ChangesDone = "";
                            if (!string.IsNullOrWhiteSpace(((AjaxResult)(output.Value)).ChangesDone))
                            {
                                result.ChangesDone += "For each BD::<br />";
                                result.ChangesDone += ((AjaxResult)(output.Value)).ChangesDone + "<br/>";
                            }
                            missingInvMessage += ((AjaxResult)(output.Value)).MissingInvoiceBOL + ", ";
                            result.UpdatedInvoiceCount++;
                        }
                    }

                    if (missingInvMessage.LastIndexOf(", ") > 0)
                    {
                        missingInvMessage = missingInvMessage.Remove(missingInvMessage.LastIndexOf(", "));
                    }
                    result.MissingInvBOLsMsg = missingInvMessage;
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                Common.Log.Error(ex);
            }
            return Json("Error");
        }

        public JsonResult UpdateInvoiceStatusToInvoiced_REC(string bdIdsByStatus, string status)
        {
            try
            {
                var statusResult = new InvoiceStatusResult();
                var balDues = Common.BalanceDues.GetBalanceDues(bdIdsByStatus);
                var customers = balDues.Select(x => x.CustomerId).Distinct();
                statusResult.Info = new List<InvoiceStatusResultInfo>();

                if (balDues != null && balDues.Any())
                {
                    var oldStatus = "";
                    var missingInvMessage = Common.MissingInvoiceMessage;

                    foreach (var cId in customers)
                    {
                        var useExisting = false;

                        foreach (var bdInfo in balDues.Where(x => x.CustomerId == cId))
                        {
                            oldStatus = bdInfo.InvoiceStatus;
                            var output = UpdateBDInvoiceStatus(bdInfo.BalanceId, oldStatus, status, "", useExisting);
                            useExisting = true;
                            statusResult.ChangesDone = ((AjaxResult)(output.Value)).ChangesDone;
                            missingInvMessage += ((AjaxResult)(output.Value)).MissingInvoiceBOL + ",";
                            statusResult.UpdatedInvoiceCount++;
                        }
                    }

                    if (missingInvMessage.LastIndexOf(", ") > 0)
                    {
                        missingInvMessage = missingInvMessage.Remove(missingInvMessage.LastIndexOf(", "));
                    }
                    statusResult.MissingInvBOLsMsg = missingInvMessage;
                }
                return Json(statusResult);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                Common.Log.Error(ex);
            }
            return Json("Error");
        }

        public JsonResult UpdatePendingStatus(string bdIds)
        {
            if (Arg.DataAccess.ActiveClient.Info.CompanyId <= 0)
            {
                return Json("Client not active. Please <a href='" + Common.MyRoot + "Account/Login' title='Login'>re-login.</a>");
            }

            try
            {
                var statusResult = new InvoiceStatusResult();
                statusResult.Info = new List<InvoiceStatusResultInfo>();

                if (bdIds.Substring(bdIds.Length - 1, 1) == ",")
                {
                    bdIds = bdIds.Remove(bdIds.Length - 1, 1);
                }

                var balDues = Common.BalanceDues.GetBalanceDues(bdIds);
                if (balDues != null && balDues.Any())
                {
                    balDues = balDues.Where(x => x.InvoiceStatus == "Pending").ToList();
                    if (balDues.Count > 0)
                    {
                        foreach (var bd in balDues)
                        {
                            bd.LastModifiedBy = User.Identity.GetUserId();
                            bd.LastModified = DateTime.Now;
                            bd.InvoiceStatus = "Pending_Approval";
                            UpdateBalanceDue(bd);
                            AddCollectionComment(bd, "InvoiceStatus set from 'Pending' to 'Pending_Approval'");
                        }
                    }
                }
                return Json(statusResult);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                Common.Log.Error(ex);
            }
            return Json("Error");
        }

        [HttpGet]
        public IActionResult Save(int? balanceId)
        {
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                var balanceDues = new BalanceDues();
                balanceDues.CommonObjects.TopHeading = "Balance Dues";
                var customers = Common.Customers.GetCustomers();
                balanceDues.Customers = new SelectList(customers, "CustomerId", "ContactName");

                var companies = Common.ArgClients.GetArgClients(Common.CurrentUserId);
                balanceDues.Companies = new SelectList(companies, "CompanyId", "Name");

                balanceDues.BalanceDuesDetail = new BalanceDue();
                var _balId = Convert.ToInt32(balanceId);

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
                    RedirectToAction("Index","BalanceDues");
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                Common.Log.Error(ex);
            }
            return RedirectToAction("Index", "BalanceDues");
        }

        [HttpGet]
        public PartialViewResult BDPaymentsDetail(int balanceId)
        {
            var paymentAmountDetails = new BDPaymentAmountDetails();

            try
            {
                if (balanceId > 0)
                {
                    var bdInfo = Common.BalanceDues.GetBalanceDue(balanceId, "", Common.GetActiveClientId());
                    var payments = Common.BalanceDuesPayments.GetBalanceDuesPayments(bdInfo.Bol, bdInfo.CompanyId, bdInfo.BalanceDueInvoice, bdInfo.BalanceDueInvoiceDate);
                    paymentAmountDetails.BalanceDueInfo = bdInfo;
                    paymentAmountDetails.Payor = bdInfo.CustomerId;
                    paymentAmountDetails.Currency = bdInfo.Currency;
                    paymentAmountDetails.PaymentDate = DateTime.Now;

                    if (payments != null && payments.Any())
                    {
                        paymentAmountDetails.BDPayments = payments;
                    }

                    var invoiceStatuses = Common.BalanceDues.GetDistinctInvoiceStatus(bdInfo.CompanyId);//todo confirm companyid
                    if (invoiceStatuses != null)
                    {
                        paymentAmountDetails.InvoiceStatuses = new SelectList(invoiceStatuses, "InvoiceStatus", "InvoiceStatus");
                    }

                    var customers = Common.BalanceDuesPayments.GetBDPaymentCustomers();
                    paymentAmountDetails.Customers = new SelectList(customers, "CustomerId", "CustomerName");
                    return PartialView(paymentAmountDetails);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                Common.Log.Error(ex);
            }
            return null;
        }

        private BalanceDue AddBalanceDuePayment(BalanceDue bdInfo)
        {
            try
            {
                var innvSummary = Common.InvoiceSummary.GetInvoiceNo(bdInfo.Bol);
                var paymentInfo = new BalanceDues_Payments
                {
                    CompanyID = bdInfo.CompanyId,
                    Region = bdInfo.Region,
                    CustomerID = bdInfo.CustomerId,
                    CustomerLocationCode = bdInfo.CustomerLocationCode,
                    BookingID = bdInfo.BookingId,
                    BOLNo = bdInfo.Bol,
                    Currency = bdInfo.Currency,
                    BOLExecutionDate = bdInfo.BolExecutionDate,
                    BalanceDueInvoiceNo = bdInfo.BalanceDueInvoice,
                    BalanceDueInvoiceDate = Convert.ToDateTime(bdInfo.BalanceDueInvoiceDate),
                    Payor = bdInfo.CustomerId,
                    PaymentAmount = innvSummary.PrepaidAmount,
                    PaymentDate = bdInfo.DateAdded,
                    PaymentType = string.Empty,
                    PaymentReference = string.Empty
                };

                Common.BalanceDuesPayments.SaveBalanceDuesPayment(paymentInfo);

                if (bdInfo.BalanceId > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Updated, bdInfo.CompanyId, "BalanceDues Payments");
                    return bdInfo;
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return null;
        }

        public class AjaxResult
        {
            public string Message { get; set; }
            public bool ShowCloseReasonCode { get; set; }
            public SelectList SelectedCloseReasonCodes { get; set; }
            public BalanceDues_Payments BDPayments { get; set; }
            public string BDCloseReasonCode { get; set; }
            public string ChangesDone { get; set; }
            public string MissingInvoiceBOL { get; set; }
        }

        public class InvoiceOutput
        {
            public string CustomerId { get; set; }
            public string CompanyName { get; set; }
            public string BalanceDueInvoice { get; set; }
            public string PdfLink { get; set; }
            public string InvoiceNo { get; set; }
            public string FileServerPath { get; set; }
            public BalanceDue BalanceDue { get; set; }
        }

        public enum ContactResultTypeEnum
        {
            Missing, 
            Found
        }

        public class CustomerContactInfo
        {
            public string Message { get; set; }
            public string CustomerId { get; set; }
            public string CustomerName { get; set; }
            public string CompanyName { get; set; }
            public string CustomerLocationCode { get; set; }
            public string Emails { get; set; }
            public ContactResultTypeEnum Type { get; set; }
        }

        public class EmailStatsInfo
        {
            public List<CustomerContactInfo> Contacts { get; set; }

            public int FailedCount
            {
                get
                {
                    return Contacts.Count(x => x.Type == ContactResultTypeEnum.Missing);
                }
            }

            public int SuccessCount
            {
                get
                {
                    return Contacts.Count(x => x.Type == ContactResultTypeEnum.Found);
                }
            }
        }

        public class ResultStats
        {
            public List<InvoiceOutput> InvoiceOutput { get; set; }
            public int InvoiceCount { get; set; }
            public int DraftCount { get; set; }
            public string Message { get; set; }
            public EmailStatsInfo EmailStats { get; set; }

            public string EmailResultHtml
            {
                get
                {
                    if (EmailStats.Contacts != null && EmailStats.Contacts.Any() && string.IsNullOrWhiteSpace(EmailStats.Contacts.First().Message))
                    {
                        var html = "<div class='resultStats' style='margin-top:50px;'>";
                        html += "PDF's generated: <span class='count' style='font-size: 20px;'>" + InvoiceCount + "</span> | Drafts generated: <span class='count' style='font-size: 20px;'>" + EmailStats.SuccessCount + "</span>";
                        html += "<table class='table table-bordered table-striped' style='width:90%;margin-top: 15px;'>";
                        html += "<thead><tr><th style='width:20%'>Customer ID</th><th style='width:30%;'>Customer</th><th style='width:20%;'>Location Code</th><th style='width:30%;'>Emails</th><th style='width:30%;'></th></thead><tbody>";
                        foreach (var item in EmailStats.Contacts)
                        {
                            html += "<tr><td>" + item.CustomerId + "</td><td>" + item.CustomerName + "</td><td>" + item.CustomerLocationCode + "</td><td>" + item.Emails + "</td><td>" + item.Type + "</td></tr>";
                        }
                        html += "</div>";
                        return html;
                    }
                    return null;
                }
            }
        }

        public class BalanceDuesByStatus
        {
            public List<BalanceDue> BalanceDuesByStatusList { get; set; }
            public int CustomerCount { get; set; }
            public decimal BDAmount { get; set; }
            public decimal BDPaidAmount { get; set; }
            public int BDCount { get; set; }
            public string BdIdsByStatus { get; set; }
        }

        private class InvoiceStatusResultInfo
        {
            public string CustomerId { get; set; }
            public string Status { get; set; }
        }

        private class InvoiceStatusResult
        {
            public List<InvoiceStatusResultInfo> Info { get; set; }
            public int UpdatedInvoiceCount { get; set; }
            public string ChangesDone { get; set; }
            public string MissingInvBOLsMsg { get; set; }
        }
    }
}
