using Arg.DataAccess;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace Arg.Ceva.DataAccess
{
    public class XrefAirCarriers
    {
        [Table("XrefAirCarriers")]
        public class XrefAirCarrier
        {
            public string IATACode { get; set; }
            public string Prefix { get; set; }
            public string CompanyName { get; set; }

            [Computed]
            public string AirCompanyName { get; set; }
        }

        public XrefAirCarrier GetIATACode(string code)
        {
            const string query = @"SELECT x.*,CONCAT(x.CompanyName,' (',x.IATACode,')') AS AirCompanyName FROM XrefAirCarriers x
                                   WHERE IATACode=@IATACode;";

            using (var connection = Common.ClientDatabase)
            {
                var iataCode = connection.QueryFirstOrDefault<XrefAirCarrier>(query, new { @IATACode = code });
                return iataCode;
            }
        }
    }
}
