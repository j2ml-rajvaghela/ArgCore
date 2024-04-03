using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arg.DataModels
{
    [Dapper.Contrib.Extensions.Table("[BalanceDues.OtherCharges]")]
    public class BalanceDues_OtherCharges
    {
        [Dapper.Contrib.Extensions.Key]
        public int ItemId { get; set; }

        [Required(ErrorMessage = "The Company field is required")]
        public int CompanyId { get; set; }

        [Required]
        public string Region { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string CustomerId { get; set; }

        [Required, Column("BOL#")]
        public string BOLNo { get; set; }

        [Required(ErrorMessage = "This field is required"), Column("TariffRef#")]
        public string TariffRefNo { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string ChargeCode { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public decimal AmountDue { get; set; }

        [Computed]
        public string ChargeCodeDesc { get; set; }

        [Computed]

        public string Currency { get; set; }

        [Computed]
        public decimal AmountPaid { get; set; }
    }
}
