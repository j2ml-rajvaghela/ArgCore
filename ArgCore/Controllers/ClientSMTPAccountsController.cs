using Arg.DataModels;
using ArgCore.Attributes;
using ArgCore.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArgCore.Controllers
{
    [Authorize]
    [AuthorizeUser]
    public class ClientSMTPAccountsController : Controller
    {
        public IActionResult Index()
        {
            var clientSMTPAccounts = new Models.ClientSMTPAccounts();
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                clientSMTPAccounts.CommonObjects.TopHeading = "SMTP Settings";
                clientSMTPAccounts.CommonObjects.Heading = "Client SMTP Settings";

                var companies = Common.ArgClients.GetArgClients(Common.CurrentUserId);
                clientSMTPAccounts.Companies = new SelectList(companies, "CompanyId", "Name");

                var smtpAccounts = Common.ClientSMTPAccounts.GetSmtpAccounts(new SearchOptions());
                if (smtpAccounts != null && smtpAccounts.Any())
                {
                    clientSMTPAccounts.SmtpAccountsList = smtpAccounts;
                }

            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return View(clientSMTPAccounts);
        }

        [HttpPost]
        public IActionResult Index(Models.ClientSMTPAccounts clientSMTPAccounts)
        {
            try
            {
                clientSMTPAccounts.CommonObjects.TopHeading = "SMTP Settings";
                var companies = Common.ArgClients.GetArgClients(Common.CurrentUserId);
                clientSMTPAccounts.Companies = new SelectList(companies, "CompanyId", "Name");
                List<ClientSMTPAccounts> smtpAccounts;
                if (clientSMTPAccounts.SearchOptions != null)
                {
                    smtpAccounts = Common.ClientSMTPAccounts.GetSmtpAccounts(clientSMTPAccounts.SearchOptions);
                }
                else
                {
                    smtpAccounts = Common.ClientSMTPAccounts.GetSmtpAccounts(new SearchOptions());
                }   
                if (smtpAccounts != null && smtpAccounts.Any())
                {
                    clientSMTPAccounts.SmtpAccountsList = smtpAccounts;
                }
                    
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return View(clientSMTPAccounts);
        }

        [HttpGet]
        public IActionResult Save(int? SMTPAccountId)
        {
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                var clientSMTPAccounts = new Models.ClientSMTPAccounts();
                clientSMTPAccounts.CommonObjects.TopHeading = "SMTP Settings";

                var companies = Common.ArgClients.GetArgClients(Common.CurrentUserId);
                clientSMTPAccounts.Companies = new SelectList(companies, "CompanyId", "Name");
                clientSMTPAccounts.ClientSMTPAccountDetail = new ClientSMTPAccounts();

                var _smtpAccId = Convert.ToInt32(SMTPAccountId);
                if (_smtpAccId > 0)
                {
                    clientSMTPAccounts.CommonObjects.Heading = "Edit SMTP Account";
                    clientSMTPAccounts.ClientSMTPAccountDetail = Common.ClientSMTPAccounts.GetSmtpAccount(_smtpAccId);
                    if (clientSMTPAccounts.ClientSMTPAccountDetail == null || clientSMTPAccounts.ClientSMTPAccountDetail.SMTPAccountId <= 0)
                    {
                        return RedirectToAction("ClientSMTPAccounts", new { m = "Items not found or deleted" });
                    }

                }
                else
                {
                    clientSMTPAccounts.CommonObjects.Heading = "Add SMTP Account";
                }
                return View(clientSMTPAccounts);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return null;
        }

        [HttpPost]
        public IActionResult Save(Models.ClientSMTPAccounts clientSMTPAccounts)
        {
            try
            {
                if (clientSMTPAccounts.ClientSMTPAccountDetail.SMTPAccountId > 0)
                {
                    clientSMTPAccounts.ClientSMTPAccountDetail.Password = Common.ClientSMTPAccounts.GetSmtpAccount(clientSMTPAccounts.ClientSMTPAccountDetail.SMTPAccountId).Password;
                }

                Common.ClientSMTPAccounts.SaveClientSMTPAccount(clientSMTPAccounts.ClientSMTPAccountDetail);

                if (clientSMTPAccounts.ClientSMTPAccountDetail.SMTPAccountId > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Saved, clientSMTPAccounts.ClientSMTPAccountDetail.CompanyId, "Client SMTP Accounts");
                    RedirectToAction("Index", "ClientSMTPAccounts");
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return RedirectToAction("Index", "ClientSMTPAccounts");
        }

        public IActionResult Delete(int SMTPAccountId)
        {
            try
            {
                var result = Common.ClientSMTPAccounts.DeleteClientSMTPAccount(SMTPAccountId);
                if (result > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Deleted, Common.GetActiveClientId(), "Client SMTP Accounts");
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
