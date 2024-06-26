﻿using Arg.Agility.DataAccess;
using Arg.Ceva.DataAccess;
using Arg.DataAccess;
using Arg.DataModels;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Security.Principal;
using static ArgCore.Helpers.IdentityConfig;

namespace ArgCore.Helpers
{
    public static class JavaScript
    {
        public static string Serialize(object o)
        {
            return JsonConvert.SerializeObject(o);
        }
    }

    public class Common
    {
        private static readonly IConfiguration _configuration;
        private static readonly IHttpContextAccessor _httpContextAccessor;
        private static readonly IWebHostEnvironment _webHostEnvironment;

        public class CommonObjects
        {
            public string Heading { get; set; }
            public string TopHeading { get; set; }
            public int CompanyId { get; set; }
        }

        public static string MissingInvoiceMessage
        {
            get
            {
                var clientName = Arg.DataAccess.ActiveClient.Info.Name;
                var message = "Invoice# assigned by " + clientName + " are missing for BOLs: <br/>";
                return message;
            }
        }

        public static bool IsCurrentCompanyAssigned => ArgClients.IsCurrentCompanyAssigned(CurrentUserId, GetActiveClientId());

        public class CurrentUserInfo
        {
            public static bool IsAdmin => CurrentUserRole == "Administrator";
            public static bool IsARGManager => CurrentUserRole == "ARGManager" ? true : false;
            public static bool IsClientManager => CurrentUserRole == "ClientManager" ? true : false;
            public static bool IsClientUser => CurrentUserRole == "Client User" ? true : false;
            public static bool IsARGAnalyst => CurrentUserRole == "ARGAnalyst" ? true : false;

        }

        public static SqlConnection GetConnection()
        {
            var connectionString = _configuration.GetConnectionString("ClientDBName");
            var dbName = Arg.DataAccess.ActiveClient.Info.DBName;
            if (string.IsNullOrWhiteSpace(dbName))
            {
                dbName = Arg.DataAccess.Common.ClientsDBName;
            }
            var formatedConnectionString = string.Format(connectionString, dbName);
            var db = new SqlConnection(formatedConnectionString);
            db.Open();
            return db;
        }

        public static ActivityStatsImpl ActivityStats = new();
        public static AspNetUsersImpl AspNetUsers = new();
        public static AspNetRolesImpl AspNetRoles = new();
        public static AspNetUserRolesImpl AspNetUserRoles = new();
        public static TemplatesImpl Templates = new();
        public static IPAddressRestrictionImpl IPAddressRestriction = new();
        public static TemplateCatsImpl TemplateCats = new();
        public static RSReasonCodesImpl RSReasonCodes = new();
        public static MenuItemsImpl MenuItems = new();
        public static MenusImpl Menus = new();
        public static RegionsImpl Regions = new();
        public static CustomersImpl Customers = new();
        public static ArgClientsImpl ArgClients = new();
        public static SettingGroupsImpl SettingGroups = new();
        public static SettingsImpl Settings = new();
        public static BalanceDues_Customers_ContactsImpl CustomerContacts = new();
        public static BdErrorCodesImpl BDErrorCodes = new();
        public static ClientSMTPAccountsImpl ClientSMTPAccounts = new();
        public static CurrencyConversionRatesImpl CurrencyConversionRates = new();
        public static CommissionRatesImpl CommissionRates = new();
        public static CommissionsImpl Commissions = new();
        public static AppActionsImpl AppActions = new();
        public static UserCompanyRelsImpl UserCompanyRels = new();
        public static AppActionRoleRelsImpl AppActionRoleRels = new();
        public static BdOtherChargeCodesImpl bdOtherChargeCodes = new();
        public static ResearchItemsImpl ResearchItems = new();
        public static ArgInvoicesImpl ArgInvoices = new();
        public static BalanceDuesImpl BalanceDues = new();
        public static RoleMenuRelsImpl RoleMenuRels = new();
        public static ClientsImpl Clients = new();
        public static Countries Countries = new();
        public static BOLHeaderImpl BOLHeader = new();
        public static BookingHeader BookingHeaders = new();
        public static MappingsImpl Mappings = new();
        public static TableSettingsImpl TableSettings = new();
        public static ArgInvoicesBDImpl ArgInvoicesBD = new();
        public static BalanceDues_PaymentsImpl BalanceDuesPayments = new();
        public static CollectionCommentsImpl CollectionComments = new();
        public static ArgInvoices_PaymentsImpl ArgInvoicePayments = new();
        public static BalanceDues_OtherChargesImpl BalanceDuesOtherCharges = new();
        public static ARCashImpl ARCash = new();
        public static QueryResultsImpl QueryResults = new();
        public static XrefAirServiceLevels XrefAirServiceLevels = new();
        public static XrefAirServiceLevelsDetails XrefAirServiceLevelsDetails = new();
        public static BookingHeader_ContainerDetail ContainerDetails = new();
        public static Arg.Ceva.DataAccess.DocumentImages DocImages = new();
        public static InvoiceCharges InvoiceCharges = new();
        public static XrefBookingTypes XrefBookingTypes = new();
        public static Arg.Ceva.DataAccess.Participants Participant = new();
        public static XrefPackagingCodes XrefPackagingCodes = new();
        public static XrefOceanCarriers XrefOceanCarriers = new();
        public static XrefGoodsTypes XrefGoodsTypes = new();
        public static Locations Location = new();
        public static XrefCountries XrefCountries = new();
        public static XrefAirCarriers XrefAirCarriers = new();
        public static BalanceDues_DescriptionsImpl BalanceDuesDescriptions = new();
        public static BDOtherChargeCodeImpl BDOtherChargesCodes = new();
        public static BalanceDues_CollectionStatusesImpl CollectionStatuses = new();
        public static GmailUtilities GmailUtilities = new();
        public static InvoiceSummaryImpl InvoiceSummary = new();
        public static BalanceDues_CloseReasonCodesImpl CloseReasonCode = new();
        public static BOLAuditSortingImpl BOLAuditSorting = new();
        public static AuditorPlaybooksImpl AuditorPlaybooks = new();
        public static PlaybookCommentImpl PlaybookComment = new();
        public static ShipmentJournalImpl ShipmentJournal = new();
        public static AgilityBOLHeaderImpl AgilityBOLHeader = new();
        public static BOLChargesImpl BOLCharges = new BOLChargesImpl();
        public static AgilityQueryResultsImpl AgilityQueryResults = new();
        public static ContainerEventTypesImpl ContainerEventTypes = new();
        public static BOLContainersImpl BOLContainers = new();
        public static BOLHazardousImpl BOLHazardous = new();
        public static BOLReferenceImpl BOLReference = new();
        public static BOLContainerDetailsImpl ContainerDetailsImpl = new();
        public static BookingsImpl Bookings = new();
        public static Arg.DataAccess.DocumentImagesImpl DocumentImages = new();
        public static BOLCommodityImpl BOLCommodity = new BOLCommodityImpl();
        public static ContainerEventHistoryImpl ContainerEventHistory = new();
        public static BookingsRemarksImpl BookingsRemarks = new();
        public static BookingsNotesImpl BookingsNotes = new();
        public static BOLRemarksImpl BOLRemarks = new();
        public static BookingHeadersImpl AgilityBookingHeaders = new();
        public static ParticipantsImpl Participants = new();
        public static SalesInvoicesImpl salesBOLCharges = new();

        public static ApplicationUserManager UserManager => _httpContextAccessor.HttpContext.RequestServices.GetService(typeof(ApplicationUserManager)) as ApplicationUserManager;
        public static string CurrentUserRoleId => AspNetRoles.GetAspNetRoleByName(CurrentUserRole)?.Id ?? "";
        public static IPrincipal CurrentUser => _httpContextAccessor.HttpContext?.User;
        public static string CurrentUserName => CurrentUser?.Identity?.Name?.Substring(CurrentUser.Identity.Name.IndexOf(@"\") + 1);
        public static int FailedLoginAttempt => Convert.ToInt32(_configuration["FailedLoginAttempt"]);
        public static string GoogleAuthKey => _configuration["GoogleAuthKey"];
        public static int EnableTFA => Convert.ToInt32(_configuration["EnableTFA"]);
        public static string TFAIssuer => _configuration["TFAIssuer"];
        public static string UsePdfJs => _configuration["UsePdfJs"] ?? "false".ToLower();
        public static int VersionNo => Convert.ToInt32(_configuration["VersionNo"]);
        public static int GetActiveClientId() => Arg.DataAccess.ActiveClient.Info.CompanyId;
        public static List<MenuItems> GetMenuItemsForCurrentUser() => MenuItems.GetAssignedMenuItemsWithChildren(CurrentUserRoleId);
        public static string ReportServerUserName => _configuration["ReportServerUserName"];
        public static string ReportServerPassword => _configuration["ReportServerPassword"];
        public static string ReportServerUrl => _configuration["ReportServerUrl"];
        public static int PageSize => Convert.ToInt32(_configuration["PageSize"]);
        public static string ClientFilesPath => Arg.DataAccess.ActiveClient.Info.ImportDataPath;
        public class ActiveClient
        {
            public static bool NameContains(string val)
            {
                //if (!string.IsNullOrWhiteSpace(GetName()))
                //{
                return GetName().Contains(val);
                //}
            }

            public static string GetName()
            {
                if (Arg.DataAccess.ActiveClient.Info != null && Arg.DataAccess.ActiveClient.Info.CompanyId > 0)
                {
                    return Arg.DataAccess.ActiveClient.Info.Name;
                }
                return "";
            }

            public static string GetUserFLName(string UserId)
            {
                var currentUser = AspNetUsers.GetAspNetUser(UserId);
                if (currentUser != null && !string.IsNullOrWhiteSpace(currentUser.FullName))
                {
                    return currentUser.FullName;
                }
                return CurrentUserName;
            }
        }


        public static List<string> ImportProgress = new List<string>();
        public static string[] Scopes = { SheetsService.Scope.Spreadsheets, SheetsService.Scope.Drive };
        //public static string ApplicationName = "atlasv1-152711";
        public static string ApplicationName = "level-facility-222705";
        public static UserCredential _cred;

        public static UserCredential UserCredential
        {
            get
            {
                _cred = null;
                var webHostEnvironment = _httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
                string root = Path.Combine(webHostEnvironment.ContentRootPath, "App_Data", "GoogleCredentials");
                string credPath = "";
                UserCredential credential;
                using (var stream =
                    new FileStream(root + "/client_secret_900617941828-2fchcv1fjbssd21mk2ceirho6t9l1n8u.apps.googleusercontent.com.json", FileMode.Open, FileAccess.Read))
                {
                    credPath = root + "/sheets.googleapis.com-dotnet-quickstart.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                    // Create Google Sheets API service.
                    var service = new SheetsService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = ApplicationName,
                    });
                    return credential;
                }
            }
        }

        public static bool IsUserLoggedIn()
        {
            return _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }
        public static string GetCurrentUserLastLogin()
        {
            var currentUserId = CurrentUserId;
            if (currentUserId != null)
            {
                var activeLog = ActivityStats.GetActivitySatByNote(currentUserId, "LoggedIn");
                if (activeLog != null && activeLog.StatsId > 0)
                {
                    return activeLog.AddedOn.ToString(); //.ToString("MM/dd/yyyy");
                }
            }
            return null;
        }
        public static string CurrentUserRole
        {
            get
            {
                var currentUserRole = "";
                if (CurrentUserId.Length <= 0)
                {
                    return "";
                }
                var userRoles = AspNetRoles.GetAspNetUserRole(CurrentUserId);
                if (userRoles != null)
                {
                    currentUserRole = userRoles.RoleName;
                }
                return currentUserRole;
            }
        }
        public static string CurrentUserId
        {
            get
            {
                if (_httpContextAccessor.HttpContext == null)
                {
                    return "";
                }

                if (_httpContextAccessor.HttpContext.User == null)
                {
                    return "";
                }

                IPrincipal currentUser = _httpContextAccessor.HttpContext.User;

                if (currentUser.Identity == null)
                {
                    return "";
                }

                var userId = "35f7710d-ba3e-4e3d-96c0-6013518d8e59";   //currentUser.Identity.GetUserId();
                return userId ?? "";
            }
        }
        public static string MyAppRoot
        {
            get
            {
                return _webHostEnvironment.ContentRootPath;
            }
        }

        //public static string MyAppRoot
        //{
        //    get
        //    {
        //        return HttpContext.Current.Server.MapPath("~/");
        //    }
        //}

        public static string MyRoot
        {
            get
            {
                if (_httpContextAccessor.HttpContext?.Request != null)
                {
                    var request = _httpContextAccessor.HttpContext.Request;
                    return $"{request.Scheme}://{request.Host}{request.PathBase}/";
                }
                return "";
            }
        }
        public static void GenerateReport(string reportPath, List<ReportParameter> reportParameters, string format, string outputPath)
        {
            var reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Remote;

            IReportServerCredentials irsc = new CustomReportCredentials(ReportServerUserName, ReportServerPassword);
            reportViewer.ServerReport.ReportServerCredentials = irsc;
            reportViewer.ServerReport.ReportServerUrl = new Uri(ReportServerUrl);
            reportViewer.ServerReport.ReportPath = reportPath;
            if (reportParameters != null)
                reportViewer.ServerReport.SetParameters(reportParameters);

            byte[] bytes = reportViewer.ServerReport.Render(format);

            var folder = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            using (FileStream fs = new FileStream(outputPath, FileMode.OpenOrCreate))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
        }

        public static string[] StatusOptions = { "Open", "Closed" };
        public static string[] ChargeCodeOperators = { "=", "<>" };
        public static string[] InvoiceTypes = { "BOL Overcharge", "BOL Under-billing" };


        public static List<SelectListItem> SSISReports = new()
        {
            new SelectListItem { Text = "ARG Management Snapshot", Value = "ARG_Management_Snapshot" },
            new SelectListItem { Text = "ARG Pending Balance Dues", Value = "ARG_Pending_Balance_Dues" },
            new SelectListItem { Text = "Revenue By Client Analyst", Value = "RevenueByClientAnalyst" },
            new SelectListItem { Text = "Revenue Analyst Productivity", Value = "Revenue_Analyst_Productivity" }
        };

        public static List<SelectListItem> Mode = new()
        {
            new SelectListItem { Text = "Ocean", Value = "O" },
            new SelectListItem { Text = "Air", Value = "P" }
        };

        public static List<SelectListItem> BDInvoiceStatus = new()
        {
            new SelectListItem { Text = "Invoiced_NR (Invoiced to the client customer but no receivable has been set up in the client’s ledger)", Value = "Invoiced_NR" },
            new SelectListItem { Text = "Invoiced_REC (Invoiced to the client customer, and a receivable has been set up in the client’s ledger)", Value = "Invoiced_REC" },
            new SelectListItem { Text = "Closed (Balance due is closed)", Value = "Closed" }
        };

        public static List<SelectListItem> InvoiceTypeforClientDemo = new()
        {
               new SelectListItem { Text = "BOL Invoice", Value = "BOL Invoice" },
               new SelectListItem { Text = "Per Diem Invoice", Value = "Per Diem Invoice" }
        };

        public static List<SelectListItem> WeightUnits = new()
        {
               new SelectListItem { Text = "Pounds", Value = "Pounds" },
               new SelectListItem { Text = "Kilos", Value = "Kilos" }
        };

        public static List<SelectListItem> StatsBookingFilterList = new()
        {
               new SelectListItem { Text = "Group by SHIPPER, POL, POD", Value = "1" },
               new SelectListItem { Text = "Group by POL, POD", Value = "2" },
               new SelectListItem { Text = "Group by POL", Value = "3" }
        };

        public static List<SelectListItem> HazmatOptions = new()
        {
               new() { Text = "Yes", Value = true.ToString() },
               new() { Text = "No", Value = false.ToString() }
        };

        public static List<SelectListItem> MeasureUnits = new()
        {
               new SelectListItem { Text = "Cubic Feet", Value = "Cubic Feet" },
               new SelectListItem { Text = "Cubic Meters", Value = "Cubic Meters" }
        };

        public static List<SelectListItem> InvoiceTypesCeva = new()
        {
                new SelectListItem { Text = "HBL", Value = "HBL" },
                new SelectListItem { Text = "HAWB", Value = "HAWB" }
        };

        public static List<SelectListItem> ChargableWeightOperator = new()
        {
               new SelectListItem { Text = "<", Value = "<" },
               new SelectListItem { Text = ">", Value = ">" },
               new SelectListItem { Text = "=", Value = "=" }
        };

        public static List<SelectListItem> StatusType = new()
        {
               new SelectListItem { Text = "Open", Value = "1" },
               new SelectListItem { Text = "In Process", Value = "2" },
               new SelectListItem { Text = "Completed", Value = "4" }
        };

        public static List<SelectListItem> Priority = new()
        {
               new SelectListItem { Text = "High", Value = "1" },
               new SelectListItem { Text = "Medium", Value = "2" },
               new SelectListItem { Text = "Low", Value = "3" }
        };

        public static List<SelectListItem> ItemTypes = new()
        {
               new SelectListItem { Text = "Container", Value = "Container" },
               new SelectListItem { Text = "Piece", Value = "Piece" }
        };

        public static List<SelectListItem> StatsFilterList = new()
        {
               new SelectListItem { Text = "Group by Origin/Destination", Value = "1" },
               new SelectListItem { Text = "Group by POL", Value = "2" },
               new SelectListItem { Text = "Group by Shipper", Value = "3" },
               new SelectListItem { Text = "Group by Shipper/Origin/Destination", Value = "4" }
        };

        public static class Contants
        {
            public enum StatusTypeEnum
            {
                Open = 1,
                InProcess = 2,
                Deleted = 3,
                Completed = 4
            }

            public enum PriorityTypeEnum
            {
                High = 1,
                Medium = 2,
                Low = 3
            }

            public enum BOLSorting
            {
                [Description("ARG Balance Due")]
                ARGBalDue = 1,
                [Description("Booking Information")]
                BookingInfo = 2,
                [Description("Booking Item Details")]
                BookingItemDetails = 3,
                [Description("Booking Notes / Remarks Flag")]
                RemarksNotes = 4,
                [Description("BOL Header")]
                BOLHeader = 5,
                [Description("BOL Item Detail")]
                BOLCommodity = 6,
                [Description("Invoice & Payment")]
                InvoiceSummary = 7,
                [Description("BOL Charges")]
                BOLCharges = 8,
                [Description("BOL References")]
                BOLReferences = 9,
                [Description("Tracking")]
                ContEvntHist = 10,
                [Description("Agility Booking Info")]
                AgilityBookingInfo = 11,
                [Description("Shipment Header")]
                AgilityBOLAuditResults = 12,
                [Description("Shipment Container Details")]
                AgilityBOLContainerDetails = 13,
                [Description("Customer Invoicing")]
                AgilitySalesInvoices = 14,
                [Description("Supplier Invoicing")]
                AgilityPurchaseInvoices = 15,
                [Description("Shipment Tracking")]
                AgilityShipmentTrackingDetails = 16,
            }

            public enum ClientHellmanWidgets
            {
                [Description("ARG Balance Due")]
                ARGBalDue = 1,
                [Description("Shipment Journal")]
                ShipmentJournal = 2,
                [Description("House HAWB Air")]
                HouseHAWBAir = 3,
                [Description("Master AWB Air")]
                MasterAWBAir = 4,
            }
        }

        public static bool IsActionAssignedToCurrentRole(string actionName)
        {
            var actions = AppActions.GetActionsAssignedToCurrentRole(CurrentUserRoleId, actionName);
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

        public static void GoToLogin()
        {
            _httpContextAccessor.HttpContext.Response.Redirect(MyRoot + "Account/Login");
        }

        internal static bool CheckActiveClient()
        {
            if (_httpContextAccessor.HttpContext == null || _httpContextAccessor.HttpContext.Session == null || Arg.DataAccess.ActiveClient.Info.CompanyId <= 0)
            {
                GoToLogin();
            }
            return true;
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }

        public class Log
        {
            private static readonly ILogger _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<Common>();

            public static void Error(Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
                _logger.LogError(ex, "An error occurred.");
            }
            public static void Error(string ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
                _logger.LogError(ex);
            }
            public static void Info(string message)
            {
                _logger.LogInformation(message);
            }
        }
    }

    public static class CommonExtension
    {
        public static DateTime ToDateTime(this string value)
        {
            if (value == null)
            {
                return new DateTime(0);
            }
            return DateTime.ParseExact(value, "MM-dd-yyyy", CultureInfo.CurrentCulture);
        }

        public static string ToDefaultStartDate(this DateTime value)
        {
            if (value == null || value == DateTime.MinValue || value == new DateTime(2001, 1, 1))
            {
                return new DateTime(2000, 1, 1).ToString("yyyy/MM/dd");
            }
            return value.ToString("yyyy/MM/dd");
        }

        public static string ToDefaultEndDate(this DateTime value)
        {
            if (value == null || value == DateTime.MinValue || value == new DateTime(2001, 1, 1))
            {
                return new DateTime(2099, 12, 31).ToString("yyyy/MM/dd");
            }
            return value.ToString("yyyy/MM/dd");
        }
    }

    public class CustomReportCredentials : IReportServerCredentials
    {
        private string _userName;
        private string _passWord;

        public CustomReportCredentials(string userName, string passWord)
        {
            _userName = userName;
            _passWord = passWord;
        }

        public WindowsIdentity ImpersonationUser => null;
        public ICredentials NetworkCredentials => new NetworkCredential(_userName, _passWord);
        public bool GetFormsCredentials(out Cookie authCookie, out string userName, out string password, out string authority)
        {
            throw new NotImplementedException();
        }
    }
}
