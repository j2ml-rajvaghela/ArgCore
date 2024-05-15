using Arg.DataAccess;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;
namespace Arg.Ceva.DataAccess
{
    public class XrefAirServiceLevels
    {
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
            const string query = @"SELECT xa.*,CONCAT(xa.Description,' (',xa.SERVLEVEL,')') AS AirServiceLevels FROM XrefAirServiceLevels xa
                                   ORDER BY CONCAT(xa.Description,' (',xa.SERVLEVEL,')');";

            using (var connection = Common.ClientDatabase)
            {
                var airServiceLevels = connection.Query<XrefAirServiceLevel>(query, commandType: CommandType.Text).ToList();
                return airServiceLevels;
            }
        }

        public XrefAirServiceLevel GetSERVLEVELDescription(string code)
        {
            const string query = @"SELECT xa.*,concat(xa.Description,' (',xa.SERVLEVEL,')') AS AirServiceLevels FROM XrefAirServiceLevels xa
                                   WHERE SERVLEVEL=@SERVLEVEL;";

            using (var connection = Common.ClientDatabase)
            {
                var servLeveldescription = connection.QueryFirstOrDefault<XrefAirServiceLevel>(query, new { @SERVLEVEL = code });
                return servLeveldescription;
            }
        }
    }
}
