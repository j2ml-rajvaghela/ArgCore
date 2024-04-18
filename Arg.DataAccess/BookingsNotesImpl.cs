using Arg.DataModels;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            using (var connection = Common.ClientDatabase)
            {
                var bookingsNotes = connection.Query<BookingsNotes>(query, new { BolNo = bolNo }).ToList();
                return bookingsNotes;
            }
        }
    }
}
