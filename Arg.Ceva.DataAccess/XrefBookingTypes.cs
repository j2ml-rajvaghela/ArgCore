using Arg.DataAccess;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace Arg.Ceva.DataAccess
{
    [Table("XrefBookingType")]
    public class XrefBookingTypes
    {
        public class XrefBookingType
        {
            public string BookingType { get; set; }
            public string Description { get; set; }
        }

        public XrefBookingType GetBookingType(string bookingType)
        {
            const string query = @"SELECT * FROM XrefBookingType WHERE BookingType=@BookingType;";

            using (var connection = Common.ClientDatabase)
            {
                var bType = connection.QueryFirstOrDefault<XrefBookingType>(query, new { @BookingType = bookingType });
                return bType;
            }
        }
    }
}
