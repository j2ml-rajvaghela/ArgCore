using Arg.DataModels;
using CustomExtensions;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace Arg.DataAccess
{
    public class ArgInvoicesImpl
    {
        public List<ArgInvoice> GetArgInvoices(string userId, string q)
        {
            var parameters = new DynamicParameters();
            if (userId != null && userId.Length > 0)
            {
                parameters.Add("@UserId", userId, DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(q))
            {
                parameters.Add("@q", q, DbType.String);
            }
            using (var connection = Common.Database)
            {
                var argInvoices = connection.Query<ArgInvoice>("GetArgInvoices", parameters, commandType: CommandType.StoredProcedure).ToList();
                return argInvoices;
            }
        }

        public ArgInvoice GetArgInvoice(int invoiceId, string invoiceNo = "", int companyId = 0)
        {
            var parameters = new DynamicParameters();
            if (invoiceId > 0)
            {
                parameters.Add("@InvoiceId", invoiceId, DbType.Int32);
            }
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }
            if (!string.IsNullOrWhiteSpace(invoiceNo))
            {
                parameters.Add("@InvoiceNo", invoiceNo, DbType.String);
            }
            using (var connection = Common.Database)
            {
                var argInvoice = connection.QueryFirstOrDefault<ArgInvoice>("GetArgInvoice", parameters, commandType: CommandType.StoredProcedure);
                return argInvoice;
            }
        }

        public List<ArgInvoice> GetDistinctInvoiceNo(List<string> companyId, List<string> regions, List<string> invoiceTypes)
        {
            var parameters = new DynamicParameters();

            if (companyId.Any() && companyId.FirstOrDefault() != string.Empty)
            {
                parameters.Add("@CompanyIds", string.Join(",", companyId), DbType.String);
            }
            if (regions.Any() && regions.FirstOrDefault() != string.Empty)
            {
                parameters.Add("@Regions", string.Join(",", regions), DbType.String);
            }
            if (invoiceTypes.Any() && invoiceTypes.FirstOrDefault() != string.Empty)
            {
                parameters.Add("@InvoiceTypes", string.Join(",", invoiceTypes), DbType.String);
            }
               
            using (var connection = Common.Database)
            {
                var distinctInvoicesNo = connection.Query<ArgInvoice>("GetDistinctInvoiceNo", parameters, commandType: CommandType.StoredProcedure).ToList();
                return distinctInvoicesNo;
            }
        }

        public List<ArgInvoice> GetDistinctInvoiceStatusMultiple(string companyId)
        {
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(companyId))
            {
                parameters.Add("@CompanyIds", companyId, DbType.String);
            }

            using (var connection = Common.Database)
            {  
                var distinctInvoiceStatusMultiples = connection.Query<ArgInvoice>("GetDistinctInvoiceStatusMultiple", parameters, commandType: CommandType.StoredProcedure).ToList();
                return distinctInvoiceStatusMultiples;
            }
        }

        public List<ArgInvoice> GetDistinctInvoiceNoMultiple(string companyId)
        {
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(companyId))
            {
                parameters.Add("@CompanyIds", string.Join(",", companyId), DbType.String);
            }

            using (var connection = Common.Database)
            {
                var distinctInvoiceNoMultiples = connection.Query<ArgInvoice>("GetDistinctInvoiceNoMultiple", parameters, commandType: CommandType.StoredProcedure).ToList();
                return distinctInvoiceNoMultiples;
            }
        }

        public ArgInvoice GetArgMultipleInvoice(int invoiceId, List<string> invoiceNo, int companyId = 0)
        {
            var parameters = new DynamicParameters();
 
            if (invoiceId > 0)
            {
                parameters.Add("@InvoiceId", invoiceId, DbType.Int32);
            }
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }
            if (invoiceNo != null)
            {
                parameters.Add("@InvoiceNos", string.Join(",", invoiceNo), DbType.String);
            }

            using (var connection = Common.Database)
            {
                var argMultipleInvoice = connection.QueryFirstOrDefault<ArgInvoice>("GetArgMultipleInvoice", parameters, commandType: CommandType.StoredProcedure);
                return argMultipleInvoice;
            }
        }

        public ArgInvoice GetArgInvoiceMultipleInfo(List<string> invoiceNo, string userId, string companyId)
        {
            var parameters = new DynamicParameters();
        
            if (!string.IsNullOrWhiteSpace(userId))
            {
                parameters.Add("@UserId", userId, DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(companyId))
            {
                parameters.Add("@companyId", companyId, DbType.String);
            }
            if (invoiceNo != null)
            {
                parameters.Add("@InvoiceNos", string.Join(",", invoiceNo), DbType.String);
            }
              
            using (var connection = Common.ClientDatabase)
            {
                var argInvoicesMultipleInfo = connection.QueryFirstOrDefault<ArgInvoice>("GetArgInvoiceMultipleInfo", parameters, commandType: CommandType.StoredProcedure);
                return argInvoicesMultipleInfo;
            }
        }

        public List<ArgInvoice> GetInvoices(SearchOptions so, string currentUserId, bool argManager = false)
        {
            var parameters = new DynamicParameters();

            if (so.InvoiceTypes != null)
            {
                parameters.Add("@InvoiceTypes", string.Join(",", so.InvoiceTypes), DbType.String);
            }

            if (!string.IsNullOrWhiteSpace(so.Bol))
            {
                parameters.Add("@BOL#", so.Bol, DbType.String);
            }

            if (so.CompIds != null)
            {
                parameters.Add("@CompIds", string.Join(",", so.CompIds), DbType.String);
            }

            if (so.InvoiceNos != null)
            {
                parameters.Add("@InvoiceNos", string.Join(",", so.InvoiceNos), DbType.String);
            }

            var startDateFormatted = so.InvoiceStartDate.ToDateTime();
            var endDateFormatted = so.InvoiceEndDate.ToDateTime();
            if (startDateFormatted != DateTime.MinValue && endDateFormatted != DateTime.MinValue)
            {
                var strStartDate = startDateFormatted.ToString("yyyy-MM-dd");
                var strEndDate = endDateFormatted.ToString("yyyy-MM-dd");
                parameters.Add("InvoiceStartDate", strStartDate, DbType.DateTime);
                parameters.Add("InvoiceEndDate", strEndDate, DbType.DateTime);
            }

            if (so.Regions != null)
            {
                parameters.Add("@Regions", string.Join(",", so.Regions), DbType.String);
            }

            if (so.SelectedStatus != null)
            {
                parameters.Add("@InvoiceStatus", string.Join(",", so.SelectedStatus), DbType.String);
            }

            if (!string.IsNullOrWhiteSpace(currentUserId))
            {
                parameters.Add("@CurrentUserId", currentUserId, DbType.String);
            }

            using (var connection = Common.Database)
            {
                var invoices = connection.Query<ArgInvoice>("GetInvoices", parameters, commandType: CommandType.StoredProcedure).ToList();
                return invoices;
            }
        }

        public void SaveArgInvoice(ArgInvoice argInvoice)
        {
            if (string.IsNullOrWhiteSpace(argInvoice.Region))
            {
                throw new Exception("Region can't be empty.");
            }

            using (var connection = Common.Database)
            {
                if (argInvoice.InvoiceId == 0)
                {
                    connection.Insert(argInvoice);
                }
                else
                {
                    connection.Update(argInvoice);
                }
                
            }
        }

        public int DeleteArgInvoice(int invoiceId)
        {
            const string query = "DELTE FROM ArgInvoices WHERE InvoiceId=@InvoiceId;";
            using (var connection = Common.ClientDatabase)
            {
                var result = connection.Execute(query, new { @InvoiceId = invoiceId });
                return result;
            }
        }
    }
}
