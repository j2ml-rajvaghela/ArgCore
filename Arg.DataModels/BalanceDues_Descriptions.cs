using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arg.DataModels
{
    [Table("[BalanceDues.Descriptions]")]
    public class BalanceDues_Descriptions
    {
        [Key]
        public int DescriptionID { get; set; }
        public int CompanyID { get; set; }
        public string ErrorType { get; set; }
        public string Description { get; set; }

        [Computed]
        public string BDDescription { get; set; }
    }
}
