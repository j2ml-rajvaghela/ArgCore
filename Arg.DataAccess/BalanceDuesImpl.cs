using Arg.Agility.DataModels.DriveHelpers;
using Arg.DataModels;
using Dapper;
using Dapper.Contrib.Extensions;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arg.DataAccess
{
    public class BalanceDuesImpl
    {
        public enum EnumDashboard
        {
            Daily, 
            Weekly
        }

        public void SetDateFirst()
        {
            using (var connection = Common.Database)
            {
                connection.Execute(@"SET DATEFIRST 7;");
            }
        }

        public List<BalanceDue> GetDistinctInvoiceTypesMultiple(string companyId)
        {
            var parameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(companyId))
            {
                parameters.Add("@CompanyIds", companyId, DbType.String);
            }
            using (var connection = Common.Database)
            {
                var distinctInvoiceTypesMultiple = connection.Query<BalanceDue>("GetDistinctInvoiceTypesMultiple", parameters, commandType: CommandType.StoredProcedure).ToList();
                return distinctInvoiceTypesMultiple;
            }
        }

        public List<BalanceDue> GetRevenueCollectedPastYear(string userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId, DbType.String);
            using (var connection = Common.Database)
            {
                var revenueCollectedPastYears = connection.Query<BalanceDue>("GetRevenueCollectedPastYear", parameters, commandType: CommandType.StoredProcedure).ToList();
                return revenueCollectedPastYears;
            }
        }

        public List<BalanceDue> GetRevenueCollectedForYearToDate(string userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId, DbType.String);
            using (var connection = Common.ClientDatabase)
            {
                var revenueCollectedForYearsToDates = connection.Query<BalanceDue>("GetRevenueCollectedForYearToDate", parameters, commandType: CommandType.StoredProcedure).ToList();
                return revenueCollectedForYearsToDates;
            }
        }

        public List<BalanceDue> GetBDSetUp(EnumDashboard enumValues, string userId = "")
        {
            SetDateFirst();
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId, DbType.String);
            parameters.Add("@EnumValues", enumValues, DbType.Int32);
            using (var connection = Common.Database)
            {
                var bdSetUps = connection.Query<BalanceDue>("GetBDSetUp", parameters, commandType: CommandType.StoredProcedure).ToList();
                return bdSetUps;
            }
        }

        public List<BalanceDue> GetBDCollected(EnumDashboard enumValues, string userId = "")
        {
            SetDateFirst();
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId, DbType.String);
            parameters.Add("@EnumValues", enumValues, DbType.Int32);
            using (var connection = Common.Database)
            {
                var bdCollected = connection.Query<BalanceDue>("GetBDCollected", parameters, commandType: CommandType.StoredProcedure).ToList();
                return bdCollected;
            }
        }

        public List<BalanceDue> GetPendingBD(string userId = "", string invoiceStatus = "")
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId, DbType.String);
            parameters.Add("@InvoiceStatus", invoiceStatus, DbType.String);
            using (var connection = Common.Database)
            {
                var pendingBds = connection.Query<BalanceDue>("GetPendingBD", parameters, commandType: CommandType.StoredProcedure).ToList();
                return pendingBds;
            }
        }

        public List<BalanceDue> GetOpenInvoices(string userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId, DbType.String);
            using (var connection = Common.Database)
            {
                var openInvoices = connection.Query<BalanceDue>("GetOpenInvoices", parameters, commandType: CommandType.StoredProcedure).ToList();
                return openInvoices;
            }
        }

        public List<BalanceDue> GetMultipleBalanceDues(string companyId, List<string> region, List<string> invoiceType)
        {
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(companyId))
            {
                parameters.Add("@CompanyId", companyId, DbType.String);
            }
            if (region != null)
            {
                parameters.Add("@Region", string.Join(",", region), DbType.String);
            }
            if (invoiceType != null)
            {
                parameters.Add("@InvoiceType", string.Join(",", invoiceType), DbType.String);
            }
               
            using (var connection = Common.Database)
            {
                var multipleBalanceDues = connection.Query<BalanceDue>("GetMultipleBalanceDues", parameters, commandType: CommandType.StoredProcedure).ToList();
                return multipleBalanceDues;
            }
        }

        public List<BalanceDue> GetDistinctInvoiceTypesByStatus(string companyId, List<string> region, string status)
        {
            var parameters = new DynamicParameters();

          
            if (!string.IsNullOrWhiteSpace(companyId))
            {
                parameters.Add("@CompanyId", companyId, DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(status))
            {
                parameters.Add("@Status", status, DbType.String);
            }
            if (region != null)
            {
                parameters.Add("@Region", string.Join(",", region), DbType.String);
            }

            using (var connection = Common.Database)
            {
                var distinctInvoiceTypesByStatus = connection.Query<BalanceDue>("GetDistinctInvoiceTypesByStatus", parameters, commandType: CommandType.StoredProcedure).ToList();
                return distinctInvoiceTypesByStatus;
            }
        }

        public BalanceDue GetMultipleBalanceDue(int balanceId, string bol, string companyId, List<string> invoiceTypes = null)
        {
            var parameters = new DynamicParameters();

            if (balanceId > 0)
            {
                parameters.Add("@BalanceId", balanceId, DbType.Int32);
            }
            if (!string.IsNullOrWhiteSpace(bol))
            {
                parameters.Add("@Bol", bol, DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(companyId))
            {
                parameters.Add("@CompanyId", companyId, DbType.String);
            }
            if (invoiceTypes != null)
            {
                parameters.Add("@InvoiceTypes", string.Join(",", invoiceTypes), DbType.String);
            }

            using (var connection = Common.Database)
            {
                var multipleBalanceDue = connection.QueryFirstOrDefault<BalanceDue>("GetMultipleBalanceDue", parameters, commandType: CommandType.StoredProcedure);
                return multipleBalanceDue;
            }
        }

        public BalanceDue GetBalanceDue(int balanceId, string bol, int companyId, List<string> invoiceTypes = null)
        {
            var parameters = new DynamicParameters();

            if (balanceId > 0)
            {
                parameters.Add("@BalanceId", balanceId, DbType.Int32);
            }
            if (!string.IsNullOrWhiteSpace(bol))
            {
                parameters.Add("@Bol", bol, DbType.String);
            }
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }
            if (invoiceTypes != null)
            {
                parameters.Add("@InvoiceTypes", string.Join("," ,invoiceTypes), DbType.String);
            }

            using (var connection = Common.Database)
            {
                var balaneDue = connection.QueryFirstOrDefault<BalanceDue>("GetBalanceDue", parameters, commandType: CommandType.StoredProcedure);
                return balaneDue;
            }
        }

        public BalanceDue GetBalanceDue(int balanceId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@BalanceId", balanceId, DbType.Int32);

            using (var connection = Common.Database)
            {
                var balanceDue = connection.QueryFirstOrDefault<BalanceDue>("GetBalanceDueByBalanceId", parameters);
                return balanceDue;
            }
        }

        public List<BalanceDue> GetDistinctRevenueAnalystAuditor(int companyId)
        {
            var parameters = new DynamicParameters();
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }

            using (var connection = Common.Database)
            {
                var distinctRevenueAnalystAuditor = connection.Query<BalanceDue>("GetDistinctRevenueAnalystAuditorByCompanyId", parameters, commandType: CommandType.StoredProcedure).ToList();
                return distinctRevenueAnalystAuditor;
            }
        }

        public List<BalanceDue> GetDistinctRevenueAnalystCollector(int companyId)
        {
            var parameters = new DynamicParameters();
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }

            using (var connection = Common.Database)
            {
                var distinctRevenueAnalystCollector = connection.Query<BalanceDue>("GetDistinctRevenueAnalystCollectorByCompanyId", parameters, commandType: CommandType.StoredProcedure).ToList();
                return distinctRevenueAnalystCollector;
            }
        }

        public List<BalanceDue> GetBalanceDuesByInvoice(string invoice, int companyId = 0)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@Invoice", invoice, DbType.String);
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }
               
            using (var connection = Common.Database)
            {
                var balanceDuesByInvoices = connection.Query<BalanceDue>("GetBalanceDuesByInvoice", parameters, commandType: CommandType.StoredProcedure).ToList();
                return balanceDuesByInvoices;
            }
        }

        public decimal GetCurrentOpenBalance(string region, int companyId, List<string> reportType)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Region", region, DbType.String);
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }
            if (reportType != null)
            {
                parameters.Add("@ReportType", string.Join(",", region), DbType.String);
            }

            using (var connection = Common.Database)
            {
                var openBalance = connection.ExecuteScalar<decimal>("GetCurrentOpenBalance", parameters, commandType: CommandType.StoredProcedure);
                return openBalance;
            }


        }

        public List<BalanceDue> GetARGBalanceDue(string bolNo, int companyId, string customerId, string region, DateTime bolExecDate)
        {
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(bolNo))
            {
                parameters.Add("@Bol", bolNo, DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(customerId))
            {
                parameters.Add("@CustomerId", customerId, DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(customerId))
            {
                parameters.Add("@Region", region, DbType.String);
            }
            if (bolExecDate != DateTime.MinValue)
            {
                parameters.Add("@BolExecutionDate", bolExecDate.ToString("yyyy-MM-dd"), DbType.String);
            }
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }

            using (var connection = Common.Database)
            {
                var argBalanceDues = connection.Query<BalanceDue>("GetARGBalanceDue", parameters, commandType: CommandType.StoredProcedure).ToList();
                return argBalanceDues;
            }


        }

        public decimal GetBDPaymentAmount(string bolNo, int companyId)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@BOL#", bolNo, DbType.String);
            parameters.Add("@CompanyId", companyId, DbType.Int32);

            using (var connection = Common.Database)
            {
                var bDPaymentAmount = connection.ExecuteScalar<decimal>("GetBDPaymentAmount", parameters, commandType: CommandType.StoredProcedure);
                return bDPaymentAmount;
            }
        }

        public List<BalanceDue> GetCustomerBalanceDues(string bolNo, int companyId)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@CompanyId", companyId, DbType.Int32);
            if (!string.IsNullOrWhiteSpace(bolNo))
            {
                parameters.Add("@Bol", bolNo, DbType.String);
            }

            using (var connection = Common.Database)
            {
                var customerBalanceDues = connection.Query<BalanceDue>("GetCustomerBalanceDues", parameters, commandType: CommandType.StoredProcedure).ToList();
                return customerBalanceDues;
            }
        }

        public List<BalanceDue> GetBalanceDues(int balanceId, string q, string customerId)
        {
            var parameters = new DynamicParameters();
            if (balanceId > 0)
            {
                parameters.Add("@BalanceId", balanceId, DbType.Int32);
            }
            if (!string.IsNullOrWhiteSpace(customerId))
            {
                parameters.Add("@CustomerId", customerId, DbType.String);
            }
            if (!String.IsNullOrWhiteSpace(q))
            {
                parameters.Add("@Q", q, DbType.Int32);
            }

            using (var connection = Common.Database)
            {
                var balanceDues = connection.Query<BalanceDue>("GetBalanceDuesByCustomerId", parameters, commandType: CommandType.StoredProcedure).ToList();
                return balanceDues;
            }
        }

        public void FixWildcards(ref SearchOptions so)
        {
            so.BookingId = Common.WildCardSearchToNormal(so.BookingId);
            so.Bol = Common.WildCardSearchToNormal(so.Bol);
        }

        private string _myCmd = @"b.*,(SELECT TOP 1 CustomerName FROM [BalanceDues.Customers] p WHERE p.CustomerId=b.CustomerId AND p.CompanyId=b.CompanyId) AS Customer,
                                (SELECT SUM(i.AmountDue) FROM [BalanceDues.Item] i WHERE i.Bol=b.Bol AND i.CompanyId=b.CompanyId) AS ItemsAmount,
                                (SELECT SUM(o.AmountDue) FROM [BalanceDues.OtherCharges] o WHERE o.BOL#=b.Bol AND o.CompanyId=b.CompanyId) AS OtherChargesAmount,
                                ISNULL((SELECT sum(paymentamount) FROM [BalanceDues.Payments] bdp
                                WHERE BOL#=b.bol AND b.customerid=bdp.customerid AND bdp.CompanyId=b.CompanyId AND bdp.BalanceDueInvoice#=
                                b.BalanceDueInvoice AND bdp.BalanceDueInvoiceDate=b.BalanceDueInvoiceDate AND b.BalanceDueInvoice<>''),0) AS AmountPaid,
                                b.BolExecutionDate AS ActualDepartureDate,c.Name AS Company";

        public List<BalanceDue> GetBalanceDues(SearchOptions so)
        {
            var sqlCmd = $"SELECT {_myCmd} FROM BalanceDues b";
            sqlCmd += " INNER JOIN ArgClients c ON c.CompanyId=b.CompanyId";
            FixWildcards(ref so);
            BuildCmd(ref sqlCmd, so);

            using (var connection = Common.Database)
            {
                var results = connection.Query<BalanceDue>(sqlCmd);
                return results.ToList();
            }
        }

        public void BuildCmd(ref string sqlCmd, SearchOptions so)
        {
            if (so.BalanceDuePaymentEndDate != DateTime.MinValue && so.BalanceDuePaymentStartDate != DateTime.MinValue)
            {
                sqlCmd += " " + "INNER JOIN [BalanceDues.Payments] bp ON bp.CompanyID=b.CompanyId AND bp.Region=b.Region AND b.CustomerId=bp.CustomerID AND b.Bol=bp.BOL# AND b.BolExecutionDate = bp.BOLExecutionDate AND b.BalanceDueInvoice = bp.BalanceDueInvoice# AND b.BalanceDueInvoiceDate=bp.BalanceDueInvoiceDate AND  bp.BalanceDueInvoice# <>''";
            }

            if (so.BDOtherChargeCodes != null)
            {
                sqlCmd += " INNER JOIN [BalanceDues.OtherCharges] o ON b.Bol=o.BOL#";
                sqlCmd += " INNER JOIN [BalanceDues.OtherChargesCodes] co ON co.ChargeCode=o.ChargeCode";
                sqlCmd += $" WHERE co.ChargeCode IN ('{string.Join(",", so.BDOtherChargeCodes) }')";
            }

            if (so.BalanceDuePaymentStartDate != DateTime.MinValue)
            {
                sqlCmd += $"AND bp.PaymentDate>'{so.BalanceDuePaymentStartDate:yyyy-MM-dd}'";
            }

            if (so.BalanceDuePaymentEndDate != DateTime.MinValue)
            {
                sqlCmd += $"AND bp.PaymentDate<'{so.BalanceDuePaymentEndDate:yyyy-MM-dd}'";
            }

            if (so.CompanyId > 0)
            {
                sqlCmd += $" AND b.CompanyId = {so.CompanyId}";
            }

            if (!string.IsNullOrWhiteSpace(so.Bol))
            {
                sqlCmd += $" AND b.Bol LIKE '%{so.Bol}%'";
            }

            if (!string.IsNullOrWhiteSpace(so.CloseReasonCode))
            {
                sqlCmd += $"AND b.CloseReasonCode = {so.CloseReasonCode} ";
            }

            if (!string.IsNullOrWhiteSpace(so.Quote))
            {
                sqlCmd += $"AND b.Quote = {so.Quote} ";
            }

            if (so.BalanceDueAmount > 0)
            {
                sqlCmd += $"AND b.BalanceDueAmount = {so.BalanceDueAmount}";
            }

            if (so.BalanceDueInvoiceStartDate != DateTime.MinValue)
            {
                sqlCmd += $"AND b.BolExecutionDate>'{so.BolExecutionStartDate:yyyy-MM-dd}'";
            }

            if (so.BolExecutionEndDate != DateTime.MinValue)
            {
                sqlCmd += $"AND b.BolExecutionDate<'{so.BolExecutionEndDate:yyyy-MM-dd}'";
            }

            if (so.BalanceDueInvoiceStartDate != DateTime.MinValue)
            {
                sqlCmd += $"AND b.BalanceDueInvoiceDate>'{so.BalanceDueInvoiceStartDate:yyyy-MM-dd}'";
            }

            if (so.BalanceDueInvoiceEndDate != DateTime.MinValue)
            {
                sqlCmd += $"AND b.BalanceDueInvoiceDate<'{so.BalanceDueInvoiceEndDate:yyyy-MM-dd}'";
            }

            if (so.DateAddedStart != DateTime.MinValue)
            {
                sqlCmd += $"AND b.DateAdded>'{so.DateAddedStart:yyyy-MM-dd}'";
            }

            if (so.DateAddedEnd != DateTime.MinValue)
            {
                sqlCmd += $"AND b.DateAdded<'{so.DateAddedEnd:yyyy-MM-dd}'";
            }

            if (!string.IsNullOrWhiteSpace(so.CustomerLocationCode))
            {
                sqlCmd += $"AND b.CustomerLocationCode = {so.CustomerLocationCode}";
            }

            if (!string.IsNullOrWhiteSpace(so.BookingId))
            {
                sqlCmd += $"AND b.BookingId LIKE '%{so.BookingId}%'";
            }

            if (so.RevenueAnalystAuditors != null)
            {
                sqlCmd += $"AND b.RevenueAnalystAuditor IN ({string.Join(",", so.RevenueAnalystAuditors.Select(ra => $"'{ra}'"))})";
            }

            if (so.RevenueAnalystCollectors != null)
            {
                sqlCmd += $"b.RevenueAnalystCollector IN ({string.Join(",", so.RevenueAnalystCollectors.Select(rac => $"'{rac}'"))})";
            }

            if (so.BalanceDueInvoices != null)
            {
                sqlCmd += $"AND b.BalanceDueInvoice IN ({string.Join(",", so.BalanceDueInvoices.Select(bi => $"'{bi}'"))})";
            }

            if (so.CustomerIds != null)
            {
                sqlCmd += $"AND b.CustomerId IN ({string.Join(",", so.CustomerIds.Select(ci => $"'{ci}'"))})";
            }

            if (so.BDErrorCodes != null)
            {
                sqlCmd += $"AND b.BDErrorCode IN ({string.Join(",", so.BDErrorCodes.Select(bdc => $"'{bdc}'"))})";
            }

            if (so.Regions != null)
            {
                sqlCmd += $"AND b.Region IN ({string.Join(",", so.Regions.Select(r => $"'{r}'"))})";
            }

            if (so.InvoiceTypes != null)
            {
                sqlCmd += $"AND b.InvoiceType IN ({string.Join(",", so.InvoiceTypes.Select(it => $"'{it}'"))})";
            }

            if (so.Vessel != null)
            {
                sqlCmd += $"AND b.Vessel IN ({string.Join(",", so.Vessel.Select(v => $"'{v}'"))})";
            }

            if (so.Voyage != null)
            {
                sqlCmd += $"AND b.Voyage IN ({string.Join(",", so.Voyage.Select(v => $"'{v}'"))})";
            }

            if (so.MoveType != null)
            {
                sqlCmd += $"AND b.MoveType IN ({string.Join(",", so.MoveType.Select(mt => $"'{mt}'"))})";
        }

            if (so.OriginLocationCode != null)
            {
                sqlCmd += $"AND b.OriginLocationCode IN ({string.Join(",", so.OriginLocationCode.Select(ol => $"'{ol}'"))})";
            }

            if (so.DestinationLocationCode != null)
            {
                sqlCmd += $"AND b.DestinationLocationCode IN ({string.Join(",", so.DestinationLocationCode.Select(dl => $"'{dl}'"))})";
            }

            if (so.PortOfLoading != null)
            {
                sqlCmd += $"AND b.PortOfLoading IN ({string.Join(",", so.PortOfLoading.Select(pl => $"'{pl}'"))})";
            }

            if (so.PortOfDischarge != null)
            {
                sqlCmd += $"AND b.PortOfDischarge IN ({string.Join(",", so.PortOfDischarge.Select(pd => $"'{pd}'"))})";
            }

            if (so.InvoiceStatus != null)
            {
                sqlCmd += $"AND b.InvoiceStatus IN ({string.Join(",", so.InvoiceStatus.Select(invoice => $"'{invoice}'"))})";
            }

            if (so.CollectionStatus != null)
            {
                sqlCmd += $"AND b.CollectionStatus IN ({string.Join(",", so.CollectionStatus.Select(cs => $"'{cs}'"))})";
            }

            if (so.ClientGlStatus != null)
            {
                sqlCmd += $"AND b.ClientGlStatus IN ({string.Join(",", so.ClientGlStatus.Select(cgs => $"'{cgs}'"))})";
            }

            if (so.CloseReasonCodes != null)
            {
                sqlCmd += $"AND b.CloseReasonCode IN ({string.Join(",", so.CloseReasonCodes.Select(crs => $"'{crs}'"))})";
            }

            sqlCmd += " ORDER BY b.Region, CONCAT((SELECT TOP 1 CustomerName FROM [BalanceDues.Customers] p " +
                       " WHERE p.CustomerId=b.CustomerId AND p.CompanyId=b.CompanyId),'  ',c.Name),  " +
                       " b.BalanceDueInvoice, b.InvoiceStatus, b.CollectionStatus,b.OriginLocationCode, b.DestinationLocationCode";
        }

        public List<BalanceDue> GetBalanceDues(string bdIds)
        {
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(bdIds))
            {
                parameters.Add("@balanceIds", string.Join(",",bdIds), DbType.String);
            }

            using (var connection = Common.Database)
            {
                var distinctClientGLStatus = connection.Query<BalanceDue>("GetBalanceDuesByBDIds", parameters, commandType: CommandType.StoredProcedure).ToList();
                return distinctClientGLStatus;
            }
        }

        public List<BalanceDue> GetDistinctInvoiceStatus(int companyId)
        {
            var parameters = new DynamicParameters();
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }

            using (var connection = Common.Database)
            {
                var distinctInvoiceStatus = connection.Query<BalanceDue>("GetDistinctInvoiceStatusByCompanyId", parameters, commandType: CommandType.StoredProcedure).ToList();
                return distinctInvoiceStatus;
            }
        }

        public List<BalanceDue> GetDistinctClientGLStatus(int companyId)
        {
            var parameters = new DynamicParameters();
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }

            using (var connection = Common.Database)
            {
                var distinctClientGLStatus = connection.Query<BalanceDue>("GetDistinctClientGLStatusByCompanyId", parameters, commandType: CommandType.StoredProcedure).ToList();
                return distinctClientGLStatus;
            }
        }

        public List<BalanceDue> GetDistinctCloseReasonCodes(int companyId)
        {
            var parameters = new DynamicParameters();
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }

            using (var connection = Common.Database)
            {
                var distinctCloseReasonCodes = connection.Query<BalanceDue>("GetDistinctCloseReasonCodesByCompanyId", parameters, commandType: CommandType.StoredProcedure).ToList();
                return distinctCloseReasonCodes;
            }
        }

        public List<BalanceDue> GetDistinctInvoiceNo(int companyId)
        {
            var parameters = new DynamicParameters();
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }

            using (var connection = Common.Database)
            {
                var distinctInvoiceNo = connection.Query<BalanceDue>("GetDistinctInvoiceNoByCompanyId", parameters, commandType: CommandType.StoredProcedure).ToList();
                return distinctInvoiceNo;
            }
        }

        public List<BalanceDue> GetDistinctInvoiceTypes(int companyId)
        {
            var parameters = new DynamicParameters();
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }

            using (var connection = Common.Database)
            {
                var distinctInvoiceTypes = connection.Query<BalanceDue>("GetDistinctInvoiceTypesByCompanyId", parameters, commandType: CommandType.StoredProcedure).ToList();
                return distinctInvoiceTypes;
            }
        }

        public List<BalanceDue> GetDistinctMoveTypes(int companyId)
        {
            var parameters = new DynamicParameters();
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }

            using (var connection = Common.Database)
            {
                var distinctMoveTypes = connection.Query<BalanceDue>("GetDistinctMoveTypesByCompanyId", parameters, commandType: CommandType.StoredProcedure).ToList();
                return distinctMoveTypes;
            }
        }

        public List<BalanceDue> GetDistinctVessels(int companyId)
        {
            var parameters = new DynamicParameters();
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }

            using (var connection = Common.Database)
            {
                var distinctVessels = connection.Query<BalanceDue>("GetDistinctVesselsByCompanyId", parameters, commandType: CommandType.StoredProcedure).ToList();
                return distinctVessels;
            }
        }

        public List<BalanceDue> GetDistinctVoyages(int companyId)
        {
            var parameters = new DynamicParameters();
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }

            using (var connection = Common.Database)
            {
                var distinctVoyages = connection.Query<BalanceDue>("GetDistinctVoyagesByCompanyId", parameters, commandType: CommandType.StoredProcedure).ToList();
                return distinctVoyages;
            }
        }

        public List<BalanceDue> GetDistinctCollectionStatus(int companyId)
        {
            var parameters = new DynamicParameters();
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }

            using (var connection = Common.Database)
            {
                var distinctCollectionStatus = connection.Query<BalanceDue>("GetDistinctCollectionStatusByCompanyId", parameters, commandType: CommandType.StoredProcedure).ToList();
                return distinctCollectionStatus;
            }
        }

        public List<BalanceDue> GetDistinctOriginLocationCodes(int companyId)
        {
            var parameters = new DynamicParameters();
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }

            using (var connection = Common.Database)
            {
                var distinctOriginLocationCodes = connection.Query<BalanceDue>("GetDistinctOriginLocationCodesByCompanyId", parameters, commandType: CommandType.StoredProcedure).ToList();
                return distinctOriginLocationCodes;
            }
        }

        public List<BalanceDue> GetDistinctDestinationLocationCodes(int companyId)
        {
            var parameters = new DynamicParameters();
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }

            using (var connection = Common.Database)
            {
                var distinctDestinationLocationCodes = connection.Query<BalanceDue>("GetDistinctDestinationLocationCodesByCompanyId", parameters, commandType: CommandType.StoredProcedure).ToList();
                return distinctDestinationLocationCodes;
            }
        }

        public List<BalanceDue> GetDistinctPOL(int companyId)
        {
            var parameters = new DynamicParameters();
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }

            using (var connection = Common.Database)
            {
                var distinctPOL = connection.Query<BalanceDue>("GetDistinctPOLByCompanyId", parameters, commandType: CommandType.StoredProcedure).ToList();
                return distinctPOL;
            }
        }

        public List<BalanceDue> GetDistinctPOD(int companyId)
        {
            var parameters = new DynamicParameters();
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }

            using (var connection = Common.Database)
            {
                var distinctPOL = connection.Query<BalanceDue>("GetDistinctPODByCompanyId", parameters, commandType: CommandType.StoredProcedure).ToList();
                return distinctPOL;
            }
        }

        public string GenerateInvNoFromInvSummaryBasedOnBOL(string bolNo)
        {
            var invSumInfo = new InvoiceSummaryImpl().GetInvoiceNo(bolNo);

            if (invSumInfo != null && !string.IsNullOrWhiteSpace(invSumInfo.InvoiceNumber))
            {
                return invSumInfo.InvoiceNumber.Trim() + "B";
            }
            return null;
        }

        public void SaveBalanceDue(BalanceDue balanceDue)
        {
            if (string.IsNullOrWhiteSpace(balanceDue.BDErrorCode))
            {
                throw new Exception("Please select BDErrorCode");
            }

            using (var connection = Common.Database)
            {
                if (balanceDue.BalanceId == 0)
                {
                    connection.Insert(balanceDue);
                }
                else
                {
                    connection.Update(balanceDue);
                }
                
            }
        }

        public static Spreadhseet.SpreadsheetInfo CreateStatsSpreadsheet(StatsOutputSpreadsheet statsResults, string sheetTitle, string siteRoot, string filter)
        {
            if (ActiveClient.Info.DBName.Contains("ClientPasha"))
            {
                var spreadsheet = new Spreadsheet();
                spreadsheet.Sheets = new List<Sheet>();
                spreadsheet.Properties = new SpreadsheetProperties();
                spreadsheet.Properties.Title = sheetTitle;
                var sheet1 = new Sheet();
                sheet1.Properties = new SheetProperties();
                sheet1.Properties.Title = "Stats Results";
                sheet1.Properties.SheetId = 0;
                sheet1.Merges = new List<GridRange>();

                //INITIALIZE EVERYTHING
                var gd = new GridData();
                gd.RowData = new List<RowData>();
                var rd = new RowData();
                rd.Values = new List<CellData>();

                sheet1.Data = new List<GridData>();
                sheet1.Data.Add(gd);

                Spreadhseet.MergeCells(0, 4, 0, 4, ref sheet1);
                Spreadhseet.MergeCells(10, 11, 0, 20, ref sheet1);

                spreadsheet.Sheets.Add(sheet1);
                spreadsheet = Spreadhseet.Service.Spreadsheets.Create(spreadsheet).Execute();

                var sheet1Id = Convert.ToInt32(spreadsheet.Sheets[0].Properties.SheetId);
                var idx = 0;
                var runningIdx = 0;
                var batchSize = 500;
                var totalResults = statsResults.OutputSpreadsheetStats.Count;

                if (totalResults < batchSize)
                {
                    batchSize = totalResults;
                }

                gd = new GridData();
                gd.RowData = new List<RowData>();
                var bu = new BatchUpdateSpreadsheetRequest();
                bu.Requests = new List<Request>();
                var req = new Request();

                Spreadhseet.AddValueRowToSheetData("", ref gd);
                Spreadhseet.AddValueRowToSheetData("", ref gd);
                Spreadhseet.AddValueRowToSheetData("", ref gd);
                Spreadhseet.AddValueRowToSheetData("", ref gd);
                Spreadhseet.AddValueRowToSheetData("ARG Stats Result", ref gd, Spreadhseet.H1HeadingCellFormat);
                Spreadhseet.AddValueRowToSheetData("", ref gd);

                Spreadhseet.AddRowData(new[] { "Report Date", DateTime.Today.Date.ToString("MM/dd/yyyy") }, ref gd, Spreadhseet.DefaultCellFormat);
                Spreadhseet.AddValueRowToSheetData(filter, ref gd, Spreadhseet.DefaultCellFormat);
                Spreadhseet.AddValueRowToSheetData("", ref gd);
                Spreadhseet.AddValueRowToSheetData("", ref gd);

                Spreadhseet.AddRowData(new[] { "Stats Data", "", "", "", "", "", "", "", "", "", "" }, ref gd, Spreadhseet.HeadingCellFormat, true);
                string[] headings = { "Shipper ID", "Participant Name", "Origin", "Destination", "Shipment Count", "Min Charges", "Max Charges", "Difference", "Standard Deviation", "POL" };
                Spreadhseet.AddRowData(headings, ref gd, Spreadhseet.HeadingCellFormat, true);

                decimal totalsOfTotalCharges = 0;
                decimal totalAmountPaid = 0;
                decimal totalAmountDue = 0;

                foreach (var item in statsResults.OutputSpreadsheetStats)
                {
                    idx++;
                    runningIdx++;
                    rd = new RowData();
                    rd.Values = new List<CellData>();

                    Spreadhseet.AddCellDataRow(item.ShipperID ?? (" "), ref rd, Spreadhseet.CenterAlignCellFormatDetailsSheet);
                    Spreadhseet.AddCellDataRow(item.ParticipantName ?? (" "), ref rd, Spreadhseet.LeftAlignCellFormatDetailsSheet);
                    Spreadhseet.AddCellDataRow(item.Origin ?? (" "), ref rd, Spreadhseet.CenterAlignCellFormatDetailsSheet);
                    Spreadhseet.AddCellDataRow(item.Destination ?? (" "), ref rd, Spreadhseet.CenterAlignCellFormatDetailsSheet);
                    Spreadhseet.AddCellDataRow(Convert.ToString(item.ShipmentCount) ?? (" "), ref rd, Spreadhseet.CenterAlignCellFormatDetailsSheet);
                    Spreadhseet.AddCellDataRow(item.MinCharges.ToString("F") ?? (" "), ref rd, Spreadhseet.CenterAlignCellFormatDetailsSheet);
                    Spreadhseet.AddCellDataRow(item.MaxCharges.ToString("F") ?? (" "), ref rd, Spreadhseet.CenterAlignCellFormatDetailsSheet);
                    Spreadhseet.AddCellDataRow(item.Difference.ToString("F") ?? (" "), ref rd, Spreadhseet.CenterAlignCellFormatDetailsSheet);
                    Spreadhseet.AddCellDataRow(item.StandardDeviation.ToString("F") ?? (" "), ref rd, Spreadhseet.CenterAlignCellFormatDetailsSheet);
                    Spreadhseet.AddCellDataRow(Convert.ToString(item.POL ?? (" ")), ref rd, Spreadhseet.CenterAlignCellFormatDetailsSheet);

                    gd.RowData.Add(rd);

                    if (idx == batchSize)
                    {
                        bu = new BatchUpdateSpreadsheetRequest();
                        bu.Requests = new List<Request>();
                        req = new Request();
                        req.AppendCells = new AppendCellsRequest();
                        req.AppendCells.SheetId = sheet1Id;
                        req.AppendCells.Rows = new List<RowData>();
                        req.AppendCells.Fields = "*";
                        req.AppendCells.Rows = gd.RowData;
                        bu.Requests.Add(req);
                        Spreadhseet.ProcessRequests(new List<Request> { req },
                            spreadsheet.SpreadsheetId);
                        gd = new GridData();
                        gd.RowData = new List<RowData>();
                        idx = 0;

                        if (totalResults - runningIdx < batchSize) 
                        {
                            batchSize = totalResults - runningIdx;
                        }     
                    }
                }

                gd = new GridData();
                gd.RowData = new List<RowData>();

                //SHEET1
                var req_shipperid = Spreadhseet.GenerateUpdateColumnWidthRequest(sheet1Id, 90, 0, 1);
                var req_participantname = Spreadhseet.GenerateUpdateColumnWidthRequest(sheet1Id, 250, 1, 2);
                var req_origin = Spreadhseet.GenerateUpdateColumnWidthRequest(sheet1Id, 120, 2, 3);
                var req_destination = Spreadhseet.GenerateUpdateColumnWidthRequest(sheet1Id, 100, 3, 4);
                var req_shipmentcount = Spreadhseet.GenerateUpdateColumnWidthRequest(sheet1Id, 150, 4, 5);
                var req_mincharges = Spreadhseet.GenerateUpdateColumnWidthRequest(sheet1Id, 100, 5, 6);
                var req_maxcharges = Spreadhseet.GenerateUpdateColumnWidthRequest(sheet1Id, 100, 6, 7);
                var req_difference = Spreadhseet.GenerateUpdateColumnWidthRequest(sheet1Id, 100, 7, 8);
                var req_standarddeviation = Spreadhseet.GenerateUpdateColumnWidthRequest(sheet1Id, 200, 8, 9);
                var req_pol = Spreadhseet.GenerateUpdateColumnWidthRequest(sheet1Id, 170, 9, 10);

                Spreadhseet.ProcessRequests(new List<Request> { req_shipperid, req_participantname, req_origin, req_destination, req_shipmentcount, req_mincharges, req_maxcharges, req_difference, req_standarddeviation, req_pol }, spreadsheet.SpreadsheetId);

                DriveHelper.OpenReadOnlySharingForFile(spreadsheet.SpreadsheetId);

                var sInfo = new Spreadhseet.SpreadsheetInfo();
                sInfo.Id = spreadsheet.SpreadsheetId;
                sInfo.Url = spreadsheet.SpreadsheetUrl;
                return sInfo;
            }
            else if (ActiveClient.Info.DBName.Contains("ClientAgility"))
            {
                var spreadsheetAgility = new Spreadsheet();

                spreadsheetAgility.Sheets = new List<Sheet>();
                spreadsheetAgility.Properties = new SpreadsheetProperties();
                spreadsheetAgility.Properties.Title = sheetTitle;
                var sheetagility = new Sheet();
                sheetagility.Properties = new SheetProperties();
                sheetagility.Properties.Title = "Stats Results";
                sheetagility.Properties.SheetId = 0;
                sheetagility.Merges = new List<GridRange>();

                var gdagility = new GridData();
                gdagility.RowData = new List<RowData>();
                var rdAgility = new RowData();
                rdAgility.Values = new List<CellData>();

                sheetagility.Data = new List<GridData>();
                sheetagility.Data.Add(gdagility);

                Spreadhseet.MergeCells(0, 4, 0, 4, ref sheetagility); 
                Spreadhseet.MergeCells(10, 11, 0, 20, ref sheetagility);

                spreadsheetAgility.Sheets.Add(sheetagility);
                spreadsheetAgility = Spreadhseet.Service.Spreadsheets.Create(spreadsheetAgility).Execute();

                var agilitysheet1Id = Convert.ToInt32(spreadsheetAgility.Sheets[0].Properties.SheetId);
                var idxagility = 0;
                var agilityrunningIdx = 0;
                var agilitybatchSize = 500;
                var agilitytotalResults = statsResults.AgilityOutputSpreadsheetStats.Count;

                if (agilitytotalResults < agilitybatchSize)
                {
                    agilitybatchSize = agilitytotalResults;
                }

                gdagility = new GridData();
                gdagility.RowData = new List<RowData>();
                var agilitybu = new BatchUpdateSpreadsheetRequest();
                agilitybu.Requests = new List<Request>();
                var req1 = new Request();

                Spreadhseet.AddValueRowToSheetData("", ref gdagility);
                Spreadhseet.AddValueRowToSheetData("", ref gdagility);
                Spreadhseet.AddValueRowToSheetData("", ref gdagility);
                Spreadhseet.AddValueRowToSheetData("", ref gdagility);
                Spreadhseet.AddValueRowToSheetData("ARG Stats Result", ref gdagility, Spreadhseet.H1HeadingCellFormat);
                Spreadhseet.AddValueRowToSheetData("", ref gdagility);

                Spreadhseet.AddRowData(new[] { "Report Date", DateTime.Today.Date.ToString("MM/dd/yyyy") }, ref gdagility, Spreadhseet.DefaultCellFormat);
                Spreadhseet.AddValueRowToSheetData(filter, ref gdagility, Spreadhseet.DefaultCellFormat);
                Spreadhseet.AddValueRowToSheetData("", ref gdagility);
                Spreadhseet.AddValueRowToSheetData("", ref gdagility);

                Spreadhseet.AddRowData(new[] { "Stats Data", "", "", "", "", "", "", "", "", "", "" }, ref gdagility, Spreadhseet.HeadingCellFormat, true);
                string[] agilityheadings = { "Shipper", "Participant Name", "Origin", "Destination", "Shipment Count", "Min Charges", "Max Charges", "Difference", "Standard Deviation", "POL" };
                Spreadhseet.AddRowData(agilityheadings, ref gdagility, Spreadhseet.HeadingCellFormat, true);

                foreach (var item in statsResults.AgilityOutputSpreadsheetStats)
                {
                    idxagility++;
                    agilityrunningIdx++;
                    rdAgility = new RowData();
                    rdAgility.Values = new List<CellData>();

                    Spreadhseet.AddCellDataRow(item.Shipper ?? (" "), ref rdAgility, Spreadhseet.CenterAlignCellFormatDetailsSheet);
                    Spreadhseet.AddCellDataRow(item.ParticipantName ?? (" "), ref rdAgility, Spreadhseet.LeftAlignCellFormatDetailsSheet);
                    Spreadhseet.AddCellDataRow(item.Origin ?? (" "), ref rdAgility, Spreadhseet.CenterAlignCellFormatDetailsSheet);
                    Spreadhseet.AddCellDataRow(item.Destination ?? (" "), ref rdAgility, Spreadhseet.CenterAlignCellFormatDetailsSheet);
                    Spreadhseet.AddCellDataRow(Convert.ToString(item.ShipmentCount) ?? (" "), ref rdAgility, Spreadhseet.CenterAlignCellFormatDetailsSheet);
                    Spreadhseet.AddCellDataRow(item.MinCharges.ToString("F") ?? (" "), ref rdAgility, Spreadhseet.CenterAlignCellFormatDetailsSheet);
                    Spreadhseet.AddCellDataRow(item.MaxCharges.ToString("F") ?? (" "), ref rdAgility, Spreadhseet.CenterAlignCellFormatDetailsSheet);
                    Spreadhseet.AddCellDataRow(item.Difference.ToString("F") ?? (" "), ref rdAgility, Spreadhseet.CenterAlignCellFormatDetailsSheet);
                    Spreadhseet.AddCellDataRow(item.StandardDeviation.ToString("F") ?? (" "), ref rdAgility, Spreadhseet.CenterAlignCellFormatDetailsSheet);
                    Spreadhseet.AddCellDataRow(Convert.ToString(item.PortofEntry ?? (" ")), ref rdAgility, Spreadhseet.CenterAlignCellFormatDetailsSheet);

                    gdagility.RowData.Add(rdAgility);

                    if (idxagility == agilitybatchSize)
                    {
                        agilitybu = new BatchUpdateSpreadsheetRequest();
                        agilitybu.Requests = new List<Request>();
                        req1 = new Request();
                        req1.AppendCells = new AppendCellsRequest();
                        req1.AppendCells.SheetId = agilitysheet1Id;
                        req1.AppendCells.Rows = new List<RowData>();
                        req1.AppendCells.Fields = "*";
                        req1.AppendCells.Rows = gdagility.RowData;
                        agilitybu.Requests.Add(req1);
                        Spreadhseet.ProcessRequests(new List<Request> { req1 }, spreadsheetAgility.SpreadsheetId);
                        gdagility = new GridData();
                        gdagility.RowData = new List<RowData>();
                        idxagility = 0;

                        if (agilitytotalResults - agilityrunningIdx < agilitybatchSize)
                        {
                            agilitybatchSize = agilitytotalResults - agilityrunningIdx;
                        }
                           
                    }
                }

                gdagility = new GridData();
                gdagility.RowData = new List<RowData>();

                //SHEET1
                var req_shipperid = Spreadhseet.GenerateUpdateColumnWidthRequest(agilitysheet1Id, 90, 0, 1);
                var req_participantname = Spreadhseet.GenerateUpdateColumnWidthRequest(agilitysheet1Id, 250, 1, 2);
                var req_origin = Spreadhseet.GenerateUpdateColumnWidthRequest(agilitysheet1Id, 120, 2, 3);
                var req_destination = Spreadhseet.GenerateUpdateColumnWidthRequest(agilitysheet1Id, 100, 3, 4);
                var req_shipmentcount = Spreadhseet.GenerateUpdateColumnWidthRequest(agilitysheet1Id, 150, 4, 5);
                var req_mincharges = Spreadhseet.GenerateUpdateColumnWidthRequest(agilitysheet1Id, 100, 5, 6);
                var req_maxcharges = Spreadhseet.GenerateUpdateColumnWidthRequest(agilitysheet1Id, 100, 6, 7);
                var req_difference = Spreadhseet.GenerateUpdateColumnWidthRequest(agilitysheet1Id, 100, 7, 8);
                var req_standarddeviation = Spreadhseet.GenerateUpdateColumnWidthRequest(agilitysheet1Id, 200, 8, 9);
                var req_pol = Spreadhseet.GenerateUpdateColumnWidthRequest(agilitysheet1Id, 170, 9, 10);

                Spreadhseet.ProcessRequests(new List<Request> { req_shipperid, req_participantname, req_origin, req_destination, req_shipmentcount, req_mincharges, req_maxcharges, req_difference, req_standarddeviation, req_pol }, spreadsheetAgility.SpreadsheetId);
                DriveHelper.OpenReadOnlySharingForFile(spreadsheetAgility.SpreadsheetId);

                var sInfo1 = new Spreadhseet.SpreadsheetInfo();
                sInfo1.Id = spreadsheetAgility.SpreadsheetId;
                sInfo1.Url = spreadsheetAgility.SpreadsheetUrl;
                return sInfo1;
            }
            else
            {
                return null;
            }
        }
    }
}
