using ArgCore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ArgCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Admin");
        }
    }
}
