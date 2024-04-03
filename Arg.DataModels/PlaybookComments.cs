using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arg.DataModels
{
    [Table("PlaybookComments")]
    public class PlaybookComments
    {
        [Dapper.Contrib.Extensions.Key]
        public int CommentId { get; set; }
        public int PlayId { get; set; }

        //[AllowHtml]
        [DataType(DataType.Html)]
        public string Comment { get; set; }

        public string AddedBy { get; set; }
        public DateTime AddedOn { get; set; }

        [Computed]
        public string CollectorName { get; set; }

        [Computed]
        public string DateTimeFormatted
        {
            get
            {
                return "<span class='dateTime'><img src='/images/datetime.png' style='margin-right:3px;' /> " + AddedOn.ToLongDateString() + " <img src='/images/time.png' style='margin-left:8px;margin-right:3px;'/> " + AddedOn.ToLongTimeString() + "</span>";
            }
        }

        [Computed]
        public string ShortComment { get; set; }
    }
}
