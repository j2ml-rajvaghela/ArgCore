using Dapper.Contrib.Extensions;

namespace Arg.DataModels
{
    [Table("[BalanceDues.CollectionStatuses]")]
    public class BalanceDues_CollectionStatuses
    {
        public int CompanyId { get; set; }
        public string CollectionStatus { get; set; }
        public string Description { get; set; }
    }
}
