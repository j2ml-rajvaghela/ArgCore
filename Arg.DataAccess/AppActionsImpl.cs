using Arg.DataModels;
using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arg.DataAccess
{
    public class AppActionsImpl
    {
        public List<AppActions> GetAppActions()
        {
            using (var connection = Common.Database)
            {
                var appActions = connection.Query<AppActions>("GetAllAppActions", commandType: CommandType.StoredProcedure).ToList();
                return appActions;
            }
        }

        public AppActions GetAppAction(int appActionId, string actionName)
        {
            var parameters = new DynamicParameters();
            if (appActionId > 0)
            {
                parameters.Add("@AppActionId", appActionId, DbType.Int32);
            }
            if (!string.IsNullOrWhiteSpace(actionName))
            {
                parameters.Add("@ActionName", actionName, DbType.String);
            }
            using (var connection = Common.Database)
            {
                var appAction = connection.QueryFirstOrDefault<AppActions>("GetAppAction", parameters, commandType: CommandType.StoredProcedure);
                return appAction;
            }
        }

        public List<AppActions> GetActionRoleRels(string roleId)
        {
            using (var connection = Common.Database)
            {
                var parameters = new DynamicParameters();
                parameters.Add("@RoleId", roleId, DbType.String);

                var actionRoleRels = connection.Query<AppActions>("GetActionRoleRelsByRoleId", parameters, commandType: CommandType.StoredProcedure).ToList();
                return actionRoleRels;
            }
        }

        public List<AppActions> GetActionsAssignedToCurrentRole(string roleId, string actionName)
        {
            using (var connection = Common.Database)
            {
                var parameters = new DynamicParameters();
                parameters.Add("@RoleId", roleId, DbType.String);
                //parameters.Add("@ActionName", actionName, DbType.String);

                var actionsAssignedToCurrentRoles = connection.Query<AppActions>("GetActionsAssignedToCurrentRole", parameters, commandType: CommandType.StoredProcedure).ToList();
                return actionsAssignedToCurrentRoles;
            }
        }

        public string GetRoleAssignedToAction(string name)
        {
            using (var connection = Common.Database)
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Name", name, DbType.String);

                var roleAssigned = connection.QueryFirstOrDefault<string>("GetRoleAssignedToAction", parameters, commandType: CommandType.StoredProcedure);
                return roleAssigned;
            }
        }

        public void SaveAppAction(AppActions appActions)
        {
            if (string.IsNullOrWhiteSpace(appActions.ActionName))
            {
                throw new Exception("Name can't be empty.");
            }
            using (var connection = Common.Database)
            {
               if (appActions.AppActionId == 0)
               {
                    connection.Insert(appActions);
               }
               else
               {
                    connection.Update(appActions);
               }
            }
        }

        public int DeleteAppAction(int appActionId)
        {
            const string query = @"DELETE FROM AppActions WHERE AppActionId=@AppActionId;";
            using (var connection = Common.Database)
            {
                var result = connection.Execute(query, new { @AppActionId = appActionId });
                return result;
            }
        }
    }
}
