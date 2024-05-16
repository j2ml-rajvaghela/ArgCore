using Arg.DataModels;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace Arg.DataAccess
{
    public class CustomersImpl
    {
        public List<Customers> GetCustomers()
        {
            using var connection = Common.Database;
            var customers = connection.Query<Customers>("GetCustomers", commandType: CommandType.StoredProcedure).ToList();
            return customers;
        }

        public List<Customers> GetBalanceDueCustomers(int companyId)
        {
            var parameters = new DynamicParameters();
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }

            using var connection = Common.Database;
            var customers = connection.Query<Customers>("GetBalanceDueCustomersByCompanyId", parameters, commandType: CommandType.StoredProcedure).ToList();
            return customers;
        }

        public Customers GetCustomer(int bdCustomerId, int companyId, string customerId = "")
        {
            var parameters = new DynamicParameters();
            if (bdCustomerId > 0)
            {
                parameters.Add("@BdCustomerId", bdCustomerId, DbType.Int32);
            }
            if (!string.IsNullOrWhiteSpace(customerId))
            {
                parameters.Add("@CustomerId", customerId, DbType.String);
            }
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }

            using var connection = Common.Database;
            var customer = connection.QueryFirstOrDefault<Customers>("GetCustomer", parameters, commandType: CommandType.StoredProcedure);
            return customer;
        }

        public void SaveCustomer(Customers customer)
        {
            using var connection = Common.Database;
            connection.Insert(customer);
        }
    }
}
