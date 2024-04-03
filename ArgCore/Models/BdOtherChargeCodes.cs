using Arg.DataModels;
using ArgCore.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArgCore.Models
{
    public class BdOtherChargeCodes
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();
        public List<Arg.DataModels.BdOtherChargeCodes> ChargeCodes { get; set; }
        public SearchOptions SearchOptions { get; set; }
        public SelectList Companies { get; set; }
        public SelectList ChargeCodesList { get; set; }
        public Arg.DataModels.BdOtherChargeCodes ChargeCodeDetail { get; set; }
        public string ErrorMessage { get; set; }
    }
}
