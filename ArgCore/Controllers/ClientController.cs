using ArgCore.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArgCore.Controllers
{
    [Route("client")]
    [Authorize(Roles = "ARGManager")]
    public class ClientController : _baseMVCController
    {
        public IActionResult Dashboard(int id = 0)
        {
            return RedirectToAction("Index", new { id = id });
        }

        public IActionResult Index(int id = 0)
        {
            int clientId = 0;

            if (id > 0)
            {
                clientId = id;
            }

            if (HttpContext.Session.GetInt32("clientid").HasValue)
            {
                clientId = HttpContext.Session.GetInt32("clientid").Value;
            }

            if (clientId == 0)
            {
                return RedirectToAction("Index", "Manager");
            }

            HttpContext.Session.SetInt32("clientid", clientId);

            return View();
        }

        public IActionResult Zoom()
        {
            return View();
        }

        [HttpGet]
        [Route("managebillofladings/{clientid}/{customerid}")]
        public IActionResult ManageBillOfLadings(int clientid, int customerid)
        {
            ViewBag.clientd = clientid;
            ViewBag.customerid = customerid;
            HttpContext.Session.SetInt32("clientid", clientid);
            HttpContext.Session.SetInt32("customerid", customerid);
            return View();
        }

        [HttpGet]
        [Route("billoflading/{id}")]
        public IActionResult BillofLading(int id)
        {
            try
            {
                Uri myUrl = Request.GetTypedHeaders().Referer;
                int clientId = 0;
                int customerId = 0;

                if (HttpContext.Session.GetInt32("clientid").HasValue)
                {
                    clientId = HttpContext.Session.GetInt32("clientid").Value;
                }

                if (HttpContext.Session.GetInt32("customerid").HasValue)
                {
                    customerId = HttpContext.Session.GetInt32("customerid").Value;
                }

                if (clientId == 0)
                {
                    return RedirectToAction("Index", "Manager");
                }

                int billOfLadingId = id;
                ViewBag.clientid = clientId;
                ViewBag.customerid = customerId;
                ViewBag.billofladingid = billOfLadingId;
                ViewBag.referrer = myUrl.ToString();

                return View();
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                return null;
            }
        }

        [HttpGet]
        [Route("workwithbillofladings/{clientid?}")]
        public IActionResult WorkWithBillOfLadings(int clientid = 0)
        {
            try
            {
                if (clientid == 0 && HttpContext.Session.GetInt32("clientid").HasValue)
                {
                    clientid = HttpContext.Session.GetInt32("clientid").Value;
                }

                if (clientid == 0)
                {
                    return RedirectToAction("Index", "Manager");
                }

                ViewBag.clientid = clientid;
                HttpContext.Session.SetInt32("clientid", clientid);
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
