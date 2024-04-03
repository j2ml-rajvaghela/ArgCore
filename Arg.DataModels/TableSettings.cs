using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Arg.DataModels
{
    [Table("TableSettings")]
    public class TableSettings
    {
        [Dapper.Contrib.Extensions.Key]
        public int TableSettId { get; set; }

        [Required]
        public string TableName { get; set; }

        [Required]
        [Display(Name = "Truncate Table")]
        public bool TruncateTable { get; set; }
    }
}
