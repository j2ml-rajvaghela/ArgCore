using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Arg.DataModels
{
    [Table("AppActions")]
    public class AppActions
    {
        [Dapper.Contrib.Extensions.Key]
        public int AppActionId { get; set; }

        [Required]
        public string ActionName { get; set; }

        [Computed]
        public string RoleId { get; set; }
    }
}
