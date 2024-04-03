using ArgCore.Attributes;
using ArgCore.Helpers;
using ArgCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArgCore.Controllers
{
    [Authorize]
    [AuthorizeUser]
    public class MenusController : Controller
    {
        public IActionResult Index(string q)
        {

            var menus = new Menus();
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                menus.CommonObjects.TopHeading = "Menus";
                menus.CommonObjects.Heading = "Menus";

                var menuItems = new Arg.DataAccess.MenusImpl().GetMenus(q);
                if (menuItems != null && menuItems.Any())
                {
                    menus.MenusList = menuItems;
                }

            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return View(menus);
        }

        [HttpGet]
        public IActionResult Save(int? menuId)
        {
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                var data = new Menus();
                data.CommonObjects.TopHeading = "Menus";
                data.MenuDetail = new Arg.DataModels.Menus();
                var menus = Common.Menus.GetMenus("");
                var _menuId = Convert.ToInt32(menuId);

                if (_menuId > 0)
                {
                    data.CommonObjects.Heading = "Edit Menu";
                    data.MenuDetail = Common.Menus.GetMenu(_menuId);
                    if (data.MenuDetail == null || data.MenuDetail.MenuId <= 0)
                    {
                        return RedirectToAction("Menus", new { m = "Items not found or deleted" });
                    }
                       
                }
                else
                {
                    data.CommonObjects.Heading = "Add Menu";
                }
                return View(data);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return null;
        }

        [HttpPost]
        public IActionResult Save(Menus menu)
        {
            try
            {
                menu.MenuDetail.LastModBy = 1;
                menu.MenuDetail.AddedBy = 1;
                menu.MenuDetail.LastModOn = DateTime.UtcNow;
                menu.MenuDetail.AddedOn = DateTime.UtcNow;

                Common.Menus.SaveMenu(menu.MenuDetail);

                if (menu.MenuDetail.MenuId > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Saved, 0, "Menus");
                    RedirectToAction("Index", "Menus");
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return RedirectToAction("Index", "Menus");
        }

        public IActionResult Assign(string roleId)
        {
            var menus = new Menus();
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                menus.CommonObjects.TopHeading = "Manage Permissions";
                menus.RoleId = Convert.ToString(roleId);

                var items = Common.MenuItems.GetMenuItems("");
                if (items != null && items.Any())
                {
                    menus.MenuItemsList = items.OrderBy(z => z.Name).ToList();
                }

                var assignMenus = Common.MenuItems.GetRelatedMenuItems(roleId);
                if (assignMenus != null && assignMenus.Any())
                {
                    menus.AssignedMenuItems = assignMenus.OrderBy(x => x.Name).ToList();
                    menus.MenuItemsList.RemoveAll(item => menus.AssignedMenuItems.Exists(y => y.ItemId == item.ItemId));
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.ToString());
            }
            return View(menus);
        }

        [HttpPost]
        public void AssignMenuItem(string roleId, int itemId)
        {
            try
            {
                Common.RoleMenuRels.AssignMenuItem(roleId, itemId);
                Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Saved, 0, "Menu Item in RoleMenuRels");
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
        }

        [HttpPost]
        public void RemoveAssignedMenuItem(string roleId, int itemId)
        {
            try
            {
                Common.RoleMenuRels.RemoveAssignedMenuItem(roleId, itemId);
                Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Deleted, 0, "Menu Item from RoleMenuRels");
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
        }

        public IActionResult Delete(int menuId)
        {
            try
            {
                var result = Common.Menus.DeleteMenu(menuId);
                if (result > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Deleted, 0, "Menus");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return RedirectToAction("Index");
        }

    }
}
