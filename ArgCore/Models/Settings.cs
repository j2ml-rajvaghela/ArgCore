using ArgCore.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArgCore.Models
{
    public class Settings
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        public List<Arg.DataModels.Settings> SettingsList { get; set; }

        public Arg.DataModels.Settings SettingDetail { get; set; }

        public SelectList SettingGroups { get; set; }
    }
}
