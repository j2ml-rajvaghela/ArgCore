using Arg.DataAccess;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Arg.Ceva.DataAccess
{
    public class XrefAirCarriers
    {

        private readonly SqlConnection _connection;

        public XrefAirCarriers()
        {
            _connection = Common.ClientDatabase;
        }

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
            const string query = @"SELECT x.*,CONCAT(x.CompanyName,' (',x.IATACode,')') AS AirCompanyName 
                                   FROM XrefAirCarriers x
                                   WHERE IATACode=@IATACode;";

            return _connection.QueryFirstOrDefault<XrefAirCarrier>(query, new { @IATACode = code });
        }
    }
}
