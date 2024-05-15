using Arg.DataAccess;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Arg.Ceva.DataAccess
{
    public class XrefOceanCarriers
    {
        private readonly SqlConnection _connection;

        public XrefOceanCarriers()
        {
            _connection = Common.ClientDatabase;
        }

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
            const string query = @"SELECT x.*,CONCAT(x.CompanyName,' (',x.SCAC,')') AS SCACCompanyName 
                                   FROM XrefOceanCarriers X 
                                   WHERE SCAC=@SCAC;";

            return _connection.QueryFirstOrDefault<XrefOceanCarrier>(query, new { @SCAC = code });
        }
    }
}
