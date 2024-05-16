using Arg.DataModels;
using CustomExtensions;
using Dapper;
using Dapper.Contrib.Extensions;
using MySqlX.XDevAPI.Common;
using System.Data;

namespace Arg.DataAccess
{
    public class BalanceDues_PaymentsImpl
    {
        private string _clientDbName = Common.ClientsDBName;

        public decimal GetAmountPaidMultiple(List<string> invoiceNo, string companyId)
        {
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(companyId))
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }
            if (invoiceNo != null)
            {
                parameters.Add("@InvoiceNo", string.Join(",", invoiceNo), DbType.String);
            }

            using var connection = Common.Database;
            var amountPaidMultiple = connection.ExecuteScalar<decimal>("GetAmountPaidMultiple", parameters, commandType: CommandType.StoredProcedure);
            return amountPaidMultiple;
        }

        public List<BalanceDues_Payments> GetBalanceDuesPayments(BalanceDue balanceDue)
        {
            var parameters = new DynamicParameters();

            parameters.Add("CompanyId", balanceDue.CompanyId);
            parameters.Add("Region", balanceDue.Region);
            parameters.Add("Bol#", balanceDue.Bol);

            using var connection = Common.Database;
            var balanceDuesPayments = connection.Query<BalanceDues_Payments>("GetBalanceDuesPayments", parameters, commandType: CommandType.StoredProcedure).ToList();
            return balanceDuesPayments;
        }

        public List<BalanceDues_Payments> GetBalanceDuesPayments(string BolNo, int companyId, string invoiceNo, DateTime? invoiceDate)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Bol#", BolNo, DbType.String);
            parameters.Add("@CompanyID", companyId, DbType.Int32);
            parameters.Add("@BalanceDueInvoice##", invoiceNo, DbType.String);
            parameters.Add("@BalanceDueInvoiceDate#", invoiceDate, DbType.DateTime);

            using var connection = Common.Database;
            var balanceDuesPayments = connection.Query<BalanceDues_Payments>("GetBalanceDuesPaymentsByCompanyId", parameters, commandType: CommandType.StoredProcedure).ToList();
            return balanceDuesPayments;
        }

        public List<BalanceDues_Payments> GetRevenueRecovered(string startDate, string endDate, int companyId, List<string> regions, string currentUserId, string reportType)
        {
            var datesList = GetDatesFormatted(startDate, endDate);
            var parameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(currentUserId))
            {
                parameters.Add("@UserId", currentUserId, DbType.String);
            }
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }
            if (regions != null && regions.Any() && regions[0] != "All")
            {
                parameters.Add("@Region", string.Join(",", regions), DbType.String);
            }
            parameters.Add("@StartDate", datesList.StartDate, DbType.String);
            parameters.Add("@EndDate", datesList.EndDate, DbType.String);

            using var connection = Common.Database;
            var revenueRecovered = connection.Query<BalanceDues_Payments>("GetRevenueRecovered", parameters, commandType: CommandType.StoredProcedure).ToList();
            return revenueRecovered;
        }

        public List<BalanceDues_Payments> GetRevenueRecoveredOvercharge(string startDate, string endDate, int companyId, List<string> regions, string currentUserId, string reportType)//, bool argManager)
        {
            var dateList = GetDatesFormatted(startDate, endDate);
            var parameters = new DynamicParameters();

            parameters.Add("@StartDate", startDate, DbType.Date);
            parameters.Add("@EndDate", endDate, DbType.Date);
            parameters.Add("@CompanyId", companyId, DbType.Int32);
            if (regions != null && regions.Any() && regions[0] != "All")
            {
                parameters.Add("@Regions", string.Join(",", regions), DbType.String);
            }

            using var connection = Common.Database;
            var revenueOverCharges = connection.Query<BalanceDues_Payments>("GetRevenueRecoveredOvercharge", parameters, commandType: CommandType.StoredProcedure).ToList();
            return revenueOverCharges;
        }

        public decimal GetOverchargeRevenueLossRate(string region, int companyId, string startDate, string endDate)
        {
            var datesList = GetDatesFormatted(startDate, endDate);

            var parameters = new DynamicParameters();

            parameters.Add("@CompanyId", companyId, DbType.Int32);
            parameters.Add("@Region", region, DbType.String);

            using var connection = Common.Database;
            var revenueLossRate = connection.Query<BalanceDues_Payments>("GetOverchargeRevenueLossRate", parameters, commandType: CommandType.StoredProcedure);
            decimal totalVal = 0;
            foreach (var item in revenueLossRate)
            {
                totalVal += item.Percentage;
            }
            totalVal *= 100;
            return totalVal;
        }

        public List<BalanceDues_Payments> GetOverChargeLossTrend(string region, int companyId, string startDate, string endDate)
        {
            var datesList = GetDatesFormatted(startDate, endDate);

            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", companyId, DbType.Int32);
            parameters.Add("@Region", region, DbType.String);

            using var connection = Common.Database;
            var lossTrends = connection.Query<BalanceDues_Payments>("GetOverChargeLossTrend", parameters, commandType: CommandType.StoredProcedure).ToList();
            return lossTrends;
        }

        public List<BalanceDues_Payments> GetOverchargeByBDErrorCode(string region, int companyId, string startDate, string endDate)
        {
            var datesList = GetDatesFormatted(startDate, endDate);

            var parameters = new DynamicParameters();
            parameters.Add("@StartDate", datesList.StartDate, DbType.String);
            parameters.Add("@EndDate", datesList.EndDate, DbType.String);
            parameters.Add("@CompanyId", companyId, DbType.Int32);
            parameters.Add("@Region", region, DbType.String);

            using var connection = Common.Database;
            var bdErrorCode = connection.Query<BalanceDues_Payments>("GetOverchargeByBDErrorCode", parameters, commandType: CommandType.StoredProcedure).ToList();
            return bdErrorCode;
        }

        public List<BalanceDues_Payments> GetOverchargeByCustomer(string region, int companyId, string startDate, string endDate)
        {
            var datesList = GetDatesFormatted(startDate, endDate);

            var parameters = new DynamicParameters();
            parameters.Add("@StartDate", datesList.StartDate, DbType.String);
            parameters.Add("@EndDate", datesList.EndDate, DbType.String);
            parameters.Add("@CompanyId", companyId, DbType.Int32);
            parameters.Add("@Region", region, DbType.String);

            using var connection = Common.Database;
            var chargeByCustomer = connection.Query<BalanceDues_Payments>("GetOverchargeByCustomer", parameters, commandType: CommandType.StoredProcedure).ToList();
            return chargeByCustomer;
        }

        public List<BalanceDues_Payments> GetOverchargeByOrigin(string region, int companyId, string startDate, string endDate)
        {
            var datesList = GetDatesFormatted(startDate, endDate);

            var parameters = new DynamicParameters();
            parameters.Add("@StartDate", datesList.StartDate, DbType.String);
            parameters.Add("@EndDate", datesList.EndDate, DbType.String);
            parameters.Add("@CompanyId", companyId, DbType.Int32);
            parameters.Add("@Region", region, DbType.String);
            parameters.Add("@ClientDbName", _clientDbName, DbType.String);

            using var connection = Common.Database;
            var chargeByOrigin = connection.Query<BalanceDues_Payments>("GetOverchargeByOrigin", parameters, commandType: CommandType.StoredProcedure).ToList();
            return chargeByOrigin;
        }

        public decimal GetCollectionRatePaymentAmount(string region, int companyId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Region", region, DbType.String);
            parameters.Add("@CompanyId", companyId, DbType.Int32);

            using var connection = Common.Database;
            var paymentAmount = connection.ExecuteScalar<decimal>("GetCollectionRatePaymentAmount", parameters, commandType: CommandType.StoredProcedure);
            return paymentAmount;
        }

        public decimal GetRevenueLossRate(string region, int companyId, string startDate, string endDate)
        {
            var datesList = GetDatesFormatted(startDate, endDate);

            var parameters = new DynamicParameters();
            parameters.Add("@Region", region, DbType.String);
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }

            using var connection = Common.Database;
            var lossRate = connection.Query<BalanceDues_Payments>("GetRevenueLossRate", parameters, commandType: CommandType.StoredProcedure);
            decimal totalVal = 0;
            decimal totalPayment = 0;
            decimal totalScodeRev = 0;
            foreach (var item in lossRate)
            {
                // totalVal += item.Percentage;
                totalPayment += item.PaymentAmount;
                totalScodeRev += item.ScopeRevenue;
            }
            totalVal = (totalPayment / totalScodeRev) * 100;
            return totalVal;
        }

        public List<BalanceDues_Payments> GetRevenueLossTrend(string region, int companyId, string startDate, string endDate)
        {
            var datesList = GetDatesFormatted(startDate, endDate);

            var parameters = new DynamicParameters();
            parameters.Add("@Region", region, DbType.String);
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }

            using var connection = Common.Database;
            var lossTrend = connection.Query<BalanceDues_Payments>("GetRevenueLossTrend", parameters, commandType: CommandType.StoredProcedure).ToList();
            return lossTrend;
        }

        public List<BalanceDues_Payments> GetRevLossByBDErrorCode(string region, string startDate, string endDate)
        {
            var datesList = GetDatesFormatted(startDate, endDate);

            var parameters = new DynamicParameters();
            parameters.Add("@Region", region, DbType.String);
            parameters.Add("@StartDate", datesList.StartDate, DbType.String);
            parameters.Add("@EndDate", datesList.EndDate, DbType.String);

            using var connection = Common.Database;
            var lossByBdErrorCode = connection.Query<BalanceDues_Payments>("GetRevLossByBDErrorCode", parameters, commandType: CommandType.StoredProcedure).ToList();
            return lossByBdErrorCode;
        }

        public List<BalanceDues_Payments> GetRevRecoveredByOrigin(string region, string startDate, string endDate)
        {
            var datesList = GetDatesFormatted(startDate, endDate);

            var parameters = new DynamicParameters();
            parameters.Add("@Region", region, DbType.String);
            parameters.Add("@StartDate", datesList.StartDate, DbType.String);
            parameters.Add("@EndDate", datesList.EndDate, DbType.String);

            using var connection = Common.Database;
            var RecoveredByOrigin = connection.Query<BalanceDues_Payments>("GetRevRecoveredByOrigin", parameters, commandType: CommandType.StoredProcedure).ToList();
            return RecoveredByOrigin;
        }

        public List<BalanceDues_Payments> GetRevByCustomer(string region, int companyId, string startDate, string endDate)
        {
            var datesList = GetDatesFormatted(startDate, endDate);
            var parameters = new DynamicParameters();
            parameters.Add("@StartDate", datesList.StartDate, DbType.String);
            parameters.Add("@EndDate", datesList.EndDate, DbType.String);
            parameters.Add("@Region", region, DbType.String);
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }

            using var connection = Common.Database;
            var revByCustomer = connection.Query<BalanceDues_Payments>("GetRevByCustomer", parameters, commandType: CommandType.StoredProcedure).ToList();
            return revByCustomer;
        }

        public BalanceDues_Payments GetBalanceDuesPayment(int paymentId, string bolNo)
        {
            var parameters = new DynamicParameters();

            if (paymentId > 0)
            {
                parameters.Add("@PaymentId", paymentId, DbType.Int32);
            }
            if (!string.IsNullOrWhiteSpace(bolNo))
            {
                parameters.Add("@BOL#", bolNo, DbType.String);
            }

            using var connection = Common.Database;
            var balanceDuePayment = connection.QueryFirstOrDefault<BalanceDues_Payments>("GetBalanceDuesPaymentByPaymentId", parameters, commandType: CommandType.StoredProcedure);
            return balanceDuePayment;
        }

        public List<BalanceDues_Payments> GetBDPaymentCustomers()
        {
            using var connection = Common.Database;
            var bDPaymentCustomers = connection.Query<BalanceDues_Payments>("GetBDPaymentCustomers", commandType: CommandType.StoredProcedure).ToList();
            return bDPaymentCustomers;
        }

        public decimal GetBalanceDuesPaymentAmnt(string bolNo, int companyId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@BOL#", bolNo, DbType.String);
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }

            using var connection = Common.Database;
            var paymentAmnt = connection.ExecuteScalar<decimal>("GetBalanceDuesPaymentAmnt", parameters, commandType: CommandType.StoredProcedure);
            return paymentAmnt;
        }

        public int GetBDPayCount(int companyId, string region, string customerId, string invoiceNo, string BolNo)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Bol", BolNo, DbType.String);
            parameters.Add("@CompanyID", companyId, DbType.Int32);
            parameters.Add("@CustomerID", customerId, DbType.String);
            parameters.Add("@BalanceDueInvoice", invoiceNo, DbType.String);
            parameters.Add("@Region", region, DbType.String);

            using var connection = Common.Database;
            var bDPayCount = connection.ExecuteScalar<int>("GetBDPayCount", parameters, commandType: CommandType.StoredProcedure);
            return bDPayCount;
        }

        public void SaveBalanceDuesPayment(BalanceDues_Payments payments)
        {
            using var connection = Common.Database;
            connection.Insert(payments);
        }

        public int DeleteBDPay(string customerId, string bolNo, int companyId, string region)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", companyId, DbType.Int32);
            parameters.Add("@Region", region, DbType.String);
            parameters.Add("@CustomerId", customerId, DbType.String);
            parameters.Add("@BolNo", bolNo, DbType.String);

            const string query = @"DELETE FROM [BalanceDues.Payments] 
                                   WHERE CustomerID=@CustomerId AND BOL#=@BolNo AND CompanyID=@CompanyId AND Region=@Region;";

            using var connection = Common.ClientDatabase;
            var result = connection.Execute(query, parameters);
            return result;
        }

        public int DeleteBalanceDuesPayment(int companyId, string region, string customerId, string invoiceNo, string bolNo)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CustomerID", customerId, DbType.String);
            parameters.Add("@Bol", bolNo, DbType.String);
            parameters.Add("@CompanyID", companyId, DbType.Int32);
            parameters.Add("@Region", region, DbType.String);
            parameters.Add("@BalanceDueInvoice", invoiceNo, DbType.String);

            const string query = @"DELETE FROM [BalanceDues.Payments]
                                   WHERE CustomerID=@CustomerID AND BOL#=@Bol AND CompanyID=@CompanyID AND Region=@Region AND BalanceDueInvoice#=@BalanceDueInvoice;";

            using (var connection = Common.Database)
            {
                var result = connection.Execute(query, parameters);
                return result;
            }
        }

        private DatesList GetDatesFormatted(string startDate, string endDate)
        {
            var startDateFormatted = startDate.ToDateTime();
            var endDateFormatted = endDate.ToDateTime();
            var strStartDate = "";
            var strEndDate = "";
            if (startDateFormatted != DateTime.MinValue)
            {
                strStartDate = startDateFormatted.ToString("yyyy-MM-dd");
            }
            if (endDateFormatted != DateTime.MinValue)
            {
                strEndDate = endDateFormatted.ToString("yyyy-MM-dd");
            }
            var result = new DatesList { StartDate = strStartDate, EndDate = strEndDate };
            return result;
        }

        public class DatesList
        {
            public string StartDate { get; set; }
            public string EndDate { get; set; }
        }
    }
}
