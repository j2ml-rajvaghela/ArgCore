using Arg.DataModels;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace Arg.DataAccess
{
    public class ArgClientsImpl
    {
        public ArgClient GetArgClient(int companyId, string name)
        {
            var parameters = new DynamicParameters();

            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                parameters.Add("@Name", name, DbType.String);
            }

            using var connection = Common.Database;
            var argClient = connection.QueryFirstOrDefault<ArgClient>("GetArgClient", parameters, commandType: CommandType.StoredProcedure);
            return argClient;
        }

        public ArgClient GetClientDetailsByCustomer(string customerId)
        {
            using var connection = Common.Database;
            var clientDetailByCustomer = connection.QueryFirstOrDefault<ArgClient>("GetClientDetailByCustomer", new { customerId }, commandType: CommandType.StoredProcedure);
            return clientDetailByCustomer;
        }

        public List<ArgClient> GetDistinctArgClientNames()
        {
            using var connection = Common.Database;
            var DistinctClientsName = connection.Query<ArgClient>("GetAllDistinctArgClientsName", commandType: CommandType.StoredProcedure).ToList();
            return DistinctClientsName;
        }

        public List<ArgClient> GetArgClients()
        {
            using var connection = Common.Database;
            var argClients = connection.Query<ArgClient>("GetArgClients", commandType: CommandType.StoredProcedure).ToList();
            return argClients;
        }

        public List<ArgClient> GetArgClients(string userId)
        {
            if (userId == null && userId.Length <= 0)
            {
                System.Diagnostics.Trace.TraceError("User not selected");
            }

            using var connection = Common.Database;
            var argClientsByUserId = connection.Query<ArgClient>("GetArgClientsByUserId", new { userId }, commandType: CommandType.StoredProcedure).ToList();
            return argClientsByUserId;
        }

        public List<ArgClient> GetArgClients(int companyId, string name, string email, string location, string contact, DateTime lastModStartDate, DateTime lastModEndDate, string userId)
        {
            if (userId == null && userId.Length <= 0)
            {
                System.Diagnostics.Trace.TraceError("User not selected");
            }

            var parameters = new DynamicParameters();

            parameters.Add("@UserId", userId, DbType.String);
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }
            if (!string.IsNullOrWhiteSpace(email))
            {
                parameters.Add("@Email", email, DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                parameters.Add("@Email", name, DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(location))
            {
                parameters.Add("@Location", location, DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(contact))
            {
                parameters.Add("@Contact", contact, DbType.String);
            }
            if (lastModStartDate != DateTime.MinValue)
            {
                var lastModStartDateFormatted = lastModStartDate.ToString("yyyy-MM-dd");
                parameters.Add("@LastModStartDate", lastModStartDateFormatted);
            }
            if (lastModEndDate != DateTime.MinValue)
            {
                var lastModEndDateFormatted = lastModEndDate.Date.ToString("yyyy-MM-dd");
                parameters.Add("@LastModEndDate", lastModEndDateFormatted);
            }

            using var connection = Common.Database;
            var customerClients = connection.Query<ArgClient>("GetArgClientsByCompanyId", parameters, commandType: CommandType.StoredProcedure).ToList();
            return customerClients;
        }

        public List<ArgClient> GetBOLClients(string userId)
        {
            if (userId == null && userId.Length <= 0)
            {
                System.Diagnostics.Trace.TraceError("User not selected");
            }

            using var connection = Common.Database;
            var customerClients = connection.Query<ArgClient>("GetBOLClientsByUserId", new { userId }, commandType: CommandType.StoredProcedure).ToList();
            return customerClients;
        }

        public List<ArgClient> GetBalanceDueClients(int companyId, string userId)
        {
            if (userId == null && userId.Length <= 0)
            {
                System.Diagnostics.Trace.TraceError("User not selected");
            }

            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId, DbType.String);

            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }

            using var connection = Common.Database;
            var balanceDueClients = connection.Query<ArgClient>("GetBalanceDueClientsByUserId", parameters, commandType: CommandType.StoredProcedure).ToList();
            return balanceDueClients;
        }

        public List<ArgClient> GetResearchClients(string userId)
        {
            if (userId == null && userId.Length <= 0)
            {
                System.Diagnostics.Trace.TraceError("User not selected");
            }

            using var connection = Common.Database;
            var customerClients = connection.Query<ArgClient>("GetResearchClientsByUserId", new { userId }, commandType: CommandType.StoredProcedure).ToList();
            return customerClients;
        }

        public List<ArgClient> GetCustomerClients(string userId)
        {
            if (userId == null && userId.Length <= 0)
            {
                System.Diagnostics.Trace.TraceError("User not selected");
            }

            using var connection = Common.Database;
            var customerClients = connection.Query<ArgClient>("GetCustomerClientsByUserId", new { userId }, commandType: CommandType.StoredProcedure).ToList();
            return customerClients;

        }

        public List<ArgClient> GetActivityClients(string userId)
        {
            if (userId == null && userId.Length <= 0)
            {
                System.Diagnostics.Trace.TraceError("User not selected");
            }

            using var connection = Common.Database;
            var activityClients = connection.Query<ArgClient>("GetActivityClientsByUserId", new { userId }, commandType: CommandType.StoredProcedure).ToList();
            return activityClients;
        }

        public bool IsCurrentCompanyAssigned(string userId, int companyId)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@UserId", userId, DbType.String);
            parameters.Add("@CompanyId", companyId, DbType.Int32);

            const string query = @"SELECT * FROM UserCompanyRels 
                                   WHERE UserId=@UserId AND CompanyId=@CompanyId;";

            using var connection = Common.Database;
            var isCurrentCompanyAssigned = Convert.ToBoolean(connection.ExecuteScalar<object>(query, parameters));
            return isCurrentCompanyAssigned;
        }

        public List<ArgClient> GetRelatedUserCompanies(string userId)
        {
            using var connection = Common.Database;
            var relatedUserCompanies = connection.Query<ArgClient>("GetRelatedUserCompanies", new { userId }, commandType: CommandType.StoredProcedure).ToList();
            return relatedUserCompanies;
        }

        public void SaveArgClient(ArgClient argClient)
        {

            if (string.IsNullOrWhiteSpace(argClient.Name))
            {
                throw new Exception("Name can't be empty.");
            }

            using var connection = Common.Database;
            if (argClient.CompanyId == 0)
            {
                connection.Insert(argClient);
            }
            else
            {
                connection.Update(argClient);
            }

        }

        public int DeleteArgClient(int clientId)
        {
            const string query = @"DELETE FROM ArgClients 
                                   WHERE CompanyId=@CompanyId;";

            using var connection = Common.Database;
            var result = connection.Execute(query, new { clientId });
            return result;
        }

    }
}
