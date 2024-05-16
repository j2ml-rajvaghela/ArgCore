using Arg.DataModels;
using Dapper;

namespace Arg.DataAccess
{
    public class BookingsNotesImpl
    {
        public List<BookingsNotes> GetBookingsNotes(string bolNo)
        {
            const string query = @"SELECT n.ReferenceNumber,n.Note,n.NoteDate,n.Sequence FROM BookingsNotes n
                                   INNER JOIN BOLHeader h ON h.BookingID=n.ReferenceNumber
                                   WHERE h.BOL#=@BolNo
                                   ORDER BY n.Sequence;";

            using var connection = Common.ClientDatabase;
            var bookingsNotes = connection.Query<BookingsNotes>(query, new {  bolNo }).ToList();
            return bookingsNotes;
        }
    }
}
