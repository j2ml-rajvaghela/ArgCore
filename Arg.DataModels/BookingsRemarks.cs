using Dapper.Contrib.Extensions;

namespace Arg.DataModels
{
    [Table("BookingsRemarks")]
    public class BookingsRemarks
    {
        public string DocType { get; set; }
        public string ReferenceNumber { get; set; }
        public int? Sequence { get; set; }
        public string Remark { get; set; }
        public DateTime? RemarkDate { get; set; }
        public string RemarkTime { get; set; }
        public string NoteUser { get; set; }
        public DateTime? Uploaded { get; set; }
    }
}
