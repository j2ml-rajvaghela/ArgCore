using ArgCore.Helpers;

namespace ArgCore.Models
{
    public class Menus
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        public List<Arg.DataModels.Menus> MenusList { get; set; }

        public Arg.DataModels.Menus MenuDetail { get; set; }

        public string RoleId { get; set; }

        public List<Arg.DataModels.MenuItems> MenuItemsList { get; set; }

        public List<Arg.DataModels.MenuItems> AssignedMenuItems { get; set; }
    }
}
