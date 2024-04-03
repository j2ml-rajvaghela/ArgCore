using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arg.DataModels
{
    [Dapper.Contrib.Extensions.Table("ContainerEventHistory")]
    public class ContainerEventHistory
    {
        [Key]
        public string ContainerID { get; set; }
        public string Container10Digit { get; set; }
        public string CheckDigit { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string EventType { get; set; }
        public string EventDate { get; set; }
        public string EventTime { get; set; }
        public DateTime? EventDateTime { get; set; }
        public string LoadStatus { get; set; }
        public string FromLocationCode { get; set; }
        public string ToLocationCode { get; set; }
        public decimal? Weight { get; set; }
        public string Seal { get; set; }
        public string BookingID { get; set; }

        [Column("Bol#")]
        public string BOLNo { get; set; }

        public DateTime? Uploaded { get; set; }

        [Computed]
        public string Container { get; set; }

        [Computed]
        public string FromCode { get; set; }

        [Computed]
        public string ToCode { get; set; }
    }
}
