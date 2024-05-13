using Arg.Agility.DataModels;
using Arg.DataAccess;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
namespace Arg.Agility.DataAccess
{
    public class AgilityQueryResultsImpl
    {
        private readonly SqlConnection _connection;
        public AgilityQueryResultsImpl()
        {
            _connection = Common.ClientDatabase;
        }
        public QueryResults GetQueryResults(int queryId)
        {
            var parameters = new DynamicParameters();

            if (queryId > 0)
            {
                parameters.Add("@QueryId", queryId, DbType.Int32);
            }
            const string query = @"SELECT * 
                                   FROM QueryResults
                                   WHERE QueryId=@QueryId;";

            return _connection.QueryFirstOrDefault<QueryResults>(query, parameters);
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
            _connection.Insert(qr);
            return qr;
        }
    }
}
