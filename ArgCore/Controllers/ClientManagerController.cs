using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArgCore.Controllers
{
    public class ClientManagerController : Controller
    {

        [Authorize(Roles = "ClientManager")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
