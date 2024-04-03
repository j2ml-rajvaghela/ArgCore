using Arg.DataModels;
using CacheManager.Core;
using CustomExtensions;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Arg.DataAccess
{
    public class ShipmentJournalImpl
    {
        private string _dbName = Common.DBName;

        public ShipmentJournal GetShipment(string shipmentNo)
        {
            const string query = @"SELECT * FROM ShipmentJournal
	                               WHERE (Shipment_No = @ShipmentNo OR @ShipmentNo = '');";
               
            using (var connection = Common.ClientDatabase)
            {
                var shipment = connection.QueryFirstOrDefault<ShipmentJournal>(query, new { @ShipmentNo = shipmentNo });
                return shipment;
            }
        }

        public List<ShipmentJournal> GetDistinctShipmentType()
        {
            const string query = @"SELECT DISTINCT CONCAT(s.Shipment_Type,' ', d.XRefDescription) AS ShipmentTypeDescription, s.Shipment_Type
                                   FROM ShipmentJournal s
                                   LEFT JOIN Descriptions d ON d.XrefCode = s.Shipment_Type AND d.XrefFieldName='ShipmentType'
                                   ORDER BY ShipmentTypeDescription;";

            using (var connection = Common.ClientDatabase)
            {
                var distinctShipmentType = connection.Query<ShipmentJournal>(query, commandType: CommandType.Text).ToList();
                return distinctShipmentType;
            }
        }

        public List<ShipmentJournal> GetDistinctIssuingDept()
        {
            const string query = @"SELECT DISTINCT Issuing_Dept
                                   FROM ShipmentJournal 
                                   WHERE Issuing_Dept <> ''
                                   ORDER BY Issuing_Dept;";

            using (var connection = Common.ClientDatabase)
            {
                var distinctIssuingDept = connection.Query<ShipmentJournal>(query, commandType: CommandType.Text).ToList();
                return distinctIssuingDept;
            }
        }

        public List<ShipmentJournal> GetDistinctShipmentStatus()
        {
            const string query = @"SELECT DISTINCT CONCAT(s.Shipment_Status,' ', d.XRefDescription) AS  ShipmentStatusDescription, s.Shipment_Status
                                   FROM ShipmentJournal s
                                   LEFT JOIN Descriptions d ON d.XrefCode = s.Shipment_Status AND d.XrefFieldName='ShipmentStatus'
                                   ORDER BY ShipmentStatusDescription;";

            using (var connection = Common.ClientDatabase)
            {
                var distinctShipmentStatus = connection.Query<ShipmentJournal>(query, commandType: CommandType.Text).ToList();
                return distinctShipmentStatus;
            }
        }

        public List<ShipmentJournal> GetDistinctShipmentCLStatus()
        {
            const string query = @"SELECT DISTINCT CONCAT(s.Shipment_CL_Status,' ', d.XRefDescription) AS  ShipmentCLStatusDescription, s.Shipment_CL_Status
                                   FROM ShipmentJournal s
                                   LEFT JOIN Descriptions d ON d.XrefCode = s.Shipment_CL_Status AND d.XrefFieldName='ShipmentCLStatus'
                                   ORDER BY ShipmentCLStatusDescription;";

            using (var connection = Common.ClientDatabase)
            {
                var distinctShipmentCLStatus = connection.Query<ShipmentJournal>(query, commandType: CommandType.Text).ToList();
                return distinctShipmentCLStatus;
            }
        }

        public List<ShipmentJournal> GetDistinctRegion()
        {
            int companyId = ActiveClient.Info.CompanyId;

            string query = $"SELECT DISTINCT  CONCAT(s.Region,' ', r.Description) AS  RegionDescription, s.Region " +
                           $"FROM ShipmentJournal s" +
                           $"LEFT JOIN argocean.dbo.Regions r ON s.Region=r.Region AND r.CompanyId = {companyId}" +
                           $"WHERE s.Region <> ''" +
                           $"ORDER BY Region;";

            using (var connection = Common.ClientDatabase)
            {
                var distinctRegion = connection.Query<ShipmentJournal>(query).ToList();
                return distinctRegion;
            }
        }

        public List<ShipmentJournal> GetDistinctOrigin()
        {
            const string query = @"SELECT DISTINCT Origin FROM ShipmentJournal 
                                   WHERE Origin <> ''
                                   ORDER BY Origin;";

            using (var connection = Common.ClientDatabase)
            {
                var distinctOrigin = connection.Query<ShipmentJournal>(query, commandType: CommandType.Text).ToList();
                return distinctOrigin;
            }

        }

        public List<ShipmentJournal> GetDistinctDestination()
        {
            const string query = @"SELECT DISTINCT Dest FROM ShipmentJournal 
                                   WHERE Dest <> ''
                                   ORDER BY Dest;";

            using (var connection = Common.ClientDatabase)
            {
                var distinctDestination = connection.Query<ShipmentJournal>(query, commandType: CommandType.Text).ToList();
                return distinctDestination;
            }

        }

        //public MasterAWB GetMasterAWB(string AWBBLNo, string MAWBBLNo, string shipmentDate)
        //{
        //     var query = @"SELECT * FROM MasterAWB WHERE";
        //    if (!string.IsNullOrEmpty(AWBBLNo) && !string.IsNullOrEmpty(MAWBBLNo))
        //    {
        //        query += $" (awb_nr= {AWBBLNo} OR awb_nr=@{MAWBBLNo}) AND  (SELECT DATEDIFF(day, flown_date, '{shipmentDate} ') AS DateDifference) <= 90";
        //    }
        //    else if (!string.IsNullOrEmpty(AWBBLNo))
        //    {
        //        query += $" AND awb_nr= {AWBBLNo} AND (SELECT DATEDIFF(day, flown_date, '{shipmentDate} ') AS DateDifference) <= 90";
        //    }
        //    else if (!string.IsNullOrEmpty(MAWBBLNo))
        //    {
        //        query += $"AND awb_nr = {MAWBBLNo} AND (SELECT DATEDIFF(day, flown_date, '{shipmentDate}') AS DateDifference) <= 90";
        //    }

        //    using (var connection = Common.ClientDatabase)
        //    {
        //        var masterAWB = connection.QueryFirstOrDefault<MasterAWB>(query);
        //        return masterAWB;
        //    }
        //}

        public HouseHAWBAir GetHouseHAWBAir(string AWBBLNo, string MAWBBLNo, string shipmentDate)
        {
            string query = "SELECT * FROM HouseHAWBAir WHERE 1=1";

            if (!string.IsNullOrEmpty(AWBBLNo) && !string.IsNullOrEmpty(MAWBBLNo))
            {
                query += " AND (awb_nr = @AWBBLNo OR m_awb_nr = @MAWBBLNo)";
            }
            else if (!string.IsNullOrEmpty(AWBBLNo))
            {
                query += " AND awb_nr = @AWBBLNo";
            }
            else if (!string.IsNullOrEmpty(MAWBBLNo))
            {
                query += " AND m_awb_nr = @MAWBBLNo";
            }

            query += " AND DATEDIFF(day, flown_date, @ShipmentDate) <= 90";

            using (var connection = Common.ClientDatabase)
            {
                var parameters = new { AWBBLNo, MAWBBLNo, ShipmentDate = shipmentDate };
                var houseHAWBAir = connection.QueryFirstOrDefault<HouseHAWBAir>(query, parameters);
                return houseHAWBAir;
            }
        }

        public MasterAWB GetMasterAWB(string AWBBLNo, string MAWBBLNo, string shipmentDate)
        {
            string query = "SELECT * FROM MasterAWB WHERE 1=1";

            if (!string.IsNullOrEmpty(AWBBLNo) && !string.IsNullOrEmpty(MAWBBLNo))
            {
                query += " AND (awb_nr = @AWBBLNo OR awb_nr = @MAWBBLNo)";
            }
            else if (!string.IsNullOrEmpty(AWBBLNo))
            {
                query += " AND awb_nr = @AWBBLNo";
            }
            else if (!string.IsNullOrEmpty(MAWBBLNo))
            {
                query += " AND awb_nr = @MAWBBLNo";
            }

            query += " AND DATEDIFF(day, flown_date, @ShipmentDate) <= 90";

            using (var connection = Common.ClientDatabase)
            {
                var parameters = new { AWBBLNo, MAWBBLNo, ShipmentDate = shipmentDate };
                var masterAWB = connection.QueryFirstOrDefault<MasterAWB>(query, parameters);
                return masterAWB;
            }
        }


        public List<HellmannDocumentImages> GetDocumentImage(string shipmentNo, string shipmentDate, int companyId)
        {
            const string query = @"SELECT * FROM DocumentImages i
                                   WHERE i.fileName <> '' 
                                   AND (RefNo=@ShipmentNo AND CompanyID=@CompanyId AND RefDate=@ShipmentDate)
                                   ORDER BY i.Loaded DESC;";

            using (var connection = Common.ClientDatabase)
            {
                var documentImages = connection.Query<HellmannDocumentImages>(query, new { @ShipmentNo = shipmentNo, @CompanyId = companyId, @ShipmentDate = shipmentDate }).ToList();
                return documentImages;
            }
        }

        private ICacheManager<List<ShipmentJournal>> _manager = CacheFactory.Build<List<ShipmentJournal>>(Arg.Core.Settings.DefaultCacheSettings);

        public ShipmentJournal GetResult(int queryId, int idx)
        {
            var results = GetResults(queryId, "");
            var bol = results.FirstOrDefault(x => x.Idx == idx) ?? new ShipmentJournal();
            bol.ResultCount = results.Count;
            return bol;
        }

        public virtual List<ShipmentJournal> GetResults(int queryId, string type)
        {
            var key = "cached-" + queryId;
            var results = _manager.Get(key);
            if (results == null || !results.Any())
            {
                var qr = new QueryResultsImpl().GetQueryResults(queryId);
                var searchOptions = Newtonsoft.Json.JsonConvert.DeserializeObject<SearchOptions>(qr.QueryJson);
                if (type == "Stats")
                {
                    results = GetShipmentResultStats(searchOptions, ActiveClient.Info.Name);
                    return results;
                }
                else if (type == "StatsByOrigin")
                {
                    //results = GetAuditResultStatsByOrigin(searchOptions, ActiveClient.Info.Name);
                    return results;
                }
                else if (type == "StatsByShipper")
                {
                    //results = GetAuditResultStatsByShipper(searchOptions, ActiveClient.Info.Name);
                    return results;
                }
                else if (type == "StatsByPOL")
                {
                    //results = GetAuditResultStatsByPOL(searchOptions, ActiveClient.Info.Name);
                    return results;
                }
                else if (qr.SqlQuery != null && type == "table")
                {
                    results = GetResults(null, qr.SqlQuery, true);
                    return results;
                }
                else if (type == "table")
                {
                    results = GetResults(searchOptions, true);
                    return results;
                }
                else
                {
                    results = GetResults(searchOptions);
                }
                   
                _manager.Add(key, results);
            }
            return results;
        }

        public List<ShipmentJournal> GetResults(SearchOptions so, bool Table = false)
        {
            if (Table)
            {
                return GetResults(so, null, true);
            }
            return GetResults(so, null);
        }

        public List<ShipmentJournal> GetResults(SearchOptions so, string sqlCmd, bool Table = false)
        {
            var sql = "";
            var sortingOrder = "ORDER BY ";

            if (sqlCmd == null)
            {
                foreach (var sort in so.SortOptions.Where(x => x.IsSelected).OrderBy(x => x.Idx))
                {
                    if (sort.IsDesc)
                    {
                        sortingOrder += sort.ColumnName + " DESC,";
                    }
                    else
                    {
                        sortingOrder += sort.ColumnName + ",";
                    }
                }
                sortingOrder = sortingOrder.Remove(sortingOrder.LastIndexOf(","));

                var columns = @"CONCAT(s.Region,' ', r.Description) AS RegionDescription,CONCAT(s.Shipment_Status,' ', d2.XRefDescription) AS ShipmentStatusDescription,
                                CONCAT(s.Shipment_Type,' ', d1.XRefDescription) AS ShipmentTypeDescription,CONCAT(s.Shipment_CL_Status,' ', d3.XRefDescription) AS ShipmentCLStatusDescription,s.*";

                if (Table)
                {
                    columns += $",(SELECT TOP(1) ast.EventType AS BOLViewed FROM {_dbName}.dbo.ActivityStats ast  WHERE ast.BolNo=s.Shipment_No AND " +
                               $"ast.EventType='BOLViewed' AND ast.ClientId = {so.CompanyId}) AS BOLViewed";
                }
                sql += $"SELECT b.* FROM (SELECT b.*,ROW_NUMBER() OVER ({sortingOrder}) AS Idx FROM(SELECT {columns} FROM ShipmentJournal s LEFT JOIN argocean.dbo.Regions r ON s.Region=r.Region " +
                       $"LEFT JOIN Descriptions d1 ON s.Shipment_Type = d1.XrefCode AND d1.XrefFieldName = 'ShipmentType' " +
                       $"LEFT JOIN Descriptions d2 ON s.Shipment_Status = d2.XrefCode AND d2.XrefFieldName = 'ShipmentStatus' " +
                       $"LEFT JOIN Descriptions d3 ON s.Shipment_CL_Status = d3.XrefCode AND d3.XrefFieldName = 'ShipmentCLStatus') AS b";

                BuildCmdWhereCondition(ref sql, so, Table);
                sql += ") AS b";
                if (so.CompanyId > 0)
                {
                    ActiveClient.Set(so.CompanyId);
                }
            }
            else
            {
                sql = sqlCmd;
            }

            using (var connection = Common.ClientDatabase)
            {
                var results = connection.Query<ShipmentJournal>(sql).ToList();
                return results;
            }
        }

        public List<ShipmentJournal> GetShipmentResultStats(SearchOptions so, string clientName)
        {
            var cmd = @"Select DISTINCT b.region,CONCAT(b.Region,' ', r.Description) AS RegionDescription,b.Origin, COUNT(b.Shipment_No) AS ShipmentCount 
		                       FROM ClientHellmann.dbo.ShipmentJournal b LEFT JOIN argocean.dbo.Regions r ON b.Region=r.Region 
                               LEFT JOIN ClientHellmann.dbo.HouseHAWBAir ha ON (ha.awb_nr =b.AWB_BL_No  OR ha.m_awb_nr  = b.M_AWB_BL_No) AND abs(datediff(d,ha.flown_date , b.Shipment_Date))<=90 
                               LEFT JOIN ClientHellmann.dbo.MasterAWB ma ON (ma.awb_nr =b.AWB_BL_No  OR ma.awb_nr  = b.M_AWB_BL_No) AND abs(datediff(d,ma.flown_date , b.Shipment_Date))<=90";

            BuildCmdWhereCondition(ref cmd, so, false);
            cmd += @"GROUP BY  b.region, b.Origin,CONCAT(b.Region,' ', r.Description) 
                     ORDER BY ShipmentCount desc";

            using (var connection = Common.ClientDatabase)
            {
                var resultStats = connection.Query<ShipmentJournal>(cmd, commandTimeout: 1200).ToList();
                return resultStats ?? new List<ShipmentJournal>();
            }
        }

        public void BuildCmdWhereCondition(ref string cmd, SearchOptions so, bool Table = false)
        {
            if (so.IssuingDepts != null && so.IssuingDepts.Any())
            {
                string list = Core.Utility.JoinStrings(so.IssuingDepts.ToArray());
                cmd += $" AND b.Issuing_Dept IN ({list})";
            }

            if (!string.IsNullOrEmpty(so.ShipmentNo))
            {
                cmd += $" AND b.Shipment_No LIKE '%{so.ShipmentNo}%'";
            }

            if (!string.IsNullOrEmpty(so.AWBBLNo))
            {
                cmd += $" AND b.AWB_BL_No LIKE '%{so.AWBBLNo}%' OR b.M_AWB_BL_No LIKE '%{so.AWBBLNo}%'";
            }

            var ShipmentStartDate = so.ShipmentStartDate.ToDateTime();
            var ShipmentEndDate = so.ShipmentEndDate.ToDateTime();

            if (ShipmentStartDate != DateTime.MinValue && ShipmentEndDate != DateTime.MinValue)
            {
                string strShipmentStartDate = ShipmentStartDate.ToString("yyyy-MM-dd");
                string strShipmentEndDate = ShipmentEndDate.ToString("yyyy-MM-dd");
                cmd += $" AND b.Shipment_Date BETWEEN {strShipmentStartDate} AND {strShipmentEndDate}";
            }

            if (so.ShipmentType != null && so.ShipmentType.Any())
            {
                string list = Core.Utility.JoinStrings(so.ShipmentType.ToArray());
                cmd += $" AND b.Shipment_Type IN ({list})";
            }

            if (so.ShipmentStatus != null && so.ShipmentStatus.Any())
            {
                string list = Core.Utility.JoinStrings(so.ShipmentStatus.ToArray());
                cmd += $" AND b.Shipment_Status IN ({list})";
            }

            if (so.ShipmentCLStatus != null && so.ShipmentCLStatus.Any())
            {
                string list = Core.Utility.JoinStrings(so.ShipmentCLStatus.ToArray());
                cmd += $" AND b.Shipment_CL_Status IN ({list})";
            }

            if (so.Regions != null && so.Regions.Any())
            {
                string list = Core.Utility.JoinStrings(so.Regions.ToArray());
                cmd += $" AND b.Region IN ({list})";
            }

            if (so.Origin != null && so.Origin.Any())
            {
                string list = Core.Utility.JoinStrings(so.Origin.ToArray());
                cmd += $" AND b.Origin IN ({list})";
            }

            if (so.Dest != null && so.Dest.Any())
            {
                string list = Core.Utility.JoinStrings(so.Dest.ToArray());
                cmd += $" AND b.Dest IN ({list})";
            }

            if (so.MinChargeableWeight > 0 && so.MaxChargeableWeight > 0)
            {
                cmd += $" AND b.Chargeable_Weight BETWEEN {so.MinChargeableWeight} AND {so.MaxChargeableWeight}";
            }
            else if (so.MinChargeableWeight > 0)
            {
                cmd += $" AND b.Chargeable_Weight >  {so.MinChargeableWeight}";
            }
            else if (so.MaxChargeableWeight > 0)
            {
                cmd += $" AND b.Chargeable_Weight <  {so.MaxChargeableWeight}";
            }

            if (so.FLInput1 != null)
            {
                cmd += $" AND (b.Actual_Revenue_CCR + b.Accrual_Revenue_Total_CCR) - (b.Actual_Cost_CCR + b.Accrual_Cost_Total_CCR) {so.FLSign1} {so.FLInput1}";
            }

            if (so.FLInput2 != null)
            {
                cmd += $" AND (b.Actual_Revenue_CCR + b.Accrual_Revenue_Total_CCR) {so.FLSign2} {so.FLInput2}";
            }

            if (so.FLInput3 != null)
            {
                cmd += $" AND b.Chargeable_Weight {so.FLSign3} {so.FLInput3}";
            }

            if (so.FLInput4 != null)
            {
                cmd += $" AND (b.Chargeable_Weight - b.Actual_Weight_kgs) {so.FLSign4} {so.FLInput4}";
            }

            if (so.EliminateBDResearchItems)
            {
                AppendConditionOperator(ref cmd, Table);
                cmd += $" AND b.Shipment_No NOT IN (SELECT d.Bol FROM {_dbName}.dbo.BalanceDues d WHERE d.CompanyId={so.CompanyId} UNION " +
                       $"Select r.Bol from {_dbName}.dbo.ResearchItems r where r.CompanyId={so.CompanyId}";
            }

            if (!string.IsNullOrWhiteSpace(so.BOLViews))
            {
                AppendConditionOperator(ref cmd, Table);
                cmd += $" AND (SELECT COUNT(*) AS ViewCount FROM {_dbName}.dbo.ActivityStats st WHERE";

                if (so.UserIds.Any() && so.UserIds[0] != "00000000-0000-0000-0000-000000000000")
                {
                    string list = Arg.Core.Utility.JoinStrings(so.UserIds.ToArray());
                    cmd += $" st.userId IN ({list}) AND";
                }
                cmd += $" st.BOLNo=b.Shipment_No and st.ClientID = {so.CompanyId} AND EventType='BOLViewed') <= {Convert.ToInt32(so.BOLViews)}";
            }
        }

        public void AppendConditionOperator(ref string cmd, bool Table = false)
        {
            if (Table)
            {
                if (Table)
                {
                    if (cmd.IndexOf("b) AS b\n WHERE", StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                        cmd.IndexOf("b) AS b\n WHERE", StringComparison.CurrentCulture) > 0 ||
                        cmd.IndexOf("P.Type='Customer'\n WHERE", StringComparison.CurrentCulture) > 0 ||
                        cmd.IndexOf("P.Type='Customer'\n WHERE", StringComparison.CurrentCulture) > 0)
                    {
                        cmd += " AND ";
                    }
                    else
                    {
                        cmd += " WHERE ";
                    }
                }
                else
                {
                    if (cmd.IndexOf("WHERE", StringComparison.CurrentCultureIgnoreCase) >= 0)
                    {
                        cmd += " AND ";
                    }
                    else
                    {
                        cmd += " WHERE ";
                    }
                }
            }
        }
    }
}
