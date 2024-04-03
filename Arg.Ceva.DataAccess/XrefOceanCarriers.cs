using Arg.DataAccess;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace Arg.Ceva.DataAccess
{
    public class XrefOceanCarriers
    {
        [Table("XrefOceanCarriers")]
        public class XrefOceanCarrier
        {
            public string SCAC { get; set; }
            public string CompanyName { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string ISOCountry { get; set; }
            public string Group { get; set; }
            public string Tier { get; set; }
            public DateTime? Modified { get; set; }

            [Computed]
            public string SCACCompanyName { get; set; }
        }

        public XrefOceanCarrier GetOceanCarriersCompanyName(string code)
        {
            const string query = @"SELECT x.*,CONCAT(x.CompanyName,' (',x.SCAC,')') AS SCACCompanyName FROM XrefOceanCarriers X WHERE SCAC=@SCAC;";

            using (var connection = Common.ClientDatabase)
            {
                var oceanCarriersCompanyName = connection.QueryFirstOrDefault<XrefOceanCarrier>(query, new { @SCAC = code });
                return oceanCarriersCompanyName;
            }
        }
    }
}
