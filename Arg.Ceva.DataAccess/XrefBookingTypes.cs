using Arg.DataAccess;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Arg.Ceva.DataAccess
{
    [Table("XrefBookingType")]
    public class XrefBookingTypes
    {
        private readonly SqlConnection _connection;

        public XrefBookingTypes()
        {
            _connection = Common.ClientDatabase;
        }

        public class XrefBookingType
        {
            public string BookingType { get; set; }
            public string Description { get; set; }
        }

        public XrefBookingType GetBookingType(string bookingType)
        {
            const string query = @"SELECT * 
                                   FROM XrefBookingType 
                                   WHERE BookingType=@BookingType;";

            return _connection.QueryFirstOrDefault<XrefBookingType>(query, new { @BookingType = bookingType });
        }
    }
}
