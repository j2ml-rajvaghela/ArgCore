using ArgCore.Helpers;
using ArgCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;

namespace ArgCore.Controllers
{
    public class CustomerContactsController : Controller
    {
        public class AjaxResult
        {
            public string Message { get; set; }
        }

        [HttpGet]
        public IActionResult Save(int? contactId, string customerId)
        {
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                var customerContacts = new CustomerContacts();

                customerContacts.CommonObjects.TopHeading = "Customer Contacts";
                customerContacts.CustomerContactDetail = new Arg.DataModels.BalanceDues_Customers_Contacts();

                var locationCodes = Common.CustomerContacts.GetDistinctLocationCodes(customerId, Common.GetActiveClientId());
                customerContacts.LocationCodes = new SelectList(locationCodes, "CustomerLocationCode", "CustomerLocationCode");

                var _contactId = Convert.ToInt32(contactId);
                if (_contactId > 0)
                {
                    customerContacts.CommonObjects.Heading = "Edit Customer Contact";
                    customerContacts.CustomerContactDetail = Common.CustomerContacts.GetContact(_contactId, "", Common.GetActiveClientId());
                    if (customerContacts.CustomerContactDetail == null || customerContacts.CustomerContactDetail.ContactId <= 0)
                    {
                        return RedirectToAction("Index", "Customers", new { m = "Customer Contact not found or deleted" });
                    }
                }
                else
                {
                    customerContacts.CommonObjects.Heading = "Add Customer Contact";
                }
                var customerInfo = Common.Customers.GetCustomer(0, Common.GetActiveClientId(), customerId);
                if (customerInfo != null && customerInfo.BdCustomerId > 0)
                {
                    customerContacts.CustomerContactDetail.Region = customerInfo.Region;
                    customerContacts.CustomerContactDetail.CompanyId = customerInfo.CompanyId;
                    var company = Common.ArgClients.GetArgClient(customerInfo.CompanyId, "");
                    if (company != null && company.CompanyId > 0)
                    {
                        customerContacts.CompanyName = company.Name;
                    }

                }
                customerContacts.CustomerContactDetail.CustomerId = customerId;

                return View(customerContacts);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return null;
        }

        public bool IsValidEmail(string email)
        {
            try
            {
                bool isEmail = Regex.IsMatch(email.Trim(), @"\A(?:[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\.)+[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?)\Z");
                if (isEmail)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }

            return false;
        }

        [HttpPost]
        public JsonResult Save(CustomerContacts customerContacts)
        {
            var ajaxResult = new AjaxResult();
            try
            {
                if (string.IsNullOrWhiteSpace(customerContacts.CustomerContactDetail.FirstName))
                {
                    ajaxResult.Message = "Please enter FirstName.<br />";
                }

                if (string.IsNullOrWhiteSpace(customerContacts.CustomerContactDetail.CustomerLocationCode))
                {
                    ajaxResult.Message += "Please enter CustomerLocationCode.<br />";
                }

                if (!string.IsNullOrWhiteSpace(customerContacts.CustomerContactDetail.PhoneNo))
                {
                    if (customerContacts.CustomerContactDetail.PhoneNo.Length < 10 || customerContacts.CustomerContactDetail.PhoneNo.Length > 10)
                    {
                        ajaxResult.Message = "Please enter a 10 digit mobile number.";
                    }
                }

                if (!string.IsNullOrWhiteSpace(customerContacts.CustomerContactDetail.Email))
                {
                    if (!IsValidEmail(customerContacts.CustomerContactDetail.Email))
                    {
                        ajaxResult.Message += "Please enter Valid Email.<br />";
                    }
                }
                else
                {
                    ajaxResult.Message += "Please enter Email.<br />";
                }

                if (!string.IsNullOrWhiteSpace(ajaxResult.Message))
                {
                    return Json(ajaxResult);
                }
                else
                {
                    Common.CustomerContacts.SaveCustomerContact(customerContacts.CustomerContactDetail);
                    if (customerContacts.CustomerContactDetail.ContactId > 0)
                    {
                        Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Saved, customerContacts.CustomerContactDetail.CompanyId, "Customer Contacts");
                        ajaxResult.Message = "Added";
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                ajaxResult.Message = ex.ToString();
            }
            return Json(ajaxResult);
            //return Redirect(Common.MyRoot + "Customers/Index");
        }

        [HttpPost]
        public JsonResult Delete(int contactId)
        {
            try
            {
                var result = Common.CustomerContacts.DeleteCustomerContact(contactId);
                if (result > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Deleted, Common.GetActiveClientId(), "CustomerContacts");
                    return Json("Deleted");
                    //return RedirectToAction("Index", "Customers");
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                return Json("Error!");
            }
            return null;
        }
    }
}
