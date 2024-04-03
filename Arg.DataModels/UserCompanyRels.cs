using Dapper.Contrib.Extensions;

namespace Arg.DataModels
{
    [Table("UserCompanyRels")]
    public class UserCompanyRels
    {
        [Key]
        public int RelId { get; set; }
        public string UserId { get; set; }
        public int CompanyId { get; set; }
    }
}
