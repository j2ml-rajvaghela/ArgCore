using Arg.DataAccess;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace Arg.Ceva.DataAccess
{
    public class XrefAirServiceLevelsDetails
    {
        [Table("XrefAirServiceLevelsDetails")]
        public class XrefAirServiceLevelsDetail
        {
            public string SERVLVL { get; set; }
            public string Description { get; set; }

            [Computed]
            public string AirServiceLevelDetail { get; set; }

        }

        public List<XrefAirServiceLevelsDetail> GetXrefAirServiceLevelDetail()
        {
            const string query = @"SELECT xa.*,CONCAT(xa.Description,' (',xa.SERVLVL,')') AS AirServiceLevelDetail FROM XrefAirServiceLevelsDetails xa
                                   ORDER BY CONCAT(xa.Description,' (',xa.SERVLVL,')');";

            using (var connection = Common.ClientDatabase)
            {
                var airServiceLevelDetail = connection.Query<XrefAirServiceLevelsDetail>(query, commandType: CommandType.Text).ToList();
                return airServiceLevelDetail;
            }
        }

        public XrefAirServiceLevelsDetail GetSERVLVLDescription(string code)
        {
            const string query = @"SELECT xa.*,concat(xa.Description,' (',xa.SERVLVL,')') AS AirServiceLevelDetail FROM XrefAirServiceLevelsDetails xa
                                   WHERE xa.SERVLVL=@SERVLVL;";

            using (var connection = Common.ClientDatabase)
            {
                var servlVlDescription = connection.QueryFirstOrDefault<XrefAirServiceLevelsDetail>(query, new { SERVLVL = code });
                return servlVlDescription;
            }
        }
    }
}
