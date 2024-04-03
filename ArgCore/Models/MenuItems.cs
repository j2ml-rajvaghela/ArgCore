using ArgCore.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArgCore.Models
{
    public class MenuItems
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        public List<Arg.DataModels.MenuItems> MenuItemsList { get; set; }

        public Arg.DataModels.MenuItems MenuItemDetail { get; set; }

        public SelectList Menus { get; set; }

        public SelectList ParentMenuItems { get; set; }
    }
}
