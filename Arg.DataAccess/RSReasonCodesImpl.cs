using Arg.DataModels;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace Arg.DataAccess
{
    public class RSReasonCodesImpl
    {
        public List<RSReasonCodes> GetReasonCodes()
        {
            using var connection = Common.Database;
            var reasonCodes = connection.Query<RSReasonCodes>("GetReasonCodes", commandType: CommandType.StoredProcedure).ToList();
            return reasonCodes;
        }

        public RSReasonCodes GetReasonCode(int reasonCodeId)
        {
            var parameters = new DynamicParameters();
            if (reasonCodeId > 0)
            {
                parameters.Add("@ReasonCodeId", reasonCodeId, DbType.Int32);
            }
            using var connection = Common.Database;
            var reasonCode = connection.QueryFirstOrDefault<RSReasonCodes>("GetReasonCodeByReasonCodeId", parameters, commandType: CommandType.StoredProcedure);
            return reasonCode;
        }

        public List<RSReasonCodes> GetDistinctReasonCodes()
        {
            using var connection = Common.Database;
            var distinctReasonCode = connection.Query<RSReasonCodes>("GetDistinctReasonCodes", commandType: CommandType.StoredProcedure).ToList();
            return distinctReasonCode;
        }

        public List<RSReasonCodes> ReasonCodeExist(string name, int reasonCodeId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Name", name, DbType.String);
            if (reasonCodeId > 0)
            {
                parameters.Add("@ReasonCodeId", reasonCodeId, DbType.Int32);
            }
            using var connection = Common.Database;
            var isReasonCodesExist = connection.Query<RSReasonCodes>("ReasonCodeExist", parameters, commandType: CommandType.StoredProcedure).ToList();
            return isReasonCodesExist;
        }

        public void SaveRSReasonCode(RSReasonCodes rSReasonCode)
        {
            using var connection = Common.Database;
            if (rSReasonCode.ReasonCodeId == 0)
            {
                connection.Insert(rSReasonCode);
            }
            else
            {
                connection.Update(rSReasonCode);
            }
        }

        public int DeleteReasonCode(int reasonCodeId)
        {
            const string query = @"DELETE FROM [ResearchItems.ReasonCodes] 
                                   WHERE ReasonCodeId=@ReasonCodeId;";

            using var connection = Common.Database;
            var result = connection.Execute(query, new { reasonCodeId });
            return result;
        }
    }
}
