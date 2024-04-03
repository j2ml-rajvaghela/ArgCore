using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arg.DataModels
{
    [Table("BOLAuditSorting")]
    public class BOLAuditSorting
    {
        [Dapper.Contrib.Extensions.Key]
        public int ID { get; set; }
        [Required]
        public int ClientId { get; set; }
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        public string Height { get; set; }
        public string LoginID { get; set; }
    }
}
