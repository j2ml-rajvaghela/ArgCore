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
    public class BalanceDues_OtherChargesImpl
    {
        public List<BalanceDues_OtherCharges> GetBalanceDuesOtherCharges(string bolNo)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@BOL#", bolNo, DbType.String);

            using (var connection = Common.Database)
            {
                var balanceDuesOtherCharges = connection.Query<BalanceDues_OtherCharges>("GetBalanceDuesOtherChargesByBOLNo", parameters, commandType: CommandType.StoredProcedure).ToList();
                return balanceDuesOtherCharges;
            }
        }

        public List<BalanceDues_OtherCharges> GetBalanceDuesOtherChargesWithDesc(string BolNo, int companyId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Bol#", BolNo, DbType.String);
            parameters.Add("@CompanyId", companyId, DbType.Int32);

            using (var connection = Common.Database)
            {
                var otherChargesWithDesc = connection.Query<BalanceDues_OtherCharges>("GetBalanceDuesOtherChargesWithDesc", parameters, commandType: CommandType.StoredProcedure).ToList();
                return otherChargesWithDesc;
            }
        }

        public void SaveBDOtherCharges(List<BalanceDues_OtherCharges> bolCharges)
        {
            using (var connection = Common.Database)
            {
                foreach (var charge in bolCharges)
                {
                    charge.ItemId = 0;
                    connection.Insert(charge);
                }
            }
        }

        public int DeleteBolOtherCharges(int itemId)
        {
            const string query = @"DELETE FROM [BalanceDues.OtherCharges] WHERE ItemId=@ItemId";

            using (var connection = Common.Database)
            {
                var result = connection.Execute(query, new { @ItemId = itemId });
                return Convert.ToInt32 (result);
            }
        }

        public int DeleteBolOtherCharges(int companyId, string bolNo, string customerId, string region)
        {
            const string query = @"DELETE FROM [BalanceDues.OtherCharges] WHERE  CompanyId=@CompanyId AND Bol#=@BolNo AND CustomerId=@CustomerId AND Region=@Region";

            using (var connection = Common.Database)
            {
                var result = connection.Execute(query, new { @CompanyId = companyId, @BolNo = bolNo, @CustomerId = customerId, @Region = region });
                return Convert.ToInt32(result);
            }
        }
    }
}
