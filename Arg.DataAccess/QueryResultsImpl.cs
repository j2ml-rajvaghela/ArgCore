using Arg.DataModels;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace Arg.DataAccess
{
    public class QueryResultsImpl
    {
        public QueryResults GetQueryResults(int queryId)
        {
            const string query = @"SELECT * FROM QueryResults
                                   WHERE QueryId=@QueryId;";

            using (var connection = Common.ClientDatabase)
            {
                var queryResults = connection.QueryFirstOrDefault<QueryResults>(query, new { @QueryId = queryId});
                return queryResults;
            }
        }

        public QueryResults SaveQueryResults(object searchOptions)
        {
            var qr = new QueryResults();
            qr.VARBINARY = new byte[] { };
            qr.QueryJson = Newtonsoft.Json.JsonConvert.SerializeObject(searchOptions);
            SaveQueryResults(qr);
            return qr;
        }

        public QueryResults SaveQueryResults(QueryResults qr)
        {
            using (var connection = Common.ClientDatabase)
            {
                connection.Insert(qr);
                return qr;
            }
        }
    }
}
