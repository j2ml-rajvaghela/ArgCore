using Arg.DataAccess;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;
using System.Data;
namespace Arg.Ceva.DataAccess
{
    public class XrefCountries
    {
        private readonly SqlConnection _connection;

        public XrefCountries()
        {
            _connection = Common.ClientDatabase;
        }

        [Table("XrefCountries")]
        public class XrefCountry
        {
            public string Code { get; set; }
            public string Name { get; set; }
        }

        public XrefCountry GetCountryName(string code)
        {
            const string query = @"SELECT * 
                                   FROM XrefCountries 
                                   WHERE Code=@Code;";

            return _connection.QueryFirstOrDefault<XrefCountry>(query, new { @Code = code });
        }
    }
}
