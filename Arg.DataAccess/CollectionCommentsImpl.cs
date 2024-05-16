using Arg.DataModels;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;
namespace Arg.DataAccess
{
    public class CollectionCommentsImpl
    {

        public List<CollectionComment> GetCollectionComments(int collectionId, string q, string bolNo = "")
        {
            var parameters = new DynamicParameters();
            if (collectionId > 0)
            {
                parameters.Add("@CollectionId", collectionId, DbType.Int32);
            }
            if (!string.IsNullOrWhiteSpace(q))
            {
                parameters.Add("@Q", q, DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(bolNo))
            {
                parameters.Add("@Bol", bolNo, DbType.String);
            }

            const string query = @"SELECT c.*,Concat(u.FirstName,' ',u.LastName) AS CollectorName,u.UserName 
                                   FROM [BalanceDues.CollectionComments] c
                                   INNER JOIN AspNetUsers u ON u.Id=c.Collector
                                   WHERE (CollectionId = @CollectionId OR @CollectionId = 0)
                                   AND ((Comments LIKE '%' + @Q + '%') OR @Q = '')
                                   AND (Bol = @Bol OR @Bol = '');";

            using var connection = Common.Database;
            var collectionComments = connection.Query<CollectionComment>(query, parameters).ToList();
            return collectionComments;
        }

        public void SaveCollectionComment(CollectionComment collectionComment)
        {
            if (string.IsNullOrWhiteSpace(collectionComment.Comments))
            {
                throw new Exception("Comments can't be empty.");
            }

            using var connection = Common.Database;
            connection.Insert(collectionComment);
        }
    }
}
