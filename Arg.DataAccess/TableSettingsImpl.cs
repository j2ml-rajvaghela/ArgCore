using Arg.DataModels;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;
using static Arg.DataModels.Mappings;

namespace Arg.DataAccess
{
    public class TableSettingsImpl
    {
        public List<TableSettings> GetTableSettingByTruncateTable(bool getTruncateTables = false)
        {
            string query = "SELECT * FROM TableSettings ";
            if (getTruncateTables)
            {
                query += "WHERE TruncateTable = 1;";
            }
            using (var connection = Common.ClientDatabase)
            {
                var tableSetting = connection.Query<TableSettings>(query).ToList();
                return tableSetting;
            }
        }


        public TableSettings GetTableSettingById(int tableSettingId)
        {
            const string query = "SELECT * FROM TableSettings WHERE (@TableSettId = 0 OR TableSettId = @TableSettId);";
            using (var connection = Common.ClientDatabase)
            {
                var tableSetting = connection.QueryFirstOrDefault<TableSettings>(query, new { @TableSettId = tableSettingId });
                return tableSetting;
            }
        }

        public void SaveTableSetting(TableSettings tableSettings)
        {
            using (var connection = Common.ClientDatabase)
            {
                if (tableSettings.TableSettId == 0)
                {
                    connection.Insert(tableSettings);
                }
                else
                {
                    connection.Update(tableSettings);
                }
                
            }
        }

        public int DeleteTableSetting(int mappingId)
        {
            const string query = "DELETE FROM TableSettings WHERE TableSettId=@MappingId;";
            using (var connection = Common.ClientDatabase)
            {
                var result = connection.Execute(query, new { MappingId = mappingId });
                return result;
            }
        }
    }
}
