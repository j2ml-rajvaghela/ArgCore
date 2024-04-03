using Arg.DataModels;
using ArgCore.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ArgCore.Models
{
    public class Users
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();
        public string Query { get; set; }
        public IEnumerable<string> Messages { get; set; }
        public SearchOptions SearchOptions { get; set; }

        //public List<ApplicationUser> UsersList { get; set; } // By Default Comment
        public ApplicationUser UserDetail { get; set; }

        [RegularExpression("(?=.*[0-9])(?=.*[!@#$%&])[0-9a-zA-Z!@#$%^&0-9]{10,}$", ErrorMessage = "Passwords must have at least one non letter or digit character.<br />Passwords must have at least one digit ('0'-'9').<br />Passwords must have at least one lowercase ('A'-'Z'). <br />Passwords must have at least one uppercase ('A'-'Z').")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 10)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [Required]
        public string Password { get; set; }

        [Required]
        public string RoleId { get; set; }

        public string UserId { get; set; }

        public List<SelectListItem> RolesList { get; set; }

        public SelectList SelectedStatusList { get; set; }

        public List<AspNetUsers> UsersList { get; set; }

        public int CompanyId { get; set; }

        public SelectList Companies { get; set; }

        public List<Microsoft.AspNet.Identity.EntityFramework.IdentityRole> Roles { get; set; }

        public SelectList RolesDDList { get; set; }
    }
}
