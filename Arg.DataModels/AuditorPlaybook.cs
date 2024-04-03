using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.ComponentModel.DataAnnotations;
namespace Arg.DataModels
{
    [Table("AuditorPlaybook")]
    public class AuditorPlaybook
    {
        [Dapper.Contrib.Extensions.Key]
        public int PlayID { get; set; }
        public int CompanyID { get; set; }
        public string Region { get; set; }
        public string AuditingScreenFilters { get; set; }
        public int Status { get; set; }
        public int QueryId { get; set; }
        public DateTime StatusDate { get; set; }
        public int Priority { get; set; }
        public string UserID { get; set; }
        public string Title { get; set; }

        //[AllowHtml]
        [DataType(DataType.Html)]
        public string Comments { get; set; }

        [Computed]
        public string TrimAuditingScreenFilters { get; set; }

        [Computed]
        public string FielterField { get; set; }

        [Computed]
        public string FirstName { get; set; }

        [Computed]
        public String LastName { get; set; }

        [Computed]
        public String Comment { get; set; }

        [Computed]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        [Computed]
        public string PlaybookComment { get; set; }

        [Computed]
        public string UserName { get; set; }

        [Computed]
        public string SqlQuery { get; set; }

        [Computed]
        public string QueryJson { get; set; }
    }
}
