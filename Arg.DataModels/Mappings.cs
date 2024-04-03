using Dapper.Contrib.Extensions;

namespace Arg.DataModels
{
    public class Mappings
    {
        [Table("Mappings")]
        public class Mapping
        {
            [Key]
            public int MappingId { get; set; }

            public string SourceColName { get; set; }

            public int SourceColIndex { get; set; }

            public string TargetTableName { get; set; }

            public string TargetColName { get; set; }

            public string SourceFileName { get; set; }
        }
    }
}
