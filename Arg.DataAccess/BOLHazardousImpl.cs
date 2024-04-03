using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            using (var connection = Common.ClientDatabase)
            {
                var distinctUNHazmatCodes = connection.Query<DataModels.BOLHazardous>(query, commandType: CommandType.Text).ToList();
                return distinctUNHazmatCodes;
            }
        }
    }
}
