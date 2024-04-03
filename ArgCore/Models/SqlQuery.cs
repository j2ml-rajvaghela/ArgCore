using ArgCore.Helpers;

namespace ArgCore.Models
{
    public class SqlQuery
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();
        public string Query { get; set; }
        public string Message { get; set; }
        public int QueryId { get; set; }
        public bool ResultTableFormat { get; set; }
    }
}
