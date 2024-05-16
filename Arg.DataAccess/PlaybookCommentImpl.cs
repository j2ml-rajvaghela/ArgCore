using Dapper;
using Dapper.Contrib.Extensions;

namespace Arg.DataAccess
{
    public class PlaybookCommentImpl
    {
        private string _dbName = Common.DefaultDbName;

        public List<DataModels.PlaybookComments> GetPlaybookComments(int playId)
        {
            string query = $"SELECT p.*,CONCAT(u.FirstName,' ',u.LastName) AS CollectorName,LEFT(p.Comment,100) AS ShortComment FROM PlaybookComments p" +
                           $"INNER JOIN {_dbName}.dbo.AspNetUsers u ON u.Id = p.AddedBy" +
                           $"WHERE (p.PlayId=@PlayId OR @PlayId = 0);";

            using var connection = Common.ClientDatabase;
            var playBookComments = connection.Query<DataModels.PlaybookComments>(query, new { playId }).ToList();
            return playBookComments;
        }


        public void SavePlaybookComment(DataModels.PlaybookComments playbook)
        {
            using var connection = Common.ClientDatabase;
            connection.Insert(playbook);
        }
    }
}
