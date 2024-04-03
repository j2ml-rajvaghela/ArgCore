using Dapper.Contrib.Extensions;

namespace Arg.DataModels
{
    [Table("AspNetUserRoles")]
    public class AspNetUserRoles
    {
        [Key]
        public string UserId { get; set; }
        public string RoleId { get; set; }

        [Computed]
        public string RoleName { get; set; }
    }
}
