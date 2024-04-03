using Dapper.Contrib.Extensions;

namespace Arg.DataModels
{
    [Table("RoleMenuRels")]
    public class RoleMenuRels
    {
        [Key]
        public int RelId { get; set; }
        public int ItemId { get; set; }
        public string RoleId { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime LastModOn { get; set; }
        public int AddedBy { get; set; }
        public int LastModBy { get; set; }
    }
}
