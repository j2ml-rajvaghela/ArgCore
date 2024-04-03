using Arg.DataAccess;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace Arg.Ceva.DataAccess
{
    public class XrefPackagingCodes
    {
        [Table("XrefPackagingCodes")]
        public class XrefPackagingCode
        {
            public string CODE { get; set; }
            public string DESCRIPTION { get; set; }
        }

        public XrefPackagingCode GetPackagingCode(string code)
        {
            const string query = @"SELECT * FROM XrefPackagingCodes WHERE CODE=@CODE;";

            using (var connection = Common.ClientDatabase)
            {
                var packagingCode = connection.QueryFirstOrDefault<XrefPackagingCode>(query, new { @CODE = code });
                return packagingCode;
            }
        }

        public List<XrefPackagingCode> GetAllTypes()
        {
            const string query = @"SELECT * FROM XrefPackagingCodes ORDER BY CODE;";

            using (var connection = Common.ClientDatabase)
            {
                var allTypes = connection.Query<XrefPackagingCode>(query, commandType: CommandType.Text).ToList();
                return allTypes;
            }
        }
    }   
}
