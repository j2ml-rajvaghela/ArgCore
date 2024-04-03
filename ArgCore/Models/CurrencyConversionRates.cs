using ArgCore.Helpers;

namespace ArgCore.Models
{
    public class CurrencyConversionRates
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        public IEnumerable<Arg.DataModels.CurrencyConversionRates> CurrencyConversionRatesList { get; set; }

        public Arg.DataModels.CurrencyConversionRates CurrencyConversionRatesDetail { get; set; }

        public string ErrorMessage { get; set; }
    }
}
