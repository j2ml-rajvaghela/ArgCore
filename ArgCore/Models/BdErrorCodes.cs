using Arg.DataModels;
using ArgCore.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArgCore.Models
{
    public class BdErrorCodes
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();
        public SearchOptions SearchOptions { get; set; }

        public List<Arg.DataModels.BdErrorCodes> ErrorCodes { get; set; }

        public SelectList Companies { get; set; }

        public Arg.DataModels.BdErrorCodes ErrorCodeDetail { get; set; }
        public string ErrorMessage { get; set; }
    }
}
