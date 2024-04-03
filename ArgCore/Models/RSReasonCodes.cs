using ArgCore.Helpers;

namespace ArgCore.Models
{
    public class RSReasonCodes
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        public List<Arg.DataModels.RSReasonCodes> ReasonCodes { get; set; }

        public Arg.DataModels.RSReasonCodes RSReasonCodeDetail { get; set; }
        public string ErrorMessage { get; set; }
    }
}
