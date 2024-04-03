using ArgCore.Attributes;
using ArgCore.Helpers;
using ArgCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArgCore.Controllers
{
    [Authorize]
    public class MenuItemsController : Controller
    {
        public IActionResult Index(string q)
        {
            var model = new MenuItems();
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                model.CommonObjects.TopHeading = "Menu Items";
                model.CommonObjects.Heading = "Menu Items";

                var menuItems = Common.MenuItems.GetMenuItems(q);
                if (menuItems != null && menuItems.Any())
                {
                    model.MenuItemsList = menuItems;
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Save(int? itemId)
        {
            try
            {
                //if (HttpContext.Session.GetString("IsSessionActive") == null)
                //{
                //    return RedirectToAction("LogIn", "Account");
                //}

                var menuItems = new MenuItems();
                menuItems.CommonObjects.TopHeading = "Menu Items";

                var menus = Common.Menus.GetMenus("");
                menuItems.Menus = new SelectList(menus, "MenuId", "DisplayName");

                var parents = Common.MenuItems.GetParentMenuItems();
                menuItems.ParentMenuItems = new SelectList(parents, "ItemId", "DisplayName");

                menuItems.MenuItemDetail = new Arg.DataModels.MenuItems();

                var _itemId = Convert.ToInt32(itemId);
                if (_itemId > 0)
                {
                    menuItems.CommonObjects.Heading = "Edit Menu Item";
                    menuItems.MenuItemDetail = Common.MenuItems.GetMenuItem(_itemId);
                    if (menuItems.MenuItemDetail == null || menuItems.MenuItemDetail.MenuId <= 0)
                    {
                        return RedirectToAction("MenuItems", new { m = "Items not found or deleted" });
                    }

                }
                else
                {
                    menuItems.CommonObjects.Heading = "Add Menu Item";
                }
                return View(menuItems);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return null;
        }

        [HttpPost]
        public IActionResult Save(MenuItems menuItem)
        {
            try
            {
                menuItem.MenuItemDetail.LastModBy = 1;
                menuItem.MenuItemDetail.AddedBy = 1;
                menuItem.MenuItemDetail.LastModOn = DateTime.UtcNow;
                menuItem.MenuItemDetail.AddedOn = DateTime.UtcNow;

                Common.MenuItems.SaveMenuItem(menuItem.MenuItemDetail);

                if (menuItem.MenuItemDetail.ItemId > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Saved, 0, "Menu Items");
                    return RedirectToAction("Index", "MenuItems"); 
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            return RedirectToAction("Index", "MenuItems");
        }

        [AuthorizeUser]
        public IActionResult Delete(int itemId)
        {
            try
            {
                var result = Common.MenuItems.DeleteMenuItem(itemId);
                if (result > 0)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.Deleted, 0, "Menu Items");
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
