using Arg.DataModels;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace Arg.DataAccess
{
    public class BdOtherChargeCodesImpl
    {
        public List<BdOtherChargeCodes> GetOtherChargeCodes(int companyId, int bdChargeCodeId = 0)
        {
            var parameters = new DynamicParameters();
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }
            if (bdChargeCodeId > 0)
            {
                parameters.Add("@BdChargeCodeId", bdChargeCodeId, DbType.Int32);
            }

            using var connection = Common.Database;
            var otherChargeCodes = connection.Query<BdOtherChargeCodes>("GetOtherChargeCodes", parameters, commandType: CommandType.StoredProcedure).ToList();
            return otherChargeCodes;
        }

        public BdOtherChargeCodes GetOtherChargeCode(int otherChargeCodeId, int companyId)
        {
            var parameters = new DynamicParameters();
            if (otherChargeCodeId > 0)
            {
                parameters.Add("@OtherChargeCodeId", otherChargeCodeId, DbType.Int32);
            }
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }

            using var connection = Common.Database;
            var otherChargeCode = connection.QueryFirstOrDefault<BdOtherChargeCodes>("GetOtherChargeCode", parameters, commandType: CommandType.StoredProcedure);
            return otherChargeCode;
        }

        public List<BdOtherChargeCodes> OtherChargeCodeExist(int companyId, string chargeCode, int bdOtherChargeCodeId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@ChargeCode", chargeCode, DbType.String);
            parameters.Add("@CompanyId", companyId, DbType.Int32);
            if (bdOtherChargeCodeId > 0)
            {
                parameters.Add("@BdOtherChargeCodeId", bdOtherChargeCodeId, DbType.Int32);
            }

            using var connection = Common.Database;
            var isOtherChargeCodesExist = connection.Query<BdOtherChargeCodes>("OtherChargeCodeExist", parameters, commandType: CommandType.StoredProcedure).ToList();
            return isOtherChargeCodesExist;
        }

        public void SaveBdOtherChargeCode(BdOtherChargeCodes bdOtherChargeCodes)
        {
            using var connection = Common.Database;
            if (bdOtherChargeCodes.BDOtherChargeCodeId == 0)
            {
                connection.Insert(bdOtherChargeCodes);
            }
            else
            {
                connection.Update(bdOtherChargeCodes);
            }
        }

        public int DeleteBdOtherChargeCode(int otherChargeCodeId)
        {
            const string cmd = @"DELETE FROM [BalanceDues.OtherChargesCodes] 
                                 WHERE BDOtherChargeCodeId=@OtherChargeCodeId;";

            using var connection = Common.Database;
            var result = connection.Execute(cmd, new { otherChargeCodeId });
            return Convert.ToInt32(result);
        }
    }
}
