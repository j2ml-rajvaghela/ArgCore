using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace CustomExtensions
{
    //Extension methods must be defined in a static class
    public static class StringExtension
    {
        public static DateTime ToDateTime(this string value)
        {
            if (value == null)
            {
                return DateTime.MinValue;
            }
            CultureInfo newCulture = new("en-US");
            newCulture.DateTimeFormat.ShortDatePattern = "MM-dd-yyyy";
            newCulture.DateTimeFormat.DateSeparator = "-";
            return DateTime.ParseExact(value, "MM-dd-yyyy", newCulture);
        }

        public static string Left(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            maxLength = Math.Abs(maxLength);

            value = (value.Length <= maxLength
                   ? value
                   : value.Substring(0, maxLength)
                   );
            return value;
        }
    }
}
namespace Arg.DataAccess
{
    public class Common
    {
        public static AspNetRolesImpl AspNetRoles = new();
        public static AppActionsImpl AppActions = new();

        private static readonly IConfiguration _configuration;
        private static readonly IHttpContextAccessor _httpContextAccessor;

        public static string GetIPAddress()
        {
            HttpContext context = _httpContextAccessor.HttpContext;
            string ipAddress = _httpContextAccessor.HttpContext.Request.Headers["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    ipAddress = addresses[0];
                }
            }
            ipAddress = _httpContextAccessor.HttpContext.Request.Headers["REMOTE_ADDR"];
            if (ipAddress == "::1")
            {
                ipAddress = "127.0.0.1"; // localhost
            }

            return ipAddress;
        }
        public static string ClientsDBName => _configuration["ClientsDBName"];
        public static string DBName => _configuration["DBName"];
        public static string EncryptKey => _configuration["EncryptKey"] ?? "hjhjh7668fjhjf86hj8f6hj86h86jh";
        public static string CurrentUserRoleId => AspNetRoles.GetAspNetRoleByName(CurrentUserRole)?.Id ?? "";
        public static SqlConnection ClientDatabase
        {
            get
            {
                var connectionString = _configuration.GetConnectionString("ClientDBName");
                var dbName = ActiveClient.Info.DBName;
                if (string.IsNullOrWhiteSpace(dbName))
                {
                    dbName = ClientsDBName;
                }
                var formatedConnectionString = string.Format(connectionString, dbName);
                var db = new SqlConnection(formatedConnectionString);
                db.Open();
                return db;
            }
        }

        public static string WildCardSearchToNormal(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (value.Contains('*' /*"*"*/))
                    value = value.Replace("*", "%");
                if (value.Contains('?' /*"?"*/))
                    value = value.Replace("?", "%");
                if (value.Contains('_' /*"_"*/))
                    value = value.Replace("_", "%");
                return value;
            }
            return null;
        }

        public static string DefaultDbName
        {
            get
            {
                string connectString = _configuration.GetConnectionString("DefaultConnection");
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectString);
                string dataSource = builder.InitialCatalog;
                return dataSource;
            }
        }

        public static SqlConnection Database
        {
            get
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                var db = new SqlConnection(connectionString);
                db.Open();
                return db;
            }
        }

        public static string CurrentUserId
        {
            get
            {
                if (_httpContextAccessor?.HttpContext?.User?.Identity == null)
                {
                    return "";
                }
                return _httpContextAccessor.HttpContext.User.Identity.Name ?? ""; // var userId = "35f7710d-ba3e-4e3d-96c0-6013518d8e59";  
            }
        }

        public static string CurrentUserName
        {
            get
            {
                var name = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
                var indexOfBackslash = name?.IndexOf("\\") + 1 ?? 0;
                name = indexOfBackslash > 0 ? name.Substring(indexOfBackslash) : "";
                return name;
                //var name = System.Web.HttpContext.Current.User.Identity.Name.Substring(System.Web.HttpContext.Current.User.Identity.Name.IndexOf(@"\") + 1);
                //if (name == null)
                //    name = "";
                //return name;
            }
        }

        public static string CurrentUserRole
        {
            get
            {
                if (string.IsNullOrEmpty(CurrentUserId))
                {
                    return "";
                }
                var userRoles = AspNetRoles.GetAspNetUserRole(CurrentUserId);
                return userRoles?.RoleName ?? "";
            }
        }

        public static string GetUserIpAddress()
        {
            string ip = _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"];

            if (string.IsNullOrEmpty(ip))
            {
                ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();

                if (ip == "::1")
                {
                    ip = "127.0.0.1";
                }
            }
            return ip;
        }

        public static bool IsActionAssignedToCurrentRole(string actionName)
        {
            var actions = Common.AppActions.GetActionsAssignedToCurrentRole(CurrentUserRoleId, actionName);
            if (actions != null && actions.Any())
            {
                var isAssigned = actions.Where(x => x.ActionName == actionName).Any();
                if (isAssigned)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
                return false;
        }

        public static class CanRunAction
        {
            // Research Items
            public static bool ViewAllClientBalanceDueActivity => IsActionAssignedToCurrentRole(GlobalObjects.ViewAllClientBalanceDueActivity);
            public static bool CloseResearchItem => IsActionAssignedToCurrentRole(GlobalObjects.CloseResearchItem);

            // BolAuditingResults
            public static bool BolAuditingResultsShowNavigation => IsActionAssignedToCurrentRole(GlobalObjects.BolAuditingResultsShowNavigation);

            // Dashboard
            public static bool ClientDashboard => IsActionAssignedToCurrentRole(GlobalObjects.ClientDashboard);
            public static bool ViewAllWeeklyBalanceDuesCollected => IsActionAssignedToCurrentRole(GlobalObjects.ViewAllWeeklyBalanceDuesCollected);
            public static bool ViewAllClientPendingBalanceDues => IsActionAssignedToCurrentRole(GlobalObjects.ViewAllClientPendingBalanceDues);
            public static bool ViewAllClientPendingApprovalBalanceDues => IsActionAssignedToCurrentRole(GlobalObjects.ViewAllClientPendingApprovalBalanceDues);
            public static bool ViewOpenInvoices => IsActionAssignedToCurrentRole(GlobalObjects.ViewOpenInvoices);

            // ARGInvoices
            public static bool AddNewInvoice => IsActionAssignedToCurrentRole(GlobalObjects.AddNewInvoice);
            public static bool AddBDToInvoice => IsActionAssignedToCurrentRole(GlobalObjects.AddBDToInvoice);
            public static bool InvoiceCommissions => IsActionAssignedToCurrentRole(GlobalObjects.InvoiceCommissions);
            public static bool PostingInvoicePayment => IsActionAssignedToCurrentRole(GlobalObjects.PostingInvoicePayment);

            // Balance Dues
            public static bool GenerateAuditReview => IsActionAssignedToCurrentRole(GlobalObjects.GenerateAuditReview);
            public static bool GenerateCustStatements => IsActionAssignedToCurrentRole(GlobalObjects.GenerateCustStatements);
            public static bool EmailCustomerStatements => IsActionAssignedToCurrentRole(GlobalObjects.EmailCustomerStatements);
            public static bool BDOutputToSpreadsheet => IsActionAssignedToCurrentRole(GlobalObjects.BDOutputToSpreadsheet);
            public static bool EditCustomerOnBDScreen => IsActionAssignedToCurrentRole(GlobalObjects.EditCustomerOnBDScreen);
            public static bool ViewBDInfo => IsActionAssignedToCurrentRole(GlobalObjects.ViewBDInfo);
            public static bool BDAmountPaidPopUp => IsActionAssignedToCurrentRole(GlobalObjects.BDAmountPaidPopUp);
            public static bool ChangeBDStatusTo => IsActionAssignedToCurrentRole(GlobalObjects.ChangeBDStatusTo);
            public static bool ChangeCollectionStatus => IsActionAssignedToCurrentRole(GlobalObjects.ChangeCollectionStatus);
            public static bool EditBDRevenueAnalystFields => IsActionAssignedToCurrentRole(GlobalObjects.EditBDRevenueAnalystFields);
            public static bool AddBalanceDue => IsActionAssignedToCurrentRole(GlobalObjects.AddBalanceDue);

            // AuditorPlaybook
            public static bool AddAuditorPlaybookSQL => IsActionAssignedToCurrentRole(GlobalObjects.AddAuditorPlaybookSQL);
            public static bool PlayAuditorPlaybook => IsActionAssignedToCurrentRole(GlobalObjects.PlayAuditorPlaybook);
            public static bool DeleteAuditorPlaybook => IsActionAssignedToCurrentRole(GlobalObjects.DeleteAuditorPlaybook);
            public static bool UpdateAuditorPlaybook => IsActionAssignedToCurrentRole(GlobalObjects.UpdateAuditorPlaybook);
            public static bool UpdateAuditorPlaybookSQL => IsActionAssignedToCurrentRole(GlobalObjects.UpdateAuditorPlaybookSQL);

            // Commissions
            public static bool ViewAllClientCommissions => IsActionAssignedToCurrentRole(GlobalObjects.ViewAllClientCommissions);

            //View Other User Role Commissions
            public static bool ViewOtherUserRoleCommissionsClientManager => IsActionAssignedToCurrentRole(GlobalObjects.ViewOtherUserRoleCommissionsClientManager);
            public static bool ViewOtherUserRoleCommissionsARGSalesAnalyst => IsActionAssignedToCurrentRole(GlobalObjects.ViewOtherUserRoleCommissionsARGSalesAnalyst);
            public static bool ViewOtherUserRoleCommissionsARGManager => IsActionAssignedToCurrentRole(GlobalObjects.ViewOtherUserRoleCommissionsARGManager);
            public static bool ViewOtherUserRoleCommissionsAuditManager => IsActionAssignedToCurrentRole(GlobalObjects.ViewOtherUserRoleCommissionsAuditManager);
            public static bool ViewOtherUserRoleCommissionsInfoXRevenueAnalyst => IsActionAssignedToCurrentRole(GlobalObjects.ViewOtherUserRoleCommissionsInfoXRevenueAnalyst);
            public static bool ViewOtherUserRoleCommissionsInfoXAuditManage => IsActionAssignedToCurrentRole(GlobalObjects.ViewOtherUserRoleCommissionsInfoXAuditManage);
            public static bool ViewOtherUserRoleCommissionsRevenueAnalyst => IsActionAssignedToCurrentRole(GlobalObjects.ViewOtherUserRoleCommissionsRevenueAnalyst);
        }

        public static class GlobalObjects
        {
            // Research Items
            public static string CloseResearchItem = "ResearchItems.CloseResearchItem";

            // BolAuditingResults
            public static string BolAuditingResultsShowNavigation = "BolAuditingResults.ShowNavigation";

            // Dashboard
            public static string ViewAllClientBalanceDueActivity = "Dashboard.ViewAllClientBalanceDueActivity";
            public static string ClientDashboard = "Dashboard.Client";
            public static string ViewOpenInvoices = "Dashboard.ViewOpenInvoices";
            public static string ViewAllClientPendingBalanceDues = "Dashboard.ViewAllClientPendingBalanceDues";
            public static string ViewAllWeeklyBalanceDuesCollected = "Dashboard.ViewAllWeeklyBalanceDuesCollected";
            public static string ViewAllClientPendingApprovalBalanceDues = "Dashboard.ViewAllClientPendingApprovalBalanceDues";

            // ARGInvoices
            public static string AddNewInvoice = "ARGInvoices.AddNewInvoice";
            public static string AddBDToInvoice = "ARGInvoices.AddBDToInvoice";
            public static string InvoiceCommissions = "ARGInvoices.Commissions";
            public static string PostingInvoicePayment = "ARGInvoices.PostingInvoicePayment";

            // Balance Dues
            public static string GenerateAuditReview = "BalanceDues.GenerateAuditReview";
            public static string GenerateCustStatements = "BalanceDues.GenerateCustomerStatements";
            public static string EmailCustomerStatements = "BalanceDues.EmailCustomerStatements";
            public static string BDOutputToSpreadsheet = "BalanceDues.BDOutputToSpreadsheet";
            public static string EditCustomerOnBDScreen = "BalanceDues.EditCustomer";
            public static string ViewBDInfo = "BalanceDues.ViewBDInfo";
            public static string BDAmountPaidPopUp = "BalanceDues.PostPayment";
            public static string ChangeBDStatusTo = "BalanceDues.ChangeBalanceDueStatusto";
            public static string ChangeCollectionStatus = "BalanceDues.ChangeCollectionStatus";
            public static string EditBDRevenueAnalystFields = "BalanceDues.EditBDRevenueAnalystFields";
            public static string AddBalanceDue = "AuditingScreen.AddBalanceDue";

            // AuditorPlaybook
            public static string AddAuditorPlaybookSQL = "AuditorPlaybookSQL.Add";
            public static string DeleteAuditorPlaybook = "AuditorPlaybook.Delete";
            public static string UpdateAuditorPlaybook = "AuditorPlaybook.Update";
            public static string PlayAuditorPlaybook = "AuditorPlaybook.Play";
            public static string UpdateAuditorPlaybookSQL = "AuditorPlaybookSQL.Update";

            // Commissions
            public static string ViewAllClientCommissions = "Commissions.ViewAllClientCommissions";

            // View Other User Role Commissions
            public static string ViewOtherUserRoleCommissionsClientManager = "ViewOtherUserRoleCommissionsClientManager";
            public static string ViewOtherUserRoleCommissionsARGSalesAnalyst = "ViewOtherUserRoleCommissionsARGSalesAnalyst";
            public static string ViewOtherUserRoleCommissionsARGManager = "ViewOtherUserRoleCommissionsARGManager";
            public static string ViewOtherUserRoleCommissionsAuditManager = "ViewOtherUserRoleCommissionsAuditManager";
            public static string ViewOtherUserRoleCommissionsInfoXRevenueAnalyst = "ViewOtherUserRoleCommissionsInfoXRevenueAnalyst";
            public static string ViewOtherUserRoleCommissionsInfoXAuditManage = "ViewOtherUserRoleCommissionsInfoXAuditManager";
            public static string ViewOtherUserRoleCommissionsRevenueAnalyst = "ViewOtherUserRoleCommissionsRevenueAnalyst";

        }
    }
}
