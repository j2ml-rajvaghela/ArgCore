using Dapper;
using System.Data;

namespace Arg.DataAccess
{
    public class AspNetUserRolesImpl
    {
        public bool UserRoleExists(string userId, string roleId)
        {
            //var parameters = new DynamicParameters();
            //parameters.Add("@UserId", userId, DbType.String);
            //parameters.Add("@RoleId", roleId, DbType.String);
            using var connection = Common.Database;
            var isUserRoleExists = connection.QueryFirstOrDefault<bool>("UserRoleExists", new { userId, roleId }, commandType: CommandType.StoredProcedure);
            return isUserRoleExists;
        }

        public int AssignAspNetUserRoles(string userId, string roleId)
        {
            //var parameters = new DynamicParameters();
            //parameters.Add("@UserId", userId, DbType.String);
            //parameters.Add("@RoleId", roleId, DbType.String);
            const string query = @"
                        DELETE FROM [dbo].[AspNetUserRoles] WHERE UserId = @UserId;
                        INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (@UserId, @RoleId)";

            using var connection = Common.Database;
            var result = connection.Execute(query, new { userId, roleId });
            return result;
        }
    }
}
