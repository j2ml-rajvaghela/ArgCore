using ArgCore.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ArgCore.Controllers
{
    public class WebMarkAPIController : Controller
    {
        [HttpPost]
        [Route("savewebmarketing")]
        public bool SaveWebMarketing()
        {
            try
            {
                // var webMark=new arg.DataAccess.WebsiteMarketingImpl().SaveWebMarketing();
                //if(webMark)
                return false;
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.ToString());
            }
            return false;
        }
    }
}
