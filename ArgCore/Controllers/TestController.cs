
using ArgCore.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ArgCore.Controllers
{
    public class TestController : Controller
    {
        private readonly HttpContext _context;
        public TestController(IHttpContextAccessor context)
        {
            _context = context.HttpContext;
        }

        public IActionResult Index()
        {
			try
			{
                ViewBag.IP = GetIPAddress();
                ViewBag.UsersClientIpAddress = new WebClient().DownloadString("http://checkip.dyndns.org/%22");
                ViewBag.UsersPublicIpAddress = GetUserIPAddress(_context);
                ViewBag.IpAddress = Arg.DataAccess.Common.GetUserIpAddress();
            }
			catch (Exception ex)
			{
                Common.Log.Error(ex);
            }
            return View();
        }

        public IActionResult ExpireSession()
        {
            _context.Session.Clear();
            _context.Session.Remove("UserId");
            Common.CheckActiveClient();
            return null;
        }

        protected string GetUserIP()
        {
            string VisitorsIPAddr = string.Empty;

            if (_context.Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                VisitorsIPAddr = _context.Request.Headers["X-Forwarded-For"].ToString();
            }
            else if (_context.Connection.RemoteIpAddress != null)
            {
                VisitorsIPAddr = _context.Connection.RemoteIpAddress.ToString();
            }
            return VisitorsIPAddr;
        }

        //Public ipAddress of user
        public static string GetUserIPAddress(HttpContext _context)
        {
            string ip = string.Empty;

            if (_context.Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                ip = _context.Request.Headers["X-Forwarded-For"].ToString();
            }
            else if (_context.Connection.RemoteIpAddress != null)
            {
                ip = _context.Connection.RemoteIpAddress.ToString();
            }

            if (ip == "::1")
            {
                ip = "127.0.0.1";
            }
            return ip;
        }

        //User's client ip address
        protected string GetIPAddress()
        {
            string ipAddress = _context.Request.Headers["X-Forwarded-For"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0].Trim();
                }
            }
            return _context.Connection.RemoteIpAddress?.ToString().Trim();
        }
    }
}
