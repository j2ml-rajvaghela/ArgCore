using Arg.DataModels;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace Arg.DataAccess
{
    public class AppActionRoleRelsImpl
    {
        public void AssignAppAction(string roleId, int appActionId)
        {
            if (roleId == null || roleId.Length <= 0)
            {
                throw new Exception("Role not selected.");
            }
            if (appActionId <= 0)
            {
                throw new Exception("Action not selected");
            }
            var parameters = new DynamicParameters();
            parameters.Add("@RoleId", roleId, DbType.String);
            parameters.Add("@AppActionId", appActionId, DbType.Int32);

            using var connection = Common.ClientDatabase;
            var relId = connection.ExecuteScalar<int>("AssignAppAction", parameters);

            if (relId > 0)
            {
                System.Diagnostics.Trace.TraceError("Action already added.");
                return;
            }
            else
            {
                var appActionRel = new AppActionRoleRels
                {
                    AppActionId = appActionId,
                    RoleId = roleId
                };
                connection.Insert(appActionRel);
            }
        }

        public int RemoveAssignedAppAction(string roleId, int appActionId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@RoleId", roleId, DbType.String);
            parameters.Add("@AppActionId", appActionId, DbType.Int32);

            const string query = @"DELETE FROM AppActionRoleRels 
                                   WHERE roleId=@RoleId AND appActionId=@AppActionId;";

            using var connection = Common.ClientDatabase;
            var result = connection.Execute(query, parameters);
            return result;
        }
    }
}
