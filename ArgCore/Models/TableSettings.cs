using ArgCore.Helpers;

namespace ArgCore.Models
{
    public class TableSettings
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        public Arg.DataModels.TableSettings TableSettingsDetail { get; set; }

        public Arg.DataModels.TableSettings TableSettingDetail { get; set; }

        public List<Arg.DataModels.TableSettings> TableSettingsList { get; set; }
    }
}
