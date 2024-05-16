using Arg.DataModels;
using Dapper;

namespace Arg.DataAccess
{
    public class BookingsRemarksImpl
    {
        public List<BookingsRemarks> GetBookingsRemarks(string bolNo)
        {
            const string query = @"SELECT n.ReferenceNumber,n.DocType,n.Remark,n.NoteUser,n.Sequence FROM BookingsRemarks n
                                   INNER JOIN BOLHeader h ON h.BookingID=n.ReferenceNumber
                                   WHERE h.BOL#=@BolNo 
                                   ORDER BY n.Sequence;";

            using var connection = Common.ClientDatabase;
            var bookingsRemarks = connection.Query<BookingsRemarks>(query, new { bolNo }).ToList();
            return bookingsRemarks;
        }
    }
}
