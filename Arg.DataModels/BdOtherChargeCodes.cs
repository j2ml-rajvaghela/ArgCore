using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Arg.DataModels
{
    [Table("[BalanceDues.OtherChargesCodes]")]
    public class BdOtherChargeCodes
    {
        [Dapper.Contrib.Extensions.Key]
        public int BDOtherChargeCodeId { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [Required]
        public string ChargeCode { get; set; }

        [Required]
        public string Description { get; set; }

        [Computed]
        public string ChargeCodeValue { get; set; }

        [Computed]
        public string Company { get; set; }

        [Computed]
        public string ChargeCodes { get; set; }
    }
}
