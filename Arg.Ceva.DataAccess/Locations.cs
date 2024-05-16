using Arg.DataAccess;
using Dapper;
using Dapper.Contrib.Extensions;
namespace Arg.Ceva.DataAccess
{
    public class Locations
    {
        [Table("Locations")]
        public class Location
        {
            [Key]
            public string LocationID { get; set; }
            public string Type { get; set; }
            public string Region { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string Address2 { get; set; }
            public string Address3 { get; set; }
            public DateTime? AddedOn { get; set; }

            [Computed]
            public string LocationName { get; set; }

            [Computed]
            public string PODDetail { get; set; }

            [Computed]
            public string POLDetail { get; set; }
        }

        public Location GetLocation(string code)
        {
            const string query = @"SELECT l.*,CONCAT(l.Name,' (',l.LocationID,')') AS LocationName FROM Locations l
                                   WHERE LocationID=@LocationID;";

            using var connection = Common.ClientDatabase;
            var location = connection.QueryFirstOrDefault<Location>(query, new { @LocationID = code });
            return location;
        }

        public Location GetPOL(string code)
        {
            const string query = @"SELECT l.*,CONCAT(l.Name,' (',l.LocationID,')',l.Address3) AS POLDetail FROM Locations l
                                   WHERE l.LocationID=@LocationID;";

            using var connection = Common.ClientDatabase;
            var pol = connection.QueryFirstOrDefault<Location>(query, new { LocationID = code });
            return pol;
        }

        public Location GetPOD(string code)
        {
            const string query = @"SELECT l.*,CONCAT(l.Name,' (',l.LocationID,')',l.Address3) AS PODDetail FROM Locations l
                                   WHERE l.LocationID=@LocationID;";

            using var connection = Common.ClientDatabase;
            var pod = connection.QueryFirstOrDefault<Location>(query, new { LocationID = code });
            return pod;
        }
    }
}
