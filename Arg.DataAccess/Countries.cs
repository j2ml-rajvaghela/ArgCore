using Dapper;
using Dapper.Contrib.Extensions;

namespace Arg.DataAccess
{
    public class Countries
    {
        public class Country
        {
            public int countryid { get; set; }
            public string countrycd { get; set; }
            public string countrycd3 { get; set; }
            public string countryname { get; set; }
            public bool isactive { get; set; }
            public DateTime datecreated { get; set; }
        }

        public Country GetCountry(int countryId)
        {
            const string query = @"SELECT * FROM countries 
                                   WHERE CountryId=@CountryId;";

            using var connection = Common.Database;
            var country = connection.QueryFirstOrDefault<Country>(query, new { countryId });
            return country;
        }

        public IEnumerable<Country> GetCountries()
        {
            const string query = @"SELECT CountryId, CountryName FROM countries;";

            using var connection = Common.Database;
            var countries = connection.Query<Country>(query).ToList();
            return countries;
        }

        public void SaveCountry(Country country)
        {
            using var connection = Common.Database;
            connection.Insert(country);
        }
    }
}
