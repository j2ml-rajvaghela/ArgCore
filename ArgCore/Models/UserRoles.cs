using Arg.DataModels;
using ArgCore.Helpers;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ArgCore.Models
{
    public class UserRoles
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        public List<AspNetRoles> RolesList { get; set; }
        //public List<Common.UsersRole> RolesList { get; set; } // By Default Comment

        public IdentityRole RoleDetail { get; set; }

        //public Arg.DataModels.AspNetRoles RoleDetail { get; set; } // By Default Comment
    }
}
