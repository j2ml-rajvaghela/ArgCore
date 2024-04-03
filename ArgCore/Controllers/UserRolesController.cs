//using ArgCore.Helpers;
//using ArgCore.Models;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
//using Microsoft.AspNetCore.Mvc;


//namespace ArgCore.Controllers
//{
//    public class UserRolesController : Controller
//    {
//        private readonly ApplicationDbContext _context;

//        public UserRolesController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public ActionResult Index(string q)
//        {
//            var userRoles = new UserRoles();
//            try
//            {
//                //if (HttpContext.Session.GetString("IsSessionActive") == null)
//                //{
//                //    return RedirectToAction("LogIn", "Account");
//                //}

//                userRoles.CommonObjects.TopHeading = "User Roles";
//                userRoles.CommonObjects.Heading = "User Roles";

//                var roles = Common.AspNetRoles.GetRolesWithUserCount((!string.IsNullOrWhiteSpace(q) ? q : ""));
//                if (roles != null && roles.Any())
//                {
//                    userRoles.RolesList = roles.OrderBy(x => x.Name).ToList();
//                }
//            }
//            catch (Exception ex)
//            {
//                Common.Log.Error(ex);
//            }
//            return View(userRoles);
//        }

//        [HttpGet]
//        public IActionResult Save(string roleId)
//        {

//                try
//                {
//                    //if (HttpContext.Session.GetString("IsSessionActive") == null)
//                    //{
//                    //    return Redirect(Common.MyRoot + "Account/Login");
//                    //}

//                    var userRoles = new UserRoles();
//                    var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));

//                    userRoles.CommonObjects.TopHeading = "User Roles";
//                    userRoles.RoleDetail = new IdentityRole();
//                    //data.RoleDetail = new Arg.DataModels.AspNetRoles();

//                    var rId = Convert.ToString(roleId);

//                    if (roleId != null)
//                    {
//                        userRoles.CommonObjects.Heading = "Edit User Role";
//                        //data.RoleDetail = ARG.Common.AspNetRoles.GetAspNetRole(rId);
//                        userRoles.RoleDetail = RoleManager.FindById(rId);
//                        if (userRoles.RoleDetail == null || userRoles.RoleDetail.Id == null)
//                        {
//                            return RedirectToAction("UserRoles", new { m = "User Roles not found or deleted" });
//                        }

//                    }
//                    else
//                    {
//                        userRoles.CommonObjects.Heading = "Add User Role";
//                    }

//                    return View(userRoles);
//                }
//                catch (Exception ex)
//                {
//                    Common.Log.Error(ex);
//                }
//                return null;
//            }

//        [HttpPost]
//        public IActionResult Save(UserRoles roles)
//        {
//            try
//            {
//                //_dbContext.Set<UserEntity>().AddOrUpdate(entityToBeUpdatedWithId);
//                //ARG.Common.AspNetRoles.SaveUserRole(roles.RoleDetail);
//                //if (roles.RoleDetail.Id != null)
//                //    Redirect(ARG.Common.MyRoot + "UserRoles/Index");
//                var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));
//                var role = RoleManager.FindById(roles.RoleDetail.Id);
//                if (role != null && role.Id != null)
//                {
//                    //Database.Set<UserEntity>().AddOrUpdate(entityToBeUpdatedWithId);
//                    //ARG.Common.AspNetRoles.SaveUserRole(roles.RoleDetail);
//                    //Database.As.Attach(roles.RoleDetail);
//                    role.Name = roles.RoleDetail.Name;
//                    _context.Entry(role).State = System.Data.Entity.EntityState.Modified;
//                }
//                else
//                {
//                    _context.Roles.Add(new IdentityRole
//                    {
//                        Name = roles.RoleDetail.Name
//                    });
//                }
//                _context.SaveChanges();
//                Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Saved, 0, "User Roles");
//            }
//            //catch (DbEntityValidationException e)
//            //{
//            //    foreach (var eve in e.EntityValidationErrors)
//            //    {
//            //        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
//            //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
//            //        foreach (var ve in eve.ValidationErrors)
//            //        {
//            //            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
//            //                ve.PropertyName, ve.ErrorMessage);
//            //        }
//            //    }
//            //    throw;
//            //}
//            catch (Exception ex)
//            {
//               Common.Log.Error(ex);
//            }
//            return RedirectToAction("Index","UserRoles");
//        }

//        public ActionResult Delete(string roleId)
//        {
//            try
//            {
//                var result = Common.AspNetRoles.DeleteUserRole(roleId);
//                if (result > 0)
//                {
//                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Deleted, 0, "User Roles");
//                    return RedirectToAction("Index");
//                }
//            }
//            catch (Exception ex)
//            {
//                Common.Log.Error(ex);
//            }
//            return RedirectToAction("Index");
//        }
//    }
//}
