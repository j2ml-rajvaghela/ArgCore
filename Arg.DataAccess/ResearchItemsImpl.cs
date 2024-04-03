using Arg.DataModels;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace Arg.DataAccess
{
    public class ResearchItemsImpl
    {
        private string _clientDbName = Common.ClientsDBName;
        public List<ResearchItems> GetResearchReasonCodes(int companyId)
        {
            var parameters = new DynamicParameters();
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }
            using (var connection = Common.Database)
            { 
                var researchReasonCodes = connection.Query<ResearchItems>("GetResearchReasonCodes", parameters, commandType: CommandType.StoredProcedure).ToList();
                return researchReasonCodes;
            }
        }

        public List<ResearchItems> GetResearchItems(string currentUserId, int companyId, string bolNo = "")
        {
            var parameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(currentUserId))
            {
                parameters.Add("@CurrentUserId", currentUserId, DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(bolNo))
            {
                parameters.Add("@BolNo", bolNo, DbType.String);
            }
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }
            parameters.Add("@ClientDBName", _clientDbName, DbType.String);
            using (var connection = Common.Database)
            {
                var researchItems = connection.Query<ResearchItems>("GetResearchItemsByBOLNo", parameters, commandType: CommandType.StoredProcedure).ToList();
                return researchItems;
            }
        }

        public List<ResearchItems> GetResearchItems(SearchOptions so)
        {
            var parameters = new DynamicParameters();
            if (so.Regions != null)
            {
                string regionsString = string.Join(",", so.Regions);
                parameters.Add("@Regions", regionsString, DbType.String);
            }
            if (so.RevenueAnalystAuditors != null)
            {
                string revenueAnalystAuditorsString = string.Join(",", so.RevenueAnalystAuditors);
                parameters.Add("@RevenueAnalystAuditors", revenueAnalystAuditorsString, DbType.String);
            }
            if (so.SelectedStatus != null)
            {
                string selectedStatusString = string.Join(",", so.SelectedStatus);
                parameters.Add("@SelectedStatus", selectedStatusString, DbType.String);
            }
            if (so.ResearchReasonCodes != null)
            {
                string researchReasonCodesString = string.Join(",", so.ResearchReasonCodes);
                parameters.Add("@ResearchReasonCodes", researchReasonCodesString, DbType.String);
            }
            if (so.CompanyId > 0)
            {
                parameters.Add("@CompanyId", so.CompanyId, DbType.Int32);
            }
            if (!string.IsNullOrWhiteSpace(so.Bol))
            {
                parameters.Add("@BOL", so.Bol, DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(so.BookingId))
            {
                parameters.Add("@BookingId", so.BookingId, DbType.String);
            }
            if (so.BolExecutionStartDate != DateTime.MinValue)
            {
                var bolStartDateFormatted = so.BolExecutionStartDate.ToString("yyyy-MM-dd");
                parameters.Add("@BolExecutionStartDate", bolStartDateFormatted, DbType.DateTime2);
            }
            if (so.BolExecutionEndDate != DateTime.MinValue)
            {
                var bolEndDateFormatted = so.BolExecutionEndDate.Date.ToString("yyyy-MM-dd");
                parameters.Add("@BolExecutionEndDate", bolEndDateFormatted, DbType.Date);
            }
            if (so.LastModifiedStartDate != DateTime.MinValue)
            {
                var lastModStartDateFormatted = so.LastModifiedStartDate.ToString("yyyy-MM-dd");
                parameters.Add("LastModifiedStartDate", lastModStartDateFormatted);
            }
            if (so.LastModifiedEndDate != DateTime.MinValue)
            {
                var lastModEndDateFormatted = so.LastModifiedEndDate.Date.ToString("yyyy-MM-dd");
                parameters.Add("LastModifiedEndDate", lastModEndDateFormatted);
            }
            if (!string.IsNullOrWhiteSpace(so.UserId))
            {
                parameters.Add("@UserId", so.UserId, DbType.String);
            }
            parameters.Add("@ClientDBName", _clientDbName, DbType.String);

            using (var connection = Common.Database)
            { 
                var researchItems = connection.Query<ResearchItems>("GetResearchItems", parameters, commandType: CommandType.StoredProcedure).ToList();
                return researchItems;
            }
        }

        public List<ResearchItems> GetResearchItemsCeva(SearchOptions so)
        {
            var parameters = new DynamicParameters();
            if (so.Regions != null)
            {
                string regionsString = string.Join(",", so.Regions);
                parameters.Add("@Regions", regionsString, DbType.String);
            }
            if (so.RevenueAnalystAuditors != null)
            {
                string revenueAnalystAuditorsString = string.Join(",", so.RevenueAnalystAuditors);
                parameters.Add("@RevenueAnalystAuditors", revenueAnalystAuditorsString, DbType.String);
            }
            if (so.SelectedStatus != null)
            {
                string selectedStatusString = string.Join(",", so.SelectedStatus);
                parameters.Add("@SelectedStatus", selectedStatusString, DbType.String);
            }
            if (so.ResearchReasonCodes != null)
            {
                string researchReasonCodesString = string.Join(",", so.ResearchReasonCodes);
                parameters.Add("@ResearchReasonCodes", researchReasonCodesString, DbType.String);
            }
            if (so.CompanyId > 0)
            {
                parameters.Add("@CompanyId", so.CompanyId, DbType.Int32);
            }
            if (!string.IsNullOrWhiteSpace(so.Bol))
            {
                parameters.Add("@Bol", so.Bol, DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(so.BookingId))
            {
                parameters.Add("@BookingId", so.Bol, DbType.String);
            }
            if (so.BolExecutionStartDate != DateTime.MinValue)
            {
                var bolStartDateFormatted = so.BolExecutionStartDate.ToString("yyyy-MM-dd");
                parameters.Add("@BolExecutionStartDate", bolStartDateFormatted, DbType.DateTime2);
            }
            if (so.BolExecutionStartDate != DateTime.MinValue)
            {
                var bolEndDateFormatted = so.BolExecutionEndDate.ToString("yyyy-MM-dd");
                parameters.Add("@BolExecutionEndDate", bolEndDateFormatted, DbType.DateTime2);
            }
            if (so.LastModifiedStartDate != DateTime.MinValue)
            {
                var lastModStartDateFormatted = so.LastModifiedStartDate.ToString("yyyy-MM-dd");
                parameters.Add("@LastModifiedStartDate", lastModStartDateFormatted, DbType.DateTime2);
            }
            if (so.LastModifiedEndDate != DateTime.MinValue)
            {
                var lastModEndDateFormatted = so.LastModifiedEndDate.Date.ToString("yyyy-MM-dd");
                parameters.Add("@LastModifiedEndDate", lastModEndDateFormatted, DbType.DateTime2);
            }
            if (!string.IsNullOrWhiteSpace(so.UserId))
            {
                parameters.Add("@UserId", so.UserId, DbType.String);
            }
            parameters.Add("@ClientDBName", ActiveClient.Info.DBName, DbType.String);
            using (var connection = Common.Database)
            {
                var researchItemsCeva = connection.Query<ResearchItems>("GetResearchItemsCeva", parameters, commandType: CommandType.StoredProcedure).ToList();
                return researchItemsCeva;
            }
        }

        public List<ResearchItems> GetAuditingResearchItemsCeva(string currentUserId, int companyId, string bolNo = "")
        {
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(currentUserId))
            {
                parameters.Add("@RevenueAnalystAuditor", currentUserId, DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(bolNo))
            {
                parameters.Add("@BOL", bolNo, DbType.String);
            }
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }
            parameters.Add("@ClientDBName", ActiveClient.Info.DBName, DbType.String);
            using (var connection = Common.Database)
            {
                var researchItemsCeva = connection.Query<ResearchItems>("GetAuditingResearchItemsCeva", parameters, commandType: CommandType.StoredProcedure).ToList();
                return researchItemsCeva;
            }
        }

        public ResearchItems GetResearchItem(int researchId, int companyId, string bolNo = "", string region = "", DateTime? bolExecDate = null)
        {
            var parameters = new DynamicParameters();
            if (researchId > 0)
            {
                parameters.Add("@ResearchId", researchId, DbType.Int32);
            }
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }
            if (!string.IsNullOrWhiteSpace(bolNo))
            {
                parameters.Add("@BolNo", bolNo, DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(region))
            {
                parameters.Add("@Region", region, DbType.String);
            }
            if (bolExecDate != null && bolExecDate != DateTime.MinValue)
            {
                parameters.Add("@BolExecDate", bolExecDate, DbType.DateTime);
            }
            using (var connection = Common.ClientDatabase)
            { 
                var researchItem = connection.QueryFirstOrDefault<ResearchItems>("GetResearchItem", parameters, commandType: CommandType.StoredProcedure);
                return researchItem;
            }
        }

        public void SaveResearchItem(ResearchItems researchItems)
        {
            if (string.IsNullOrWhiteSpace(researchItems.Region))
            {
                throw new Exception("Region can't be empty.");
            }
            using (var connection = Common.ClientDatabase)
            {      
                if (researchItems.ResearchId == 0)
                {
                    connection.Insert(researchItems);
                }
                else
                {
                    connection.Update(researchItems);
                }
            }
        }

        public int DeleteResearchItem(int researchId)
        {
            const string query = "DELETE FROM ResearchItems WHERE ResearchId=@ResearchId";
            using (var connection = Common.Database)
            {
                var result = connection.Execute(query, new { @ResearchId = researchId });
                return result;
            }
        }
    }
}
