using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;
using static Arg.DataModels.Mappings;

namespace Arg.DataAccess
{
    public class MappingsImpl
    {
        public List<Mapping> GetMappingBySourceFileName(string file)
        {
            var parameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(file))
            {
                parameters.Add("@File", file, DbType.String);
            }
            const string query = @"SELECT * FROM Mappings 
                                   WHERE SourceFileName=@File;";

            using var connection = Common.ClientDatabase;
            var mappings = connection.Query<Mapping>(query, parameters).ToList();
            return mappings;
        }

        public void SaveMapping(Mapping mapping)
        {
            using var connection = Common.ClientDatabase;
            if (mapping.MappingId == 0)
            {
                connection.Insert(mapping);
            }
            else
            {
                connection.Update(mapping);
            }
        }

        public int DeleteMapping(int mappingId)
        {
            const string query = @"DELETE FROM Mappings 
                                   WHERE MappingId=@MappingId;";

            using var connection = Common.ClientDatabase;
            var result = connection.Execute(query, new { mappingId });
            return result;
        }
    }
}
