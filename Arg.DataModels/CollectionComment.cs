using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Arg.DataModels
{
    [Table("[BalanceDues.CollectionComments]")]
    public class CollectionComment
    {
        [Dapper.Contrib.Extensions.Key]
        public int CollectionId { get; set; }
        public string Region { get; set; }

        //public int RegionId { get; set; }
        public string CustomerId { get; set; }

        public int CompanyId { get; set; }
        public string Bol { get; set; }
        public DateTime BolExecutionDate { get; set; }
        public string Collector { get; set; }

        //[AllowHtml]
        [DataType(DataType.Html)]
        public string Comments { get; set; }

        public DateTime DateTime { get; set; }
        public string CustomerLocationCode { get; set; }
        public string BookingId { get; set; }

        [Computed]
        public string CollectorName { get; set; }

        [Computed]
        public string UserName { get; set; }

        [Computed]
        public string DateTimeFormatted
        {
            get
            {
                return "<span class='dateTime'><img src='/images/datetime.png' style='margin-right:3px;' /> " + DateTime.ToLongDateString() + " <img src='/images/time.png' style='margin-left:8px;margin-right:3px;'/> " + DateTime.ToLongTimeString() + "</span>";
            }
        }
    }
}
