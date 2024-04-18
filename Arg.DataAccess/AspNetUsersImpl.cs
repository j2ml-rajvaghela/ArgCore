using Arg.DataModels;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace Arg.DataAccess
{
    public class AspNetUsersImpl
    {
        public List<AspNetUsers> GetAspNetUsers(SearchOptions so)
        {
            using (var connection = Common.Database)
            {
                var aspNetUsers = connection.Query<AspNetUsers>("GetAspNetUsers", so, commandType: CommandType.StoredProcedure).ToList();
                return aspNetUsers;
            }
        }

        public List<AspNetUsers> GetAspNetUsers(int companyId)
        {
            var parameters = new DynamicParameters();
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }
            using (var connection = Common.Database)
            {
                var aspNetUsers = connection.Query<AspNetUsers>("GetAspNetUsersByCompanyId", parameters, commandType: CommandType.StoredProcedure).ToList();
                return aspNetUsers;
            }
        }

        public AspNetUsers GetAspNetUser(string id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id, DbType.Int32);
            using (var connection = Common.Database)
            {
                var aspNetUser = connection.QueryFirstOrDefault<AspNetUsers>("GetAspNetUser", parameters, commandType: CommandType.StoredProcedure);
                return aspNetUser;
            }
        }

        public List<AspNetUsers> GetAspNetUsers(bool addAllUserOption)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@AddAllUserOption", addAllUserOption, DbType.Boolean);
            using (var connection = Common.Database)
            {            
                var aspNetUsers = connection.Query<AspNetUsers>("GetAspNetUsers", parameters, commandType: CommandType.StoredProcedure).ToList();
                return aspNetUsers;
            }
        }

        public List<AspNetUsers> GetActivityUsers(string userId, bool argManager)
        {
            var parameters = new DynamicParameters();
            if (argManager && !string.IsNullOrWhiteSpace(userId))
            {
                parameters.Add("@UserId", userId, DbType.Int32);
                parameters.Add("@ArgManager", argManager, DbType.Boolean);
            }
            using (var connection = Common.Database)
            { 
                var activityUsers = connection.Query<AspNetUsers>("GetActivityUsers", parameters, commandType: CommandType.StoredProcedure).ToList();
                return activityUsers;
            }
        }

        public List<AspNetUsers> GetAspNetUsersWithRoles(SearchOptions so, string q)
        {
            var parameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(q))
            {
                parameters.Add("@Q", q, DbType.String);
            }
            if (so.Roles != null)
            {
                string rolesString = string.Join(",", so.Roles);
                parameters.Add("@Roles", rolesString, DbType.String);
            }
            if (so.CompanyId > 0)
            {
                parameters.Add("@CompanyId", so.CompanyId, DbType.Int32);
            }
            if (!string.IsNullOrWhiteSpace(so.Name))
            {
                parameters.Add("@Name", so.Name, DbType.String);
            }
            using (var connection = Common.Database)
            {
                var aspNetUsersWithRoles = connection.Query<AspNetUsers>("GetAspNetUsersWithRoles", parameters, commandType: CommandType.StoredProcedure).ToList();
                return aspNetUsersWithRoles;
            }
        }

        public void SaveAspNetUser(AspNetUsers aspNetUser)
        {
            using (var connection = Common.Database)
            {
                if (string.IsNullOrEmpty(aspNetUser.Id))
                {
                    connection.Insert(aspNetUser);
                }
                else
                {
                    connection.Update(aspNetUser);
                }
            }
        }

        public int DeleteAspNetUser(string userId)
        {
            const string query = "DELETE FROM AspNetUsers WHERE Id=@UserId;";
            using (var connection = Common.Database)
            { 
                var result = connection.Execute(query, new { UserId = userId });
                return result;
            }
        }
    }
}
