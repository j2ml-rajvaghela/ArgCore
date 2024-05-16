using Arg.DataModels;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace Arg.DataAccess
{
    public class BalanceDues_OtherChargesImpl
    {
        public List<BalanceDues_OtherCharges> GetBalanceDuesOtherCharges(string bolNo)
        {
            using var connection = Common.Database;
            var balanceDuesOtherCharges = connection.Query<BalanceDues_OtherCharges>("GetBalanceDuesOtherChargesByBOLNo", new { bolNo }, commandType: CommandType.StoredProcedure).ToList();
            return balanceDuesOtherCharges;
        }

        public List<BalanceDues_OtherCharges> GetBalanceDuesOtherChargesWithDesc(string bolNo, int companyId)
        {
            using var connection = Common.Database;
            var otherChargesWithDesc = connection.Query<BalanceDues_OtherCharges>("GetBalanceDuesOtherChargesWithDesc", new { bolNo, companyId }, commandType: CommandType.StoredProcedure).ToList();
            return otherChargesWithDesc;
        }

        public void SaveBDOtherCharges(List<BalanceDues_OtherCharges> bolCharges)
        {
            using var connection = Common.Database;
            foreach (var charge in bolCharges)
            {
                charge.ItemId = 0;
                connection.Insert(charge);
            }
        }

        public int DeleteBolOtherCharges(int itemId)
        {
            const string query = @"DELETE FROM [BalanceDues.OtherCharges] WHERE ItemId=@ItemId";

            using var connection = Common.Database;
            var result = connection.Execute(query, new { itemId });
            return Convert.ToInt32(result);
        }

        public int DeleteBolOtherCharges(int companyId, string bolNo, string customerId, string region)
        {
            const string query = @"DELETE FROM [BalanceDues.OtherCharges] 
                                   WHERE CompanyId=@CompanyId AND Bol#=@BolNo AND CustomerId=@CustomerId AND Region=@Region";

            using var connection = Common.Database;
            var result = connection.Execute(query, new { companyId, bolNo, customerId, region });
            return Convert.ToInt32(result);
        }
    }
}
