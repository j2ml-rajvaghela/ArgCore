using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arg.DataAccess
{
    public class BOLReferenceImpl
    {
        public List<DataModels.BOLReference> GetDistinctBOLReferences()
        {
            const string query = @"SELECT DISTINCT ReferenceType 
                                   FROM BOLReference
                                   WHERE BOLReference <> '';";

            using (var connection = Common.ClientDatabase)
            {
                var distinctBOLReferences = connection.Query<DataModels.BOLReference>(query, commandType: CommandType.Text).ToList();
                return distinctBOLReferences;
            }
        }

        public List<DataModels.BOLReference> GetBOLReferences(string bolNo)
        {
           const string query = @"SELECT CONCAT(ReferenceType,' ',Reference) AS Reference
                                  FROM BOLReference
                                  WHERE BOL#=@BolNo
                                  ORDER BY ReferenceType;";

            using (var connection = Common.ClientDatabase)
            {
                var bOLReferences = connection.Query<DataModels.BOLReference>(query, new { @BolNo = bolNo }).ToList();
                return bOLReferences;
            }
        }

        public DataModels.BOLReference GetBOLReference(string bolNo, string refType)
        {
            const string query = @"SELECT * FROM BOLReference
                                  WHERE BOL#=@BolNo AND ReferenceType=@RefType AND ReferenceType <> '';";

            using (var connection = Common.ClientDatabase)
            {
                var bOLReference = connection.QueryFirstOrDefault<DataModels.BOLReference>(query, new { @BolNo = bolNo, @RefType = refType });
                return bOLReference;
            }
        }
    }
}
