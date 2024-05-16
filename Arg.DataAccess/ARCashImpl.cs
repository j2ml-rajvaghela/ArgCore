using Dapper;
using System.Data;

namespace Arg.DataAccess
{
    public class ARCashImpl
    {
        public decimal GetAmountPaid(string bolNo, string customerId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@BOL#", bolNo, DbType.String);
            parameters.Add("@PayorID", customerId, DbType.String);

            const string query = @"SELECT ISNUll(SUM(AmountPaid),0) AS AmountPaid FROM ARCash
                                   WHERE BOL#=@BOL AND PayorID=@PayorID;";

            using var connection = Common.ClientDatabase;
            var amountPaid = connection.ExecuteScalar<decimal>(query, new { bolNo, customerId });
            return amountPaid;
        }
    }
}
