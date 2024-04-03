using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Arg.DataModels
{
    [Table("[BalanceDues.CloseReasonCodes]")]
    public class BalanceDues_CloseReasonCodes
    {
        [Dapper.Contrib.Extensions.Key]
        public int CloseReasonCodeId { get; set; }

        [Required(ErrorMessage = "The Company field is required")]
        public int CompanyId { get; set; }

        [Required]
        public string CloseReasonCode { get; set; }

        [Required]
        public string Description { get; set; }

        [Computed]
        public string CloseReasonCodeWithDesc { get; set; }
    }
}
