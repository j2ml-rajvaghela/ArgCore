using Arg.DataModels;
using ArgCore.Attributes;
using ArgCore.Helpers;
using ArgCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using static ArgCore.Helpers.IdentityConfig;

namespace ArgCore.Controllers
{
    [Authorize]
    [AuthorizeUser]
    public class UsersController : Controller
    {
        //private ApplicationUserManager _userManager;
        //private ApplicationSignInManager _signInManager;
        //private readonly IHttpContextAccessor _httpContextAccessor;
        //public ApplicationDbContext Database = new ApplicationDbContext();

        //public UsersController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, IHttpContextAccessor httpContextAccessor)
        //{
        //    _userManager = userManager;
        //    _signInManager = signInManager;
        //    _httpContextAccessor = httpContextAccessor;
        //}
        //public ActionResult Index(string q)
        //{
        //    var model = new Users();

        //    try
        //    {
        //        if (_httpContextAccessor.HttpContext.Session.GetString("IsSessionActive") == null)
        //        {
        //            return Redirect(Common.MyRoot + "Account/Login");
        //        }

        //        LoadUsersData(model);

        //        model.SearchOptions = new SearchOptions();
        //        var usersList = Common.AspNetUsers.GetAspNetUsersWithRoles(model.SearchOptions, q);

        //        if (usersList != null && usersList.Any())
        //        {

        //            var users = usersList;
        //            model.UsersList = users;
        //        }
        //        else
        //        {
        //            model.SearchOptions.Message = "No results found related to your search!";
        //        }

        //        var roles = Database.Roles.ToList();
        //        model.Roles = roles;
        //    }
        //    catch (Exception ex)
        //    {
        //        Common.Log.Error(ex);
        //    }

        //    return View(model);
        //}

        //[HttpPost]
        //public ActionResult Index(Users model)
        //{
        //    try
        //    {
        //        LoadUsersData(model);
        //        List<AspNetUsers> users;

        //        if (model.SearchOptions != null)
        //        {
        //            users = Common.AspNetUsers.GetAspNetUsers(model.SearchOptions);
        //        }
        //        else
        //        {
        //            users = Common.AspNetUsers.GetAspNetUsers(new SearchOptions());
        //        }

        //        if (users != null && users.Any())
        //        {
        //            model.UsersList = users;
        //        }
        //        else
        //        {
        //            model.SearchOptions.Message = "No results found related to your search!";
        //        }

        //        var roles = Database.Roles.ToList();
        //        model.Roles = roles;

        //        return View(model);

        //    }
        //    catch (Exception ex)
        //    {
        //        Common.Log.Error(ex);
        //    }

        //    return null;
        //}

        //public void LoadUsersData(Users users)
        //{
        //    users.CommonObjects.TopHeading = "Users";
        //    users.CommonObjects.Heading = "Users";

        //    var roles = Common.AspNetRoles.GetDistinctUserRoles().OrderBy(x => x.Name);
        //    users.RolesDDList = new SelectList(roles, "Id", "Name");

        //    var companies = Common.ArgClients.GetArgClients(Common.CurrentUserId);
        //    users.Companies = new SelectList(companies, "CompanyId", "Name");

        //    var statuses = new List<string> { "Enable", "Disable" };
        //    users.SelectedStatusList = new SelectList(statuses);
        //}

        //[HttpGet]
        //public ActionResult Save(string userId)
        //{

        //    try
        //    {
        //        var users = new Users();

        //        if (_httpContextAccessor.HttpContext.Session.GetString("IsSessionActive") == null)
        //        {
        //            return Redirect(Common.MyRoot + "Account/Login");
        //        }

        //        if (_httpContextAccessor.HttpContext.Session.GetString("ErrorMessges") != null)
        //        {
        //            users.Messages = JsonConvert.DeserializeObject<IEnumerable<string>>(
        //                 _httpContextAccessor.HttpContext.Session.GetString("ErrorMessges")
        //                );

        //            _httpContextAccessor.HttpContext.Session.Remove("ErrorMessges");
        //        }

        //        users.CommonObjects.TopHeading = "Users";

        //        var uId = Convert.ToString(userId);

        //        users.UserDetail = new ApplicationUser();
        //        users.UserId = uId;

        //        if (uId != null)
        //        {
        //            users.CommonObjects.Heading = "Edit User";

        //            var userTask = _userManager.FindByIdAsync(uId);

        //            if (users.UserDetail.Roles.Any())
        //            {
        //                users.RoleId = users.UserDetail.Roles.First().RoleId;
        //            }

        //            if (users.UserDetail == null || users.UserDetail.Id == null)
        //            {
        //                return RedirectToAction("Index", new { m = "Users not found or deleted" });
        //            }

        //        }
        //        else
        //        {
        //            users.CommonObjects.Heading = "Add User";
        //        }

        //        var roles = Database.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Text = rr.Name, Value = rr.Id }).ToList();
        //        users.RolesList = roles;

        //        return View(users);
        //    }
        //    catch (Exception ex)
        //    {
        //        Common.Log.Error(ex);
        //    }

        //    return null;
        //}

        //public ActionResult Delete(string userId)
        //{
        //    try
        //    {
        //        var result = Common.AspNetUsers.DeleteAspNetUser(userId);

        //        if (result > 0)
        //        {
        //            Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Deleted, 0, "Users");
        //            return RedirectToAction("Index");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Common.Log.Error(ex);
        //    }

        //    return RedirectToAction("Index");
        //}
    }
}
