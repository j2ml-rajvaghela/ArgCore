using Dapper.Contrib.Extensions;

namespace Arg.DataModels
{
    [Table("BookingsNotes")]
    public class BookingsNotes
    {
        public string ReferenceNumber { get; set; }
        public int? Sequence { get; set; }
        public string Note { get; set; }
        public DateTime? NoteDate { get; set; }
        public string NoteTime { get; set; }
        public string NoteUser { get; set; }
        public DateTime? Uploaded { get; set; }
    }
}
