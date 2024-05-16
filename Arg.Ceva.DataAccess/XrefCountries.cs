using Arg.DataAccess;
using Dapper;
using Dapper.Contrib.Extensions;
namespace Arg.Ceva.DataAccess
{
    public class XrefCountries
    {
        [Table("XrefCountries")]
        public class XrefCountry
        {
            public string Code { get; set; }
            public string Name { get; set; }
        }

        public XrefCountry GetCountryName(string code)
        {
            const string query = @"SELECT * FROM XrefCountries WHERE Code=@Code;";

            using var connection = Common.ClientDatabase;
            var countryName = connection.QueryFirstOrDefault<XrefCountry>(query, new { @Code = code });
            return countryName;
        }
    }
}
