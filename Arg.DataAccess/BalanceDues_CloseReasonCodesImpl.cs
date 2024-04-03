using Dapper;
using System.ComponentModel.Design;

namespace Arg.DataAccess
{
    public class BalanceDues_CloseReasonCodesImpl
    {
        public List<DataModels.BalanceDues_CloseReasonCodes> GetSelectedCloseReasonCodes(int companyId)
        {
            const string query = @"SELECT *, CONCAT(CloseReasonCode,' ',Description) AS CloseReasonCodeWithDesc 
                                   FROM [BalanceDues.CloseReasonCodes]
                                   WHERE CloseReasonCode IN ('OBILL','UCOLL','MISC','PHCL')
                                   AND (CompanyId=@CompanyId OR @CompanyId = 0)
                                   ORDER BY CloseReasonCode;";

            using (var connection = Common.Database)
            {
                var selectedCloseRasonCodes = connection.Query<DataModels.BalanceDues_CloseReasonCodes>(query, new { @CompanyId = companyId}).ToList(); 
                return selectedCloseRasonCodes;
            }
        }
    }
}
