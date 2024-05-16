using Arg.Agility.DataModels;
using Arg.DataAccess;
using Dapper;
using System.Data;

namespace Arg.Agility.DataAccess
{
    public class SalesInvoicesImpl
    {
        public List<SalesInvoices> GetDistinctCurrency()
        {
            const string query = @"SELECT DISTINCT InvoiceCurrency FROM SalesInvoices
                                   WHERE InvoiceCurrency <> '';";

            using var connection = Common.ClientDatabase;
            var distinctCurrency = connection.Query<SalesInvoices>(query, commandType: CommandType.Text).ToList();
            return distinctCurrency;
        }

        public SalesInvoices GetBOLCharge(string jobNumber)
        {
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(jobNumber))
            {
                parameters.Add("@JobNumber", jobNumber, DbType.String);
            }
            const string query = @"SELECT * FROM SalesInvoices 
                                   WHERE JobNumber=@JobNumber;";

            using var connection = Common.ClientDatabase;
            var bolCharge = connection.QueryFirstOrDefault<SalesInvoices>(query, parameters);
            return bolCharge;
        }

        public List<SalesInvoices> GetAgilityBalanceDuesOtherCharges(string jobNumber)
        {
            const string query = @"SELECT * FROM SalesInvoices 
                                   WHERE JobNumber=@JobNumber;";

            using var connection = Common.ClientDatabase;
            var otherCharges = connection.Query<SalesInvoices>(query, new { JobNumber = jobNumber }).ToList();
            return otherCharges;
        }

        public List<SalesInvoices> GetAgilityBalanceDuesOtherChargesWithDesc(string jobNumber)
        {
            const string query = @"SELECT ROW_NUMBER() OVER(PARTITION BY o.JobNumber ORDER BY o.JobNumber) AS ItemId, o.ChargeValue, o.JobNumber, o.ChargeDescription, 
                                   o.ChargeCode AS ChargeCode, Concat(o.ChargeCode,' ',o.ChargeDescription) AS ChargeCodeDesc From [SalesInvoices] o
                                   WHERE o.ChargeCode <> 'M1A' AND o.JobNumber=@JobNumber
                                   ORDER BY ItemId;";

            using var connection = Common.ClientDatabase;
            var otherChargesWithDesc = connection.Query<SalesInvoices>(query, new { JobNumber = jobNumber }).ToList();
            return otherChargesWithDesc;
        }
    }
}
