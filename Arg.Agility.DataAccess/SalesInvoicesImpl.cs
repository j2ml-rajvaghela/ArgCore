using Arg.Agility.DataModels;
using Arg.DataAccess;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Arg.Agility.DataAccess
{
    public class SalesInvoicesImpl
    {
        private readonly SqlConnection _connection;
        public SalesInvoicesImpl()
        {
            _connection = Common.ClientDatabase;
        }

        public List<SalesInvoices> GetDistinctCurrency()
        {
            const string query = @"SELECT DISTINCT InvoiceCurrency 
                                   FROM SalesInvoices
                                   WHERE InvoiceCurrency <> '';";

            return _connection.Query<SalesInvoices>(query, commandType: CommandType.Text).ToList();
        }

        public SalesInvoices GetBOLCharge(string jobNumber)
        {
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(jobNumber))
            {
                parameters.Add("@JobNumber", jobNumber, DbType.String);
            }
            const string query = @"SELECT * 
                                   FROM SalesInvoices 
                                   WHERE JobNumber=@JobNumber;";


            return _connection.QueryFirstOrDefault<SalesInvoices>(query, parameters);
        }

        public List<SalesInvoices> GetAgilityBalanceDuesOtherCharges(string jobNumber)
        {
            const string query = @"SELECT *  
                                   FROM SalesInvoices 
                                   WHERE JobNumber=@JobNumber;";

            return _connection.Query<SalesInvoices>(query, new { JobNumber = jobNumber }).ToList();
        }

        public List<SalesInvoices> GetAgilityBalanceDuesOtherChargesWithDesc(string jobNumber)
        {
            const string query = @"SELECT ROW_NUMBER() OVER(PARTITION BY o.JobNumber ORDER BY o.JobNumber) AS ItemId, o.ChargeValue, o.JobNumber, o.ChargeDescription, 
                                   o.ChargeCode AS ChargeCode, Concat(o.ChargeCode,' ',o.ChargeDescription) AS ChargeCodeDesc 
                                   FROM [SalesInvoices] o
                                   WHERE o.ChargeCode <> 'M1A' AND o.JobNumber=@JobNumber
                                   ORDER BY ItemId;";

            return _connection.Query<SalesInvoices>(query, new { JobNumber = jobNumber }).ToList();
        }
    }
}
