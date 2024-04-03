using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Arg.DataModels
{
    [Table("[ResearchItems.ReasonCodes]")]
    public class RSReasonCodes
    {
        [Dapper.Contrib.Extensions.Key]
        public int ReasonCodeId { get; set; }

        [Required]
        public string ReasonCode { get; set; }
    }
}
