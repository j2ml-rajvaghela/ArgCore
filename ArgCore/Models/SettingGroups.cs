using ArgCore.Helpers;

namespace ArgCore.Models
{
    public class SettingGroups
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        public List<Arg.DataModels.SettingGroups> SettingGroupsList { get; set; }

        public Arg.DataModels.SettingGroups SettingGroupDetail { get; set; }

        public List<Arg.DataModels.Settings> SettingsList { get; set; }
    }
}
