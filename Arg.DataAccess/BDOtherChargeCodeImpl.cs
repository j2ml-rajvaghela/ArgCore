using Dapper;
using System.ComponentModel.Design;
using System.Data;
namespace Arg.DataAccess
{
    public class BDOtherChargeCodeImpl
    {
        public List<DataModels.BDOtherChargeCode> GetDistinctChargeCodes(int companyId, bool filterbalanceDue = false)
        {
            const string query = @"SELECT DISTINCT c.ChargeCode, c.Description, CONCAT(c.ChargeCode, ' (', c.Description, ')') AS ChargeCodes
                                   FROM [BalanceDues.OtherChargesCodes] c
                                   WHERE (@FilterBalanceDue = 0 OR c.CompanyId = @CompanyId OR @CompanyId = 0)
                                   ORDER BY c.ChargeCode;";

            var parameters = new { @CompanyId = companyId, @FilterBalanceDue = filterbalanceDue };

            using (var connection = Common.Database)
            {
                var distinctChargeCodes = connection.Query<DataModels.BDOtherChargeCode>(query, parameters).ToList();
                return distinctChargeCodes;
            }
        }
        public List<DataModels.BDOtherChargeCode> GetDistinctChargeCodes()
        {
            using (var connection = Common.Database)
            {
                var distinctChargeCodes = connection.Query<DataModels.BDOtherChargeCode>("GetDistinctChargeCodes", commandType: CommandType.StoredProcedure).ToList();
                return distinctChargeCodes;
            }
        }
    }
}
