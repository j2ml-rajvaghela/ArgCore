using Arg.DataModels;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;
using System.Text.RegularExpressions;

namespace Arg.DataAccess
{
    public class SettingGroupsImpl
    {
        public List<SettingGroups> GetSettingGroups(string q)
        {
            var parameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(q))
            {
                parameters.Add("@Q", q, DbType.String);
            }
            using (var connection = Common.Database)
            {
                var settingGroups = connection.Query<SettingGroups>("GetSettingGroups", parameters, commandType: CommandType.StoredProcedure).ToList();
                return settingGroups;
            }
        }

        public SettingGroups GetSettingGroup(int groupId)
        {
            var parameters = new DynamicParameters();
            if (groupId > 0)
            {
                parameters.Add("@GroupId", groupId, DbType.String);
            }
            using (var connection = Common.Database)
            {
                var settingGroup = connection.QueryFirstOrDefault<SettingGroups>("GetSettingGroup", parameters, commandType: CommandType.StoredProcedure);
                return settingGroup;
            }
        }

        public void SaveSettingGroup(SettingGroups settingGroup)
        {
            if (string.IsNullOrWhiteSpace(settingGroup.Name))
            {
                throw new Exception("Name can't be empty.");
            }
            using (var connection = Common.Database)
            {
                if (settingGroup.GroupId == 0)
                {
                    connection.Insert(settingGroup);
                }
                else
                {
                    connection.Update(settingGroup);
                }
            }
           
        }

        public int DeleteSettingGroup(int groupId)
        {
            const string query = "DELETE FROM SettingGroups WHERE GroupId=@GroupId;";
            using (var connection = Common.Database)
            {
                var result = connection.Execute(query, new { @GroupId = groupId });
                return result;
            }
        }

    }
}
