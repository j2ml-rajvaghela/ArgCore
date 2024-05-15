using Arg.DataAccess;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;
using System.Data;
namespace Arg.Ceva.DataAccess
{
    public class XrefAirServiceLevels
    {
        private readonly SqlConnection _connection;

        public XrefAirServiceLevels()
        {
            _connection = Common.ClientDatabase;
        }

        [Table("XrefAirServiceLevels")]
        public class XrefAirServiceLevel
        {
            public string SERVLEVEL { get; set; }
            public string Description { get; set; }

            [Computed]
            public string AirServiceLevels { get; set; }
        }

        public List<XrefAirServiceLevel> GetXrefAirServiceLevels()
        {
            const string query = @"SELECT xa.*,CONCAT(xa.Description,' (',xa.SERVLEVEL,')') AS AirServiceLevels 
                                   FROM XrefAirServiceLevels xa
                                   ORDER BY CONCAT(xa.Description,' (',xa.SERVLEVEL,')');";

            return _connection.Query<XrefAirServiceLevel>(query, commandType: CommandType.Text).ToList();
        }

        public XrefAirServiceLevel GetSERVLEVELDescription(string code)
        {
            const string query = @"SELECT xa.*,concat(xa.Description,' (',xa.SERVLEVEL,')') AS AirServiceLevels 
                                   FROM XrefAirServiceLevels xa
                                   WHERE SERVLEVEL=@SERVLEVEL;";

            return _connection.QueryFirstOrDefault<XrefAirServiceLevel>(query, new { @SERVLEVEL = code });
        }
    }
}
