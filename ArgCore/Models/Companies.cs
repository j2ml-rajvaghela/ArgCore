using ArgCore.Helpers;

namespace ArgCore.Models
{
    public class Companies
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        public List<Arg.DataModels.ArgClient> CompaniesList { get; set; }

        public List<Arg.DataModels.ArgClient> AssignedCompanies { get; set; }

        public string UserId { get; set; }
    }
}
