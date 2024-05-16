using Arg.DataModels;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace Arg.DataAccess
{
    public class RoleMenuRelsImpl
    {
        public void AssignMenuItem(string roleId, int itemId)
        {
            if (roleId == null || roleId.Length <= 0)
            {
                throw new Exception("Role not selected.");
            }
            if (itemId <= 0)
            {
                throw new Exception("Item not selected");
            }
            var parameters = new DynamicParameters();
            parameters.Add("@RoleId", roleId, DbType.String);
            parameters.Add("@ItemId", itemId, DbType.Int32);

            using var connection = Common.Database;
            var relId = connection.ExecuteScalar<int>("AssignMenuItem", parameters);

            if (relId > 0)
            {
                System.Diagnostics.Trace.TraceError("MenuItem already added.");
                return;
            }
            else
            {
                var appActionRel = new RoleMenuRels
                {
                    ItemId = itemId,
                    RoleId = roleId,
                    AddedOn = DateTime.Now,
                    LastModOn = DateTime.Now
                };
                connection.Insert(appActionRel);
            }
        }

        public int RemoveAssignedMenuItem(string roleId, int itemId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@RoleId", roleId, DbType.String);
            parameters.Add("@ItemId", itemId, DbType.Int32);

            const string query = @"DELETE FROM RoleMenuRels 
                                   WHERE RoleId=@RoleId AND ItemId=@ItemId;";

            using var connection = Common.Database;
            var result = connection.Execute(query, parameters);
            return result;
        }
    }
}
