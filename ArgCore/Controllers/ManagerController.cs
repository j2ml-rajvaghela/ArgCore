using ArgCore.Helpers;
using ArgCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Renci.SshNet;

namespace ArgCore.Controllers
{
    [Route("manager")]
    [Authorize(Roles = "ARGManager")]
    public class ManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ManageClients()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ClientEditV1(int? id)
        {
            try
            {
                var saveClient = new SaveClient();
                var _clientId = Convert.ToInt32(id);

                if (_clientId > 0)
                {
                    saveClient.Client = Common.Clients.GetClient(_clientId);

                    if (saveClient.Client == null || saveClient.Client.clientid <= 0)
                    {
                        return RedirectToAction("ManageClients");
                    }
                }

                var countrList = Common.Countries.GetCountries();
                var countries = new SelectList(countrList, "countryid", "countryname");

                saveClient.Countries = countries;
                return View(saveClient);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return null;
        }

        [HttpPost]
        public IActionResult ClientEditV1(Arg.DataModels.Client client)
        {
            try
            {
                Common.Clients.SaveClient(client);

                if (client.clientid > 0)
                {
                    return RedirectToAction("ManageClients");
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return RedirectToAction("ManageClients");
        }

        [HttpGet]
        [Route("managecustomers/{clientid}")]
        public IActionResult ManageCustomers(int clientId)
        {
            try
            {
                ViewBag.clientid = clientId;
                return View();
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                return null;
            }
        }

        [HttpGet]
        [Route("customersummary/{clientid}")]
        public IActionResult CustomerSummary(int clientId)
        {
            try
            {
                ViewBag.clientid = clientId;
                return View();
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                return null;
            }
        }

        [HttpGet]
        [Route("managebillofladings/{clientid}/{customerid}")]
        public IActionResult ManageBillOfLadings(int clientId, int customerId)
        {
            try
            {
                ViewBag.clientid = clientId;
                ViewBag.customerid = customerId;
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
