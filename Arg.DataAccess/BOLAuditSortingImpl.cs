using Arg.DataModels;
using Dapper;
using Dapper.Contrib.Extensions;

namespace Arg.DataAccess
{
    public class BOLAuditSortingImpl
    {
        public List<DataModels.BOLAuditSorting> GetQueryResults(int clientId, string loginId)
        {
            const string query = @"SELECT * BOLAuditSorting 
                                   WHERE ((ClientID=@ClientId OR @ClientId = 0) AND LoginID=@LoginId);";

            using var connection = Common.Database;
            var queryResults = connection.Query<DataModels.BOLAuditSorting>(query, new { clientId, loginId }).ToList();
            return queryResults;
        }

        public void SaveSortingOrder(BOLAuditSorting auditSorting)
        {
            using var connection = Common.Database;
            connection.Insert(auditSorting);
        }

        public int DeleteSortingOrder(int clientId, string loginId)
        {
            const string query = @"DELETE FROM BOLAuditSorting 
                                   WHERE ClientID=@ClientId AND LoginID=@LoginId;";

            using var connection = Common.Database;
            var deleteSorting = connection.Execute(query, new { clientId, loginId });
            return deleteSorting;
        }
    }
}
