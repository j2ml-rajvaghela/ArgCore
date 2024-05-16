using Arg.DataModels;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace Arg.DataAccess
{
    public class UserCompanyRelsImpl
    {
        public void AssignCompany(string userId, int companyId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("User not selected.");
            }
            if (companyId <= 0)
            {
                throw new Exception("Company not selected");
            }
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId, DbType.String);
            parameters.Add("@CompanyId", companyId, DbType.Int32);

            const string query = @"SELECT RelId FROM UserCompanyRels 
                                   WHERE UserId=@UserId AND CompanyId=@CompanyId;";

            using var connection = Common.Database;
            var relId = connection.ExecuteScalar<int?>(query, parameters);
            if (relId.HasValue && relId.Value > 0)
            {
                System.Diagnostics.Trace.TraceError("Company already added.");
            }
            else
            {
                var userComapnyRel = new UserCompanyRels
                {
                    CompanyId = companyId,
                    UserId = userId,
                };
                connection.Insert(userComapnyRel);
            }
        }

        public int RemoveAssignedComp(string userId, int companyId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId, DbType.String);
            parameters.Add("@CompanyId", companyId, DbType.Int32);

            const string query = @"DELETE FROM UserCompanyRels 
                                  WHERE UserId = @UserId AND CompanyId = @CompanyId";

            using var connection = Common.Database;
            var result = connection.Execute(query, parameters);
            return result;
        }
    }
}
