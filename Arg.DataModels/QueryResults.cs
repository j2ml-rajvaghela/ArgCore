using Dapper.Contrib.Extensions;

namespace Arg.DataModels
{
    [Table("QueryResults")]
    public class QueryResults
    {
        [Key]
        public int QueryId { get; set; }
        public byte[] VARBINARY { get; set; }
        public string QueryJson { get; set; }
        public int ResultCount { get; set; }
        public string SqlQuery { get; set; }

        [Computed]
        public string Idx { get; set; }

        [Computed]
        public bool ResultTableFormat { get; set; }

        [Computed]
        public string Message { get; set; }
    }
}
