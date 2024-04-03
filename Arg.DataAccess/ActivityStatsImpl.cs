using Arg.DataModels;
using CustomExtensions;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace Arg.DataAccess
{
    public class ActivityStatsImpl
    {
        private static  readonly IHttpContextAccessor _contextAccessor;
        private string _clientDbName = Common.ClientsDBName;

        static ActivityStatsImpl()
        {
            _contextAccessor = InitializeHttpContextAccessor();
        }

        private static IHttpContextAccessor InitializeHttpContextAccessor()
        {
            var serviceProvider = GetServiceProvider();
            return serviceProvider.GetRequiredService<IHttpContextAccessor>();
        }

        private static IServiceProvider GetServiceProvider()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return serviceCollection.BuildServiceProvider();
        }

        public enum EnumActions
        {
            Saved,
            LoggedIn,
            LoggedOff,
            Deleted,
            Updated,
            BOLViewed,
            LoginFailed,
            GeneratedAuditReview,
            EmailedInvoices,
            GeneratedBDOutputSpreadsheet,
            GeneratedStatsOutputSpreadsheet,
            InvalidLoginAttempt,
            SetupTFA,
            InvalidTFAAttempt
        }

        public void SaveActivityStats(EnumActions action, int clientId, string actionNote = "", string bolNo = "")
        {
            using (var db = Common.Database)
            {
                if (!string.IsNullOrWhiteSpace(actionNote)) actionNote = ", " + actionNote;
                var url = _contextAccessor.HttpContext.Request.Path;
                //url=url.Remove(url.Length - 1);
                var activityStat = new ActivityStats
                {

                    AddedOn = DateTime.Now,
                    ClientId = clientId,
                    IpAddress = Common.GetIPAddress(),
                    Note = action.ToString() + actionNote,
                    UserId = Common.CurrentUserId,
                    BolNo = bolNo ?? "",
                    UserName = Common.CurrentUserName,
                    WebPage = url,
                    EventType = action.ToString()
                };
                db.Insert(activityStat);
            }
        }

        public ActivityStats GetActivitySatByNote(string userId, string note)
        {
            var parameters = new DynamicParameters();
            if (userId != null)
            {
                parameters.Add("@UserId", userId, DbType.String);
            }
            if (note != null)
            {
                parameters.Add("@Note", note, DbType.String);
            }
            using (var connection = Common.Database)
            {
                var activityStatsByNote = connection.QueryFirstOrDefault<ActivityStats>("GetActivitySatByNote", parameters, commandType: CommandType.StoredProcedure);
                return activityStatsByNote;
            }
        }

        public List<ActivityStats> GetActivityStatWebPages(int companyId)
        {
            var parameters = new DynamicParameters();
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }
            using (var connection = Common.Database)
            {
                var activityStatsWebPages = connection.Query<ActivityStats>("GetActivityStatWebPagesByCompanyId", parameters, commandType: CommandType.StoredProcedure).ToList();
                return activityStatsWebPages;

            }
        }

        public List<ActivityStats> GetActivityStatIpAddress(int companyId)
        {
            using (var connection = Common.Database)
            {
                var parameters = new DynamicParameters();
                parameters.Add("@CompanyId", companyId, DbType.Int32);

                var activityStatsIpAddresses = connection.Query<ActivityStats>("GetActivityStatIpAddressByCompanyId", parameters, commandType: CommandType.StoredProcedure).ToList();
                return activityStatsIpAddresses;

            }
        }

        public List<ActivityStats> GetActivityStats(SearchOptions so, string currentUserId, bool argManager = false, string eventType = "")
        {
            var parameters = new DynamicParameters();
            if (argManager && !string.IsNullOrWhiteSpace(currentUserId))
            {
                string userIdsString = string.Join(",", so.UserIds);
                parameters.Add("@UserId", userIdsString, DbType.String);
            }
            if (so.UserIds != null && so.UserIds.Any())
            {
                string userIdsString = string.Join(",", so.UserIds);
                parameters.Add("@UserId", userIdsString, DbType.String);
            }
            if (so.Roles != null)
            {
                string rolesString = string.Join(",", so.Roles);
                parameters.Add("@Roles", rolesString, DbType.String);
            }
            if (so.IpAddresses != null)
            {
                string ipAddressesString = string.Join(",", so.IpAddresses);
                parameters.Add("@IpAddresses ", ipAddressesString, DbType.String);
            }
            if (so.WebPages != null)
            {
                string webPagesString = string.Join(",", so.WebPages);
                parameters.Add("@WebPages", webPagesString, DbType.String);
            }
            if (so.ClientId != 0)
            {
                parameters.Add("@ClientId", so.ClientId, DbType.Int32);
            }
            if (so.BeginDate != DateTime.MinValue)
            {
                var beginDateFormatted = so.BeginDate.ToString("yyyy-MM-dd");
                parameters.Add("@BeginDate", beginDateFormatted, DbType.Date);
            }
            if (so.EndDate != DateTime.MinValue)
            {
                var endDateFormatted = so.EndDate.ToString("yyyy-MM-dd");
                parameters.Add("@EndDate", endDateFormatted, DbType.Date);
            }
            if (!string.IsNullOrWhiteSpace(eventType))
            {
                parameters.Add("@EventType", eventType, DbType.String);
            }
            //parameters.Add("@CurrentUserId", currentUserId, DbType.String);
            //parameters.Add("@ArgManager", argManager, DbType.Boolean);
            using (var connection = Common.Database)
            {
                var activityStatus = connection.Query<ActivityStats>("GetActivityStats", parameters, commandType: CommandType.StoredProcedure).ToList();
                return activityStatus;
            }
        }

        public List<ActivityStats> GetActivityStats(string userId, string ipAddress, int clientId, string webPage, DateTime beginDate, DateTime endDate, string roleId, string eventType = "")
        {
            var parameters = new DynamicParameters();
            if (userId != null)
            {
                parameters.Add("@UserId", userId, DbType.String);
            }
            if (roleId != null)
            {
                parameters.Add("@RoleId", roleId, DbType.String);
            }
            if (ipAddress != null)
            {
                parameters.Add("@IpAddress", ipAddress, DbType.String);
            }
            if (clientId > 0)
            {
                parameters.Add("@ClientId", clientId, DbType.Int32);
            }
            if (webPage != null)
            {
                parameters.Add("@WebPage", webPage, DbType.String);
            }
            if (beginDate != DateTime.MinValue)
            {
                var beginDateFormatted = beginDate.ToString("yyyy-MM-dd");
                parameters.Add("@AddedOn", beginDateFormatted);
            }
            if (endDate != DateTime.MinValue)
            {
                var endDateFormatted = endDate.ToString("yyyy-MM-dd");
                parameters.Add("@AddedOn", endDateFormatted);
            }
            if (!string.IsNullOrWhiteSpace(eventType))
            {
                parameters.Add("@EventType", eventType);
            }
            using (var connection = Common.Database)
            {
                var activityStats = connection.Query<ActivityStats>("GetActivityStatsByUserId", parameters, commandType: CommandType.StoredProcedure).ToList();
                return activityStats;
            }

        }

        public List<ActivityStats> GetAnalystPerformance(SearchOptions so)
        {

            var parameters = new DynamicParameters();
            if (so.CompanyId > 0)
            {
                parameters.Add("@CompanyId", so.CompanyId, DbType.Int32);
            }
            if (!string.IsNullOrWhiteSpace(so.UserId))
            {
                parameters.Add("@UserId", so.UserId, DbType.String);
            }
            if (so.Analyst != null)
            {
                string analystString = string.Join(",", so.Analyst);
                parameters.Add("@Analyst", analystString, DbType.String);
            }
            if (so.ShipperID != null)
            {
                string shipperIDString = string.Join(",", so.ShipperID);
                parameters.Add("@ShipperId", shipperIDString, DbType.String);
            }
            if (so.TransactionViewStartDate != null && so.TransactionViewEndDate != null)
            {
                var tracStartDateFormatted = so.TransactionViewStartDate.ToDateTime();
                var tracEndDateFormatted = so.TransactionViewEndDate.ToDateTime();
                if (tracStartDateFormatted != DateTime.MinValue && tracEndDateFormatted != DateTime.MinValue)
                {
                    var strtracStartDate = tracStartDateFormatted.ToString("yyyy-MM-dd");
                    var strtracEndDate = tracEndDateFormatted.ToString("yyyy-MM-dd");
                    parameters.Add("@TransactionViewStartDate", strtracStartDate, DbType.Date);
                    parameters.Add("@TransactionViewEndDate", strtracEndDate, DbType.Date);
                }
            }

            parameters.Add("@ClientDBName", _clientDbName, DbType.String);

            using (var connection = Common.Database)
            {
                var analystPerformances = connection.Query<ActivityStats>("GetAnalystPerformance", parameters, commandType: CommandType.StoredProcedure).ToList();
                return analystPerformances;
            }
        }

        public List<ActivityStats> GetAgilityAnalystPerformance(SearchOptions so)
        {
            var parameters = new DynamicParameters();
            if (so.CompanyId > 0)
            {
                parameters.Add("@CompanyId", so.CompanyId, DbType.Int32);
            }
            if (!string.IsNullOrWhiteSpace(so.UserId))
            {
                parameters.Add("@UserId", so.UserId, DbType.String);
            }
            if (so.Analyst != null)
            {
                string analystString = string.Join(",", so.Analyst);
                parameters.Add("@Analyst", analystString, DbType.String);
            }
            if (so.ShipperID != null)
            {
                string shipperIDString = string.Join(",", so.ShipperID);
                parameters.Add("@ShipperId", shipperIDString, DbType.String);
            }
            if (so.TransactionViewStartDate != null && so.TransactionViewEndDate != null)
            {
                var tracStartDateFormatted = so.TransactionViewStartDate.ToDateTime();
                var tracEndDateFormatted = so.TransactionViewEndDate.ToDateTime();
                if (tracStartDateFormatted != DateTime.MinValue && tracEndDateFormatted != DateTime.MinValue)
                {
                    var strtracStartDate = tracStartDateFormatted.ToString("yyyy-MM-dd");
                    var strtracEndDate = tracEndDateFormatted.ToString("yyyy-MM-dd");
                    parameters.Add("@TransactionViewStartDate", strtracStartDate, DbType.Date);
                    parameters.Add("@TransactionViewEndDate", strtracEndDate, DbType.Date);
                }
            }
            parameters.Add("@ClientDBName", ActiveClient.Info.DBName, DbType.String);
            using (var connection = Common.Database)
                {
                var analystPerformances = connection.Query<ActivityStats>("GetAgilityAnalystPerformance",parameters, commandType: CommandType.StoredProcedure).ToList();
                return analystPerformances;
                }
        }

        public List<ActivityStats> GetAnalystPerformanceCeva(SearchOptions so)
        {
            var parameters = new DynamicParameters();
            if (so.CompanyId > 0)
            {
                parameters.Add("@CompanyId", so.CompanyId, DbType.Int32);
            }
            if (!string.IsNullOrWhiteSpace(so.UserId))
            {
                parameters.Add("@UserId", so.UserId, DbType.String);
            }
            if (so.Analyst != null)
            {
                string analystString = string.Join(",", so.Analyst);
                parameters.Add("@Analyst", analystString, DbType.String);
            }
            if (so.ShipperID != null)
            {
                string shipperIDString = string.Join(",", so.ShipperID);
                parameters.Add("@ShipperId", shipperIDString, DbType.String);
            }
            if (so.TransactionViewStartDate != null && so.TransactionViewEndDate != null)
            {
                var tracStartDateFormatted = so.TransactionViewStartDate.ToDateTime();
                var tracEndDateFormatted = so.TransactionViewEndDate.ToDateTime();
                if (tracStartDateFormatted != DateTime.MinValue && tracEndDateFormatted != DateTime.MinValue)
                {
                    var strtracStartDate = tracStartDateFormatted.ToString("yyyy-MM-dd");
                    var strtracEndDate = tracEndDateFormatted.ToString("yyyy-MM-dd");
                    parameters.Add("@TransactionViewStartDate", strtracStartDate, DbType.Date);
                    parameters.Add("@TransactionViewEndDate", strtracEndDate, DbType.Date);
                }
            }
            parameters.Add("@ClientDBName", ActiveClient.Info.DBName, DbType.String);
            using (var connection = Common.Database)
            {
                var analystPerformances = connection.Query<ActivityStats>("GetAnalystPerformanceCeva", so, commandType: CommandType.StoredProcedure).ToList();
                return analystPerformances;
            }
        }

        public List<ActivityStats> GetAnalystList()
        {
            using (var connection = Common.Database)
            {
                var analystsList = connection.Query<ActivityStats>("GetAllAnalystList", commandType: CommandType.StoredProcedure).ToList();
                return analystsList;
            }
        }

        public List<ActivityStats> GetActivityStatsByEventType(string eventType, string bolNo, int companyId, string userId)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@EventType", eventType, DbType.String);
            parameters.Add("@BolNo", bolNo, DbType.String);
            if (companyId > 0)
            {
                parameters.Add("@ClientId", companyId, DbType.String);
            }

            using (var connection = Common.Database)
            {
                var activityStatsByEventType = connection.Query<ActivityStats>("GetActivityStatsByEventType", parameters, commandType: CommandType.StoredProcedure).ToList();
                return activityStatsByEventType;
            }
                
        }
    }
}
