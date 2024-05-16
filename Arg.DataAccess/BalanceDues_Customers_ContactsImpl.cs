using Arg.DataModels;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace Arg.DataAccess
{
    public class BalanceDues_Customers_ContactsImpl
    {
        public List<BalanceDues_Customers_Contacts> GetDistinctLocationCodes(string customerId, int companyId)
        {
            var parameters = new DynamicParameters();
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }
            parameters.Add("@CustomerId", customerId, DbType.String);

            using var connection = Common.Database;
            var distinctLocationCodes = connection.Query<BalanceDues_Customers_Contacts>("GetDistinctLocationCodes", parameters, commandType: CommandType.StoredProcedure).ToList();
            return distinctLocationCodes;
        }

        public BalanceDues_Customers_Contacts GetContact(int contactId, string customerId, int companyId)
        {
            var parameters = new DynamicParameters();
            if (contactId > 0)
            {
                parameters.Add("@ContactId", contactId, DbType.Int32);
            }
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }
            if (!string.IsNullOrWhiteSpace(customerId))
            {
                parameters.Add("@CustomerId", customerId, DbType.String);
            }

            using var connection = Common.Database;
            var contact = connection.QueryFirstOrDefault<BalanceDues_Customers_Contacts>("GetCustomerContacts", parameters, commandType: CommandType.StoredProcedure);
            return contact;
        }

        public List<BalanceDues_Customers_Contacts> GetCustomerContactsForEmails(string customerId, string customerLocationCode, string region, int companyId)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@CustomerId", customerId, DbType.String);
            if (!string.IsNullOrWhiteSpace(region))
            {
                parameters.Add("@Region", region, DbType.String);
            }
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }

            using var connection = Common.Database;
            var contactForEmail = connection.Query<BalanceDues_Customers_Contacts>("GetCustomerContactsForEmails", parameters, commandType: CommandType.StoredProcedure).ToList();
            return contactForEmail;
        }

        public void SaveCustomerContact(BalanceDues_Customers_Contacts customersContact)
        {
            using var connection = Common.Database;
            if (customersContact.ContactId == 0)
            {
                customersContact.Email = customersContact.Email.Trim();
                connection.Insert(customersContact);
            }
            else
            {
                connection.Update(customersContact);
            }
        }

        public int DeleteCustomerContact(int contactId)
        {
            const string query = @"DELETE FROM [BalanceDues.Customers.Contacts] 
                                   WHERE ContactId=@ContactId;";

            using var connection = Common.Database;
            var result = connection.Execute(query, new { contactId });
            return result;
        }
    }
}
