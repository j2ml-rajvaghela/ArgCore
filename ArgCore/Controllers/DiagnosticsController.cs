using ArgCore.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ArgCore.Controllers
{
    public class DiagnosticsController : Controller
    {
        public IActionResult Index()
        {
            try
            {
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return View();
        }

        public JsonResult GetDiagnosticData()
        {
            //return "Pasha Hawaii";
            var activeClientInfo = Arg.DataAccess.ActiveClient.Info;
            return Json(activeClientInfo);
        }
    }
}
