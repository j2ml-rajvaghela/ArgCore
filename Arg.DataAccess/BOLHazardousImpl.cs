using Dapper;
using System.Data;

namespace Arg.DataAccess
{
    public class BOLHazardousImpl
    {
        public List<DataModels.BOLHazardous> GetDistinctUNHazmatCodes()
        {
            const string query = @"SELECT DISTINCT UNHazmatCode 
                                   FROM BOLHazardous
                                   WHERE UNHazmatCode <> ''
                                   ORDER BY UNHazmatCode;";

            using var connection = Common.ClientDatabase;
            var distinctUNHazmatCodes = connection.Query<DataModels.BOLHazardous>(query, commandType: CommandType.Text).ToList();
            return distinctUNHazmatCodes;
        }
    }
}
