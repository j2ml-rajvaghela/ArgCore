using Arg.DataModels;
using CuttingEdge.Conditions;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace Arg.DataAccess
{
    public class ArgInvoices_PaymentsImpl
    {
        public List<ArgInvoices_Payments> GetInvoicePayments(string invoiceNo, int companyId)
        {
            Condition.Requires(invoiceNo).IsNotNullOrWhiteSpace();
            var parameters = new DynamicParameters();

            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }
            parameters.Add("@InvoiceNo", invoiceNo, DbType.String);

            using var connection = Common.ClientDatabase;
            var invoicesPayments = connection.Query<ArgInvoices_Payments>("GetInvoicePayments", parameters, commandType: CommandType.StoredProcedure).ToList();
            return invoicesPayments;
        }

        public void SaveInvoicesPayments(ArgInvoices_Payments invPay)
        {
            if (string.IsNullOrWhiteSpace(invPay.Region))
            {
                throw new Exception("Region can't be empty.");
            }

            using var connection = Common.ClientDatabase;
            connection.Insert(invPay);
        }
    }
}
