using ArgCore.Helpers;

namespace ArgCore.Models
{
    public class AppActions
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        public Arg.DataModels.AppActions AppActionDetail { get; set; }

        public List<Arg.DataModels.AppActions> AppActionsList { get; set; }

        public List<Arg.DataModels.AppActions> AssignedActions { get; set; }

        public string RoleId { get; set; }
    }
}
