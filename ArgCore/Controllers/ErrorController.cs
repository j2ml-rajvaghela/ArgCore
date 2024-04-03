using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ArgCore.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            Response.StatusCode = 500;
            Exception ex = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
            return View("Error", ex);
        }

        public IActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }
    }
}
