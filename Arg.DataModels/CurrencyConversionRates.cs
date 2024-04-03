using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Arg.DataModels
{
    [Table("CurrencyConversionRates")]
    public class CurrencyConversionRates
    {
        [Dapper.Contrib.Extensions.Key]
        public int CurrencyId { get; set; }

        [Required(ErrorMessage = "Currency Converted To is required.")]
        public string CurrencyConvertedTo { get; set; }

        [Required(ErrorMessage = "Conversion Rate is required.")]
        public int ConversionRate { get; set; }

        [Required(ErrorMessage = "Conversion Date is required.")]
        public DateTime ConversionDate { get; set; }

        [Required(ErrorMessage = "Base Currency is required.")]
        public string BaseCurrency { get; set; }
    }
}
