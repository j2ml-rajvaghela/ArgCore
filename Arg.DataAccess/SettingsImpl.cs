using Arg.DataModels;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace Arg.DataAccess
{
    public class SettingsImpl
    {
        public List<Settings> GetSettings(int groupId)
        {
            var parameters = new DynamicParameters();
            if (groupId > 0)
            {
                parameters.Add("@GroupId", groupId, DbType.Int32);
            }

            using var connection = Common.Database;
            var settings = connection.Query<Settings>("GetSettings", parameters, commandType: CommandType.StoredProcedure).ToList();
            return settings;
        }

        public Settings GetSetting(int settingId)
        {
            var parameters = new DynamicParameters();
            if (settingId > 0)
            {
                parameters.Add("@SettingId", settingId, DbType.Int32);
            }

            using var connection = Common.Database;
            var setting = connection.QueryFirstOrDefault<Settings>("GetSetting", parameters, commandType: CommandType.StoredProcedure);
            return setting;
        }

        public string GetSettingValue(string key)
        {
            var parameters = new DynamicParameters();
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new Exception("No key provided");
            }
            parameters.Add("@Key", key, DbType.String);

            using var connection = Common.Database;
            var setting = connection.QueryFirstOrDefault<Settings>("GetSettingValue", parameters, commandType: CommandType.StoredProcedure);
            return setting.Value;
        }
        public void SaveSetting(Settings setting)
        {
            if (string.IsNullOrWhiteSpace(setting.Label))
            {
                throw new Exception("Label can't be empty.");
            }

            using var connection = Common.Database;
            if (setting.SettingId == 0)
            {
                connection.Insert(setting);
            }
            else
            {
                connection.Update(setting);
            }

        }

        public int DeleteSetting(int settingId)
        {
            const string query = @"DELETE FROM Settings 
                                   WHERE SettingId=@SettingId";

            using var connection = Common.Database;
            var result = connection.Execute(query, new { settingId });
            return result;
        }

    }
}
