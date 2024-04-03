using Dapper.Contrib.Extensions;

namespace Arg.DataModels
{
    [Table("Modes")]
    public class Modes
    {
        public string Mode { get; set; }
        public string Description { get; set; }
    }
}
