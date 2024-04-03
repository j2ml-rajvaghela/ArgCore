using Dapper.Contrib.Extensions;

namespace Arg.DataModels
{
    [Table("AspNetUserRoles")]
    public class AspNetRoles
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }

        [Computed]
        public int UsersCount { get; set; }

        [Computed]
        public string UserId { get; set; }
    }
}
