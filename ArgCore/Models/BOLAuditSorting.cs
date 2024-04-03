using Dapper.Contrib.Extensions;

namespace ArgCore.Models
{
    public class BOLAuditSorting
    {
        public int ID { get; set; }
        public int ClientId { get; set; }
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        public string Height { get; set; }
        public string LoginID { get; set; }
        public int CompanyID { get; set; }
        public List<Arg.DataModels.BOLAuditSorting> ConvertedJson { get; set; }

        [Computed]
        public string DBName { get; set; }
    }
}
