using Dapper;
using Dapper.Contrib.Extensions;
using System.ComponentModel.Design;

namespace Arg.DataAccess
{
    public class AuditorPlaybooksImpl
    {
        private string _dbName = Common.DefaultDbName;

        public List<DataModels.AuditorPlaybook> GetAuditorPlaybooks()
        {
            string query = $"SELECT a.*,qr.QueryJson AS QueryJson,LEFT(qr.QueryJson,10) AS TrimAuditingScreenFilters,qr.SqlQuery AS SqlQuery,u.LastName,u.FirstName," +
                           $"(SELECT TOP(1) LEFT(p.Comment,50) FROM PlaybookComments p" +
                           $"WHERE p.PlayId=a.PlayID ORDER BY p.AddedOn DESC) AS PlaybookComment FROM AuditorPlaybook a" +
                           $"INNER JOIN {_dbName}.dbo.AspNetUsers u ON u.Id = a.UserID" +
                           $"INNER JOIN QueryResults qr ON qr.QueryId = a.QueryId" +
                           $"WHERE a.Status <> 3" +
                           $"ORDER BY a.Region,a.Status,a.Priority,u.FirstName;";

            using (var connection = Common.ClientDatabase)
            {
                var auditorPlaybooks = connection.Query<DataModels.AuditorPlaybook>(query).ToList();
                return auditorPlaybooks;
            }
        }

        public DataModels.AuditorPlaybook GetAuditorPlaybook(int playId, int companyId)
        {
            string query = $"SELECT a.*,u.LastName,u.FirstName,u.UserName FROM AuditorPlaybook a" +
                                 $"INNER JOIN {_dbName}.dbo.AspNetUsers u  ON u.Id = a.UserID" +
                                 $"WHERE (a.PlayID = @PlayId OR @PlayId = 0)" +
                                 $"AND (a.CompanyID = @CompanyId OR @CompanyId = 0);";
            
            using (var connection = Common.ClientDatabase)
            {
                var auditorPlaybook = connection.QueryFirstOrDefault<DataModels.AuditorPlaybook>(query, new { PlayId = playId, CompanyId = companyId });
                return auditorPlaybook;
            }
        }

        public void SaveAuditorPlaybook(DataModels.AuditorPlaybook playbook)
        {
            using (var connection = Common.ClientDatabase)
            {
                connection.Insert(playbook);
            }
        }

        public int DeleteAuditorPlaybook(int playId, int companyId)
        {
            const string query = @"UPDATE AuditorPlaybook SET Status=3
                                   WHERE PlayID = @playId AND CompanyID = @CompanyId;";

            using (var connection = Common.ClientDatabase)
            {
                var result = connection.Execute(query, new { PlayId = playId, CompanyId = companyId });
                return result;
            }
        }
    }
}
