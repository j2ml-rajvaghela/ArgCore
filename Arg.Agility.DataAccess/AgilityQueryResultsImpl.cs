using Arg.Agility.DataModels;
using Arg.DataAccess;
using Dapper;
using Dapper.Contrib.Extensions;
using Newtonsoft.Json;
using System.Data;
namespace Arg.Agility.DataAccess
{
    public class AgilityQueryResultsImpl
    {
        public QueryResults GetQueryResults(int queryId)
        {
            var parameters = new DynamicParameters();

            if (queryId > 0)
            {
                parameters.Add("@QueryId", queryId, DbType.Int32);
            }   
            const string query = @"SELECT * FROM QueryResults
                                   WHERE QueryId=@QueryId;";

            using (var connection = Common.ClientDatabase)
            {
                var queryResults = connection.QueryFirstOrDefault<QueryResults>(query, parameters);
                return queryResults;
            }
        }

        public QueryResults SaveQueryResults(object searchOptions)
        {
            var qr = new QueryResults();
            qr.VARBINARY = new byte[] { };
            qr.QueryJson = JsonConvert.SerializeObject(searchOptions);
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
