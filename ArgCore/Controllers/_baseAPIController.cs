using Arg.DAL;
using Arg.DataModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Web.Http;



namespace ArgCore.Controllers
{
    public class _baseAPIController : ApiController
    {
        public string errorMessage = "";
        public int userid;

        public string CurrentUserID()
        {
            return User.Identity.GetUserId();
        }

        public int ThisUserID()
        {
            int userId = 0;
            AppUser thisUser = new AppUser();
            string currentuserId = CurrentUserID();

            if (currentuserId.Length > 0)
            {
                using (users db = new users())
                {
                    thisUser = db.getuserprofile(currentuserId);
                    userId = thisUser.AppUserId;
                }
            }
            return userId;
        }

        public string getConnectionString(string loc = "")
        {
            string cs = "";
            try
            {
                cs = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
            return cs;
        }
    }
}
