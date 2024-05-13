using Arg.DataAccess;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Arg.Ceva.DataAccess
{
    public class XrefPackagingCodes
    {
        private readonly SqlConnection _connection;

        public XrefPackagingCodes()
        {
            _connection = Common.ClientDatabase;
        }

        [Table("XrefPackagingCodes")]
        public class XrefPackagingCode
        {
            public string CODE { get; set; }
            public string DESCRIPTION { get; set; }
        }

        public XrefPackagingCode GetPackagingCode(string code)
        {
            const string query = @"SELECT * 
                                   FROM XrefPackagingCodes 
                                   WHERE CODE=@CODE;";

            return _connection.QueryFirstOrDefault<XrefPackagingCode>(query, new { @CODE = code });
        }

        public List<XrefPackagingCode> GetAllTypes()
        {
            const string query = @"SELECT * 
                                   FROM XrefPackagingCodes 
                                   ORDER BY CODE;";

            return _connection.Query<XrefPackagingCode>(query, commandType: CommandType.Text).ToList();
        }
    }
}
