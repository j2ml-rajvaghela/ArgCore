using ArgCore.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ArgCore.Models
{
    public class PullForResearch
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        public Arg.DataModels.BOLHeader BolDetails { get; set; }
        public Arg.Agility.DataModels.BOLHeaders JobDetails { get; set; }
        public Arg.Ceva.DataAccess.BookingHeader.BookingHeaderImp BookingDetails { get; set; }

        [Required]
        public string PullReasonCode { get; set; }

        //[AllowHtml]
        [DataType(DataType.Html)]
        [Required]
        public string ResearchComments { get; set; }

        public List<Arg.DataModels.ResearchItems> ResearchItems { get; set; }

        public string BOLNo { get; set; }

        public SelectList ReasonCodes { get; set; }
        public Arg.DataModels.ShipmentJournal ShipmentDetails { get; set; }
        public bool IsShipmentDetails { get; set; }
    }
}
