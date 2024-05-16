using Dapper;
using System.Data;

namespace Arg.DataAccess
{
    public class BOLContainersImpl
    {
        public List<DataModels.BOLContainers> GetDistinctSize()
        {
            const string query = @"SELECT DISTINCT Size 
                                   FROM BOLContainers
                                   WHERE Size <> ''
                                   ORDER BY Size;";

            using var connection = Common.ClientDatabase;
            var distinctSize = connection.Query<DataModels.BOLContainers>(query, commandType: CommandType.Text).ToList();
            return distinctSize;
        }

        public List<DataModels.BOLContainers> GetDistinctType()
        {
            const string query = @"SELECT DISTINCT Type 
                                   FROM BOLContainers
                                   WHERE Type <> ''
                                   ORDER BY Type;";

            using var connection = Common.ClientDatabase;
            var distinctType = connection.Query<DataModels.BOLContainers>(query, commandType: CommandType.Text).ToList();
            return distinctType;
        }
    }
}
