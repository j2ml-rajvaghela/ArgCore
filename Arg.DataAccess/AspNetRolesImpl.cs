using Arg.DataModels;
using Dapper;
using System.Data;

namespace Arg.DataAccess
{
    public class AspNetRolesImpl
    {
        public AspNetRoles GetAspNetRoleByName(string roleName)
        {
            var parameters = new DynamicParameters();
            if (roleName != null)
            {
                parameters.Add("@RoleName", roleName, DbType.String);
            }

            using var connection = Common.Database;
            var aspNetRoleByName = connection.QueryFirstOrDefault<AspNetRoles>("GetAspNetRoleByName", parameters, commandType: CommandType.StoredProcedure);
            return aspNetRoleByName;
        }

        public List<AspNetRoles> GetRolesWithUserCount(string q)
        {
            var parameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(q))
            {
                parameters.Add("@q", q, DbType.String);
            }

            using var connection = Common.Database;
            var rolesWithUsersCount = connection.Query<AspNetRoles>("GetRolesWithUserCount", parameters, commandType: CommandType.StoredProcedure).ToList();
            return rolesWithUsersCount;
        }

        public AspNetUserRoles GetAspNetUserRole(string userId)
        {
            using var connection = Common.Database;
            var userRoleByUserId = connection.QueryFirstOrDefault<AspNetUserRoles>("GetAspNetUserRoleByUserId", new { userId }, commandType: CommandType.StoredProcedure);
            return userRoleByUserId;
        }

        public List<AspNetRoles> GetDistinctUserRoles()
        {
            using var connection = Common.Database;
            var distinctUserRoles = connection.Query<AspNetRoles>("GetDistinctUserRoles", commandType: CommandType.StoredProcedure).ToList();
            return distinctUserRoles;
        }

        public List<AspNetRoles> GetActivityUserRoles()
        {
            using var connection = Common.Database;
            var activityUserRoles = connection.Query<AspNetRoles>("GetActivityUserRoles", commandType: CommandType.StoredProcedure).ToList();
            return activityUserRoles;
        }

        public int DeleteUserRole(string roleId)
        {
            const string query = @"DELETE FROM AspNetRoles 
                                   WHERE RoleId=@RoleId;";

            using var connection = Common.Database;
            var result = connection.Execute(query, new { roleId });
            return result;
        }
    }
}
