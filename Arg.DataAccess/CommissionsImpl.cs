using Arg.DataModels;
using CustomExtensions;
using CuttingEdge.Conditions;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace Arg.DataAccess
{
    public class CommissionsImpl
    {
        public List<Commissions> GetDistinctInvoiceNos(int companyId)
        {
            var parameters = new DynamicParameters();
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }
            using (var connection = Common.Database)
            {
                var distinctInvoiceNos = connection.Query<Commissions>("GetDistinctInvoiceNos", parameters, commandType: CommandType.StoredProcedure).ToList();
                return distinctInvoiceNos;
            }
        }

        public Commissions GetCommission(int commissionId)
        {
            var parameters = new DynamicParameters();
            if (commissionId > 0)
            {
                parameters.Add("@CommissionId", commissionId, DbType.Int32);
            }
            using (var connection = Common.Database)
            {
                var commission = connection.QueryFirstOrDefault<Commissions>("GetCommissionByCommissionId", parameters, commandType: CommandType.StoredProcedure);
                return commission;
            }
        }

        public List<Commissions> GetCommissions(SearchOptions so, string currentUserId)
        {
            var parameters = new DynamicParameters();
            
            if (so.CompanyId > 0)
            {
                parameters.Add("@CompanyId", so.CompanyId, DbType.Int32);
            }
            if (!string.IsNullOrWhiteSpace(so.Status))
            {
                parameters.Add("@Status", so.Status, DbType.String);
            }
            if (so.Regions != null)
            {
                string regionsString = string.Join(",", so.Regions);
                parameters.Add("@Regions", regionsString, DbType.String);
            }
            if (so.InvoiceNos != null)
            {
                string invoiceNosString = string.Join(",", so.InvoiceNos);
                parameters.Add("@InvoiceNos", invoiceNosString, DbType.String);
            }
            if (so.UserIds != null)
            {
                string userIdsString = string.Join(",", so.UserIds);
                parameters.Add("@UserIds", userIdsString, DbType.String);
            }
            var invoiceStartDateFormatted = so.InvoiceStartDate.ToDateTime();
            var invoiceEndDateFormatted = so.InvoiceEndDate.ToDateTime();
            if (invoiceStartDateFormatted != DateTime.MinValue && invoiceEndDateFormatted != DateTime.MinValue)
            {
                parameters.Add("@InvoiceStartDate", so.InvoiceStartDate, DbType.Date);
                parameters.Add("@InvoiceEndDate", so.InvoiceEndDate, DbType.Date);
            }
            if (Common.CanRunAction.ViewAllClientCommissions)
            {
                parameters.Add("@CurrentUserId", currentUserId, DbType.String);
            }
            else
            {
                parameters.Add("@CurrentUserId", currentUserId, DbType.String);

                if (so.Roles.Count > 0)
                {
                    string rolesString = string.Join(",", so.Roles);
                    parameters.Add("@Roles", rolesString, DbType.String);
                }
            }
           
            using (var connection = Common.Database)
            {
                var commissions = connection.Query<Commissions>("GetCommissions", parameters, commandType: CommandType.StoredProcedure).ToList();
                return commissions;
            }
        }

        public List<Commissions> GetCommissions(string invoiceNo)
        {
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(invoiceNo))
            {
                parameters.Add("@InvoiceNo", invoiceNo, DbType.String);
            }
           
            using (var connection = Common.Database)
            {
                var commissions = connection.Query<Commissions>("GetCommissionsByInvoiceNo", parameters, commandType: CommandType.StoredProcedure).ToList();
                return commissions;

            }
        }

        public int GetCommCount(int companyId, string region, string customerId, string bolNo)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Bol", bolNo, DbType.String);
            parameters.Add("@CompanyId", companyId, DbType.Int32);
            parameters.Add("@CustomerId", customerId, DbType.String);
            parameters.Add("@Region", region, DbType.String);

            using (var connection = Common.Database)
            {
                var commCount = connection.ExecuteScalar<int>("GetCommCount", parameters, commandType: CommandType.StoredProcedure);
                return commCount;
            }
        }

        public List<AspNetUsers> GetCommissionUsers(string userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId, DbType.String);
            using (var connection = Common.Database)
            {
                var commissionUsers = connection.Query<AspNetUsers>("GetCommissionUsersByUserId", parameters, commandType: CommandType.StoredProcedure).ToList();
                return commissionUsers;

            }
        }

        public void SaveCommission(Commissions commissions)
        {
            //Condition.Requires(item.AmountDueUSD).IsGreaterThan(0);
            Condition.Requires(commissions.CompanyId, "CompanyId").IsGreaterThan(0);
            Condition.Requires(commissions.CustomerId, "CustomerId").IsNotNullOrWhiteSpace();
            Condition.Requires(commissions.UserId, "UserId").IsNotNullOrWhiteSpace();
            Condition.Requires(commissions.Region, "Region").IsNotNullOrWhiteSpace();

            using (var connection = Common.Database)
            {
                if (commissions.CommissionId == 0)
                {
                    connection.Insert(commissions);
                }
                else
                {
                    connection.Update(commissions);
                }
               
            }
        }

        public int DeleteCommission(int commissionId)
        {
            const string query = "DELETE FROM Commissions WHERE CommissionId=@CommissionId;";

            using (var connection = Common.Database)
            {
                var result = connection.Execute(query, new { @CommissionId  = commissionId });
                return result;

            }
        }

        public int DeleteComm(int companyId, string region, string customerId, string bolNo)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CustomerId", customerId, DbType.String);
            parameters.Add("@Bol", bolNo, DbType.String);
            parameters.Add("@CompanyId", companyId, DbType.Int32);
            parameters.Add("@Region", region, DbType.String);
            const string query = "DELETE FROM Commissions WHERE CustomerID=@CustomerId AND BOL#=@Bol AND CompanyID=@CompanyId AND Region=@Region;";

            using (var connection = Common.Database)
            {
                var result = connection.Execute(query, parameters);
                return result;
            }
        }
    }
}
