using Dapper.Contrib.Extensions;

namespace Arg.DataModels
{
    [Table("AppActionRoleRels")]
    public class AppActionRoleRels
    {

        [Key]
        public int RelId { get; set; }
        public string RoleId { get; set; }
        public int AppActionId { get; set; }
    }
}
