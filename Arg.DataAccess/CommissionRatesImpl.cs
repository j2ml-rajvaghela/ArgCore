using Arg.DataModels;
using Dapper;
using System.Data;

namespace Arg.DataAccess
{
    public class CommissionRatesImpl
    {
        public List<CommissionRates> GetCommissionRates()
        {
            using var connection = Common.Database;
            var commissionRates = connection.Query<CommissionRates>("GetAllCommissionRates", commandType: CommandType.StoredProcedure).ToList();
            return commissionRates;
        }

        public List<CommissionRates> GetMultipleRates(string argInvoiceNo)
        {
            using var connection = Common.ClientDatabase;
            var multipleRates = connection.Query<CommissionRates>("GetMultipleRates", new { argInvoiceNo }, commandType: CommandType.StoredProcedure).ToList();
            return multipleRates;
        }


    }
}
