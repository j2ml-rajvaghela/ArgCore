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
    public class ArgInvoicesBDImpl
    {
        public ArgInvoices_BalanceDues GetInvoicesBD(int invBdId, string invoice, int companyId = 0)
        {
            var parameters = new DynamicParameters();

            if (invBdId > 0)
            {
                parameters.Add("@InvoiceBDId", invBdId, DbType.Int32);
            }
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }
            if (!string.IsNullOrEmpty(invoice))
            {
                parameters.Add("@Invoice#", invoice, DbType.String);
            }

            using (var connection = Common.Database)
            {
                var invoicesBD = connection.QueryFirstOrDefault<ArgInvoices_BalanceDues>("GetInvoicesBD", parameters, commandType: CommandType.StoredProcedure);
                return invoicesBD;
            }

        }

        public decimal GetAmountDueUSD(string invoiceNo, int companyId)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@Invoice#", invoiceNo, DbType.String);
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }

            using (var connection = Common.Database)
            {
                var amountDueUSD = connection.ExecuteScalar<decimal>("GetAmountDueUSD", parameters, commandType: CommandType.StoredProcedure);
                return amountDueUSD;
            }

        }

        public decimal GetAmountDueUSDMultiple(List<string> invoiceNo, string companyId)
        {
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(companyId))
            {
                parameters.Add("@CompanyId", companyId);
            }
            if (invoiceNo != null)
            {
                string invoiceNoString = string.Join(",", invoiceNo);
                parameters.Add("Invoice#", invoiceNoString, DbType.String);
            }

            using (var connection = Common.Database)
            {
                var amountDueUSDMultiple = connection.ExecuteScalar<decimal>("GetAmountDueUSDMultiple", parameters, commandType: CommandType.StoredProcedure);
                return amountDueUSDMultiple;
            }
        }

        public List<ArgInvoices_BalanceDues> GetPDFInvoiceMultiple(List<string> invoiceNo, string userId, string companyId)
        {
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(userId))
            {
                parameters.Add("@UserId", userId, DbType.String);
            }  
            if (!string.IsNullOrWhiteSpace(companyId))
            {
                parameters.Add("@CompanyId", companyId);
            }
            if (invoiceNo != null)
            {
                string invoiceNoString = string.Join(",", invoiceNo);
                parameters.Add("Invoice#", invoiceNoString, DbType.String);
            }

            using (var connection = Common.Database)
            {
                var pdfInvoiceMultiple = connection.Query<ArgInvoices_BalanceDues>("GetPDFInvoiceMultiple", parameters, commandType: CommandType.StoredProcedure).ToList();
                return pdfInvoiceMultiple;
            }
        }

        public List<ArgInvoices_BalanceDues> GetPDFInvoice(string invoiceNo, string userId, int companyId)
        {
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(userId))
            {
                parameters.Add("@UserId", userId, DbType.String);
            }
            if (string.IsNullOrWhiteSpace(invoiceNo))
            {
                parameters.Add("Invoice#", invoiceNo, DbType.String);
            }
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId);
            }

            using (var connection = Common.Database)
            {
                var pdfInvoice = connection.Query<ArgInvoices_BalanceDues>("GetPDFInvoice", parameters, commandType: CommandType.StoredProcedure).ToList();
                return pdfInvoice;
            }

        }

        public List<ArgInvoices_BalanceDues> GetOpenInvoicesForClient(string userId)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@UserId", userId, DbType.String);

            using (var connection = Common.Database)
            {
                var openInvoicesForClients = connection.Query<ArgInvoices_BalanceDues>("GetOpenInvoicesForClient", parameters, commandType: CommandType.StoredProcedure).ToList();
                return openInvoicesForClients;
            }
        }

        public int GetArgInvBDCount(int companyId, string region, string customerId, string invoiceNo, string BolNo)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@BOL#", BolNo, DbType.String);
            parameters.Add("@CompanyId", companyId, DbType.Int32);
            parameters.Add("@CustomerId", customerId, DbType.String);
            parameters.Add("@Invoice#", invoiceNo, DbType.String);
            parameters.Add("@Region", region, DbType.String);

            using (var connection = Common.Database)
            {
                var openInvoicesForClients = connection.ExecuteScalar<int>("GetArgInvBDCount", parameters, commandType: CommandType.StoredProcedure);
                return openInvoicesForClients;
            }
        }

        public void SaveArgInvBD(ArgInvoices_BalanceDues argInvoices_BalanceDues)
        {
            using (var connection = Common.Database)
            {
                connection.Insert(argInvoices_BalanceDues);
            }
        }

        public int DeleteArgInvBD(int companyId, string region, string customerId, string bolNo)
        {
            var parameteres = new DynamicParameters();

            parameteres.Add("@CustomerId", customerId, DbType.String);
            parameteres.Add("@BOL#", bolNo, DbType.String);
            parameteres.Add("@CompanyId", companyId, DbType.Int32);
            parameteres.Add("@Region", region, DbType.String);
            const string query = @"DELETE FROM [ArgInvoices.BalanceDues] WHERE CustomerID=@CustomerId AND BOL#=@BOL# AND CompanyID=@CustomerId AND Region=@Region;";

            using (var connection = Common.Database)
            {
                var result = connection.Execute(query, parameteres);
                return result;
            }
        }
    }
}
