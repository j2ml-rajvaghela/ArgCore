using Arg.DataModels;
using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arg.DataAccess
{
    public class BdErrorCodesImpl
    {
        public List<BdErrorCodes> GetErrorCodes(int companyId)
        {
            var parameters = new DynamicParameters();
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }
            using (var connection = Common.Database)
            {
                var errorCodes = connection.Query<BdErrorCodes>("GetErrorCodes", parameters, commandType: CommandType.StoredProcedure).ToList();
                return errorCodes;
            }
        }

        public BdErrorCodes GetErrorCode(int errorCodeId, int companyId, string bdErrorCode = "")
        {
            var parameters = new DynamicParameters();
            if (errorCodeId > 0)
            {
                parameters.Add("@ErrorCodeId", errorCodeId, DbType.Int32);
            }
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }
            if (!string.IsNullOrWhiteSpace(bdErrorCode))
            {
                parameters.Add("@BdErrorCode", bdErrorCode, DbType.String);
            }
            using (var connection = Common.Database)
            {
                var errorCode = connection.QueryFirstOrDefault<BdErrorCodes>("GetErrorCode", parameters, commandType: CommandType.StoredProcedure);
                return errorCode;
            }
        }

        public List<BdErrorCodes> ErrorCodesExist(int companyId, string bdErrorCode, int errorCodeId)
        {
            var parameters = new DynamicParameters();
            if (errorCodeId > 0)
            {
                parameters.Add("@ErrorCodeId", errorCodeId, DbType.Int32);
            }
            parameters.Add("@CompanyId", companyId, DbType.Int32);
            parameters.Add("@bdErrorCode", bdErrorCode, DbType.String);

            using (var connection = Common.Database)
            {
                var errorCodesExists = connection.Query<BdErrorCodes>("GetErrorCode", parameters, commandType: CommandType.StoredProcedure).ToList();
                return errorCodesExists;
            }
        }

        public List<BdErrorCodes> GetDistinctErrorCodesCeva(int companyId)
        {
            var parameters = new DynamicParameters();

            if (companyId > 0)
            {
                parameters.Add("@ComapnyId", companyId, DbType.Int32);
            }

            using(var connection = Common.Database)
            {
                var distinctErrorCodesCeva = connection.Query<BdErrorCodes>("GetDistinctErrorCodesCeva", parameters, commandType: CommandType.StoredProcedure).ToList();
                return distinctErrorCodesCeva;
            }
        }

        public List<BdErrorCodes> GetDistinctErrorCodes(int companyId, bool filterbalanceDue = false, string bol = "", string invoiceType = "")
        {
            const string query = @"SELECT DISTINCT c.BdErrorCode, c.Description, CONCAT(c.BdErrorCode, ' (', c.Description, ')') AS ErrorCodes
                                   FROM BdErrorCodes c
                                   LEFT JOIN BalanceDues d ON d.BdErrorCode = c.BdErrorCode
                                   WHERE (@FilterBalanceDue = 0 OR d.CompanyId = @CompanyId)
                                   AND (c.CompanyId = @CompanyId OR @CompanyId = 0)
                                   AND (
                                        @InvoiceType = ''
                                        OR (@InvoiceType LIKE '%over%' AND LEFT(c.BdErrorCode, 2) = 'OC')
                                        OR (@InvoiceType LIKE '%under%' AND LEFT(c.BdErrorCode, 2) = 'UB')
                                       )
                                  ORDER BY c.BdErrorCode;"
            ;

            var parameters = new { CompanyId = companyId, FilterBalanceDue = filterbalanceDue, InvoiceType = invoiceType };

            using (var connection = Common.Database)
            {
                var distinctErrorCodes = connection.Query<BdErrorCodes>(query,parameters).ToList();
                return distinctErrorCodes;
            }
        }

        public void SaveBdErrorCode(BdErrorCodes bdErrorCode)
        {
            using (var connection = Common.Database)
            {
                 if (bdErrorCode.ErrorCodeId == 0)
                 {
                    connection.Insert(bdErrorCode);
                 }
                 else
                 {
                    connection.Update(bdErrorCode);
                 }
            }
        }

        public int DeleteBdErrorCode(int errorCodeId)
        {
            const string query = @"DELETE FROM BdErrorCodes WHERE ErrorCodeId=@ErrorCodeId;";
            using (var connection = Common.Database)
            {
                var result = connection.Execute(query, new { @ErrorCodeId = errorCodeId });
                return result;
            }
        }
    }
}
