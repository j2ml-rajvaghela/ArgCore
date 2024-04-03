using Dapper;
using System.ComponentModel.Design;
using System.Data;
namespace Arg.DataAccess
{
    public class BalanceDues_CollectionStatusesImpl
    {
        public List<DataModels.BalanceDues_CollectionStatuses> GetStatuses(int companyId)
        {
            const string query = @"SELECT * FROM [BalanceDues.CollectionStatuses]
                                   WHERE (CompanyId=@CompanyId OR @CompanyId = 0);";

            using (var connection = Common.Database)
            {
                var statuses = connection.Query<DataModels.BalanceDues_CollectionStatuses>(query, new { @CompanyId = companyId }).ToList();
                return statuses;
            } 
        }
    }
}
