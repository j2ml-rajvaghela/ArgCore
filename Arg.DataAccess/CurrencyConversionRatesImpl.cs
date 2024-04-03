using Arg.DataModels;
using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arg.DataAccess
{
    public class CurrencyConversionRatesImpl
    {
        public IEnumerable<CurrencyConversionRates> GetCurrencyConversionRates(int currencyId = 0)
        {
            var parameters = new DynamicParameters();
            if (currencyId > 0)
            {
                parameters.Add("@CurrencyId", currencyId, DbType.Int32);
            }
            using (var conenction = Common.Database)
            {
                var currencyConversionsRate = conenction.Query<CurrencyConversionRates>("GetConversionRateByCurrencyId", parameters, commandType: CommandType.StoredProcedure).ToList();
                return currencyConversionsRate;
            }
        }

        public decimal GetConversionRate(string currency, DateTime invoiceDate)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@Currency", currency, DbType.String);
            parameters.Add("@InvoiceDate", invoiceDate, DbType.DateTime);

            using (var conenction = Common.Database)
            {
                var currencyConversionRate = conenction.ExecuteScalar<decimal>("GetConversionRate", parameters, commandType: CommandType.StoredProcedure);
                return currencyConversionRate;
            }
        }

        public void SaveCurrencyConversionRate(CurrencyConversionRates currencyConversionRate)
        {
            using (var conenction = Common.Database)
            {
                conenction.Insert(currencyConversionRate);
            }
        }
    }
}
