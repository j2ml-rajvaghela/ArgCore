using ArgCore.Helpers;
using ArgCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace ArgCore.Controllers
{
    public class InvoicesController : Controller
    {
        public IActionResult Index(string q)
        {
            var model = new Invoices();
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                model.CommonObjects.TopHeading = "Invoices";

                var invoices = Common.ArgInvoices.GetArgInvoices(Common.CurrentUserId, (!string.IsNullOrWhiteSpace(q) ? q : ""));
                if (invoices != null && invoices.Any())
                {
                    model.InvoicesList = invoices;
                }

            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return View(model);
        }

        public class DataByCompany
        {
            public SelectList Regions { get; set; }
            public SelectList InvoiceTypes { get; set; }
            public string Invoice { get; set; }
            public string Message { get; set; }
        }

        public JsonResult LoadDataByCompany(int companyId)
        {
            var model = new DataByCompany();
            try
            {
                var regions = Common.Regions.GetRegions(0, "", companyId);
                model.Regions = new SelectList(regions, "Region", "Region");
                model.Invoice = Arg.Core.Utility.GenerateRandomInvoiceNo("10");

                var invoiceTypes = Common.BalanceDues.GetDistinctInvoiceTypesMultiple(Convert.ToString(companyId));
                model.InvoiceTypes = new SelectList(invoiceTypes, "InvoiceType", "InvoiceType");
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                model.Message = ex.ToString();
            }
            return Json(model);
        }

        [HttpGet]
        public IActionResult Save(int? invoiceId, int? companyId)
        {
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                var invoices = new Invoices();
                invoices.InvoiceStatus = Common.StatusOptions.Select((r, index) => new SelectListItem { Text = r, Value = r });
                invoices.CommonObjects.TopHeading = "Invoices";

                var _companyId = Convert.ToInt32(companyId);

                var invoiceTypes = Common.BalanceDues.GetDistinctInvoiceTypesMultiple("");
                invoices.InvoiceTypes = new SelectList(invoiceTypes, "InvoiceType", "InvoiceType");

                var companies = Common.ArgClients.GetArgClients(Common.CurrentUserId);
                invoices.Companies = new SelectList(companies, "CompanyId", "Name");
                invoices.Clients = new SelectList(companies, "CompanyId", "InvoiceTerms"); 

                var regions = Common.Regions.GetRegions(0, "", _companyId);
                invoices.Regions = new SelectList(regions, "Region", "Region");

                invoices.InvoiceDetail = new Arg.DataModels.ArgInvoice();
                if (companyId > 0)
                {
                    invoices.InvoiceDetail.Invoice = Arg.Core.Utility.GenerateRandomInvoiceNo("10");
                    invoices.InvoiceDetail.CompanyId = _companyId;
                }

                var _invoiceId = Convert.ToInt32(invoiceId);
                if (_invoiceId > 0)
                {
                    invoices.CommonObjects.Heading = "Edit Invoice";
                    invoices.InvoiceDetail = Common.ArgInvoices.GetArgInvoice(_invoiceId, "", _companyId);
                    if (invoices.InvoiceDetail == null || invoices.InvoiceDetail.InvoiceId <= 0)
                        return RedirectToAction("Invoices", new { m = "ARG Invoice not found or deleted" });
                }
                else
                {
                    invoices.CommonObjects.Heading = "Add Invoice";
                }

                //SET DEFAULT VALUES
                if (string.IsNullOrWhiteSpace(invoices.InvoiceDetail.InvoiceStatus))
                {
                    invoices.InvoiceDetail.InvoiceStatus = "Open";
                }

                return View(invoices);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                Common.Log.Error(ex);
            }
            return null;
        }

        public class AjaxResult
        {
            public string Message { get; set; }
        }

        //Used on http://localhost:26744/ArgInvoicesBD/Index?companyId=1005 in Add Invoice Popup

        [HttpPost]
        public JsonResult Save(Invoices invoices)
        {
            var ajaxResult = new AjaxResult();
            try
            {
                if (invoices.InvoiceDetail.InvoiceDate == DateTime.MinValue)
                {
                    invoices.InvoiceDetail.InvoiceDate = DateTime.Now;
                }

                if (invoices.InvoiceDetail.DueDate == DateTime.MinValue)
                {
                    invoices.InvoiceDetail.DueDate = invoices.InvoiceDetail.InvoiceDate.AddDays(20);
                }

                Common.ArgInvoices.SaveArgInvoice(invoices.InvoiceDetail);

                if (invoices.InvoiceDetail.InvoiceId > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Saved, invoices.InvoiceDetail.CompanyId, "Invoices");
                    ajaxResult.Message = "New Invoice added!";
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

        public IActionResult Delete(int invoiceId)
        {
            try
            {
                var result = Common.ArgInvoices.DeleteArgInvoice(invoiceId);
                if (result > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Deleted, Common.GetActiveClientId(), "Invoices");
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
