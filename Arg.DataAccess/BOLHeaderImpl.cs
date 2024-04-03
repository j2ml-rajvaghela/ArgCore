using Arg.DataModels;
using CacheManager.Core;
using CustomExtensions;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;
using System.Drawing;
using System.Linq;

namespace Arg.DataAccess
{
    public class BOLHeaderImpl
    {
        private string _dbName = Common.DBName;
        public List<BOLHeader> GetDistinctShipper()
        {
            using (var connection = Common.ClientDatabase)
            {
                var distinctShippers = connection.Query<BOLHeader>("GetDistinctShipper", commandType: CommandType.StoredProcedure).ToList();
                return distinctShippers;
            }
        }

        public BOLHeader GetBOLHeader(string bolNo)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@BOL#", bolNo, DbType.String);

            using (var connection = Common.ClientDatabase)
            {
                var bOLHeader = connection.QueryFirstOrDefault<BOLHeader>("GetBOLHeaderByBolNo", parameters, commandType: CommandType.StoredProcedure);
                return bOLHeader;
            }
        }

        public List<BOLHeader> GetAgilityDistinctShipper()
        {
            using (var connection = Common.ClientDatabase)
            {
                var distinctShippers = connection.Query<BOLHeader>("GetAgilityDistinctShipper", commandType: CommandType.StoredProcedure).ToList();
                return distinctShippers;
            }
        }

        public List<BalanceDues_Item> GetBalanceDueItems(string bolNo, int companyId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@BOL", bolNo, DbType.String);
            parameters.Add("@CompanyId", companyId, DbType.Int32);

            using (var connection = Common.Database)
            {
                var balanceDueItems = connection.Query<BalanceDues_Item>("GetBalanceDueItems", parameters, commandType: CommandType.StoredProcedure).ToList();
                return balanceDueItems;
            }
        }

        public List<BOLHeader> GetDistinctPOL()
        {
            using (var connection = Common.ClientDatabase)
            {
                var distinctPOL = connection.Query<BOLHeader>("GetDistinctPOL", commandType: CommandType.StoredProcedure).ToList();
                return distinctPOL;
            }
        }

        public List<BOLHeader> GetDistinctPOD()
        {
            using (var connection = Common.ClientDatabase)
            {
                var distinctPOD = connection.Query<BOLHeader>("GetDistinctPOD", commandType: CommandType.StoredProcedure).ToList();
                return distinctPOD;
            }
        }

        public List<BOLHeader> GetDistinctOrigin()
        {
            using (var connection = Common.ClientDatabase)
            {
                var distinctOrigin = connection.Query<BOLHeader>("GetDistinctOrigin", commandType: CommandType.StoredProcedure).ToList();
                return distinctOrigin;
            }
        }

        public List<BOLHeader> GetDistinctDestination()
        {
            using (var connection = Common.ClientDatabase)
            {
                var distinctDestination = connection.Query<BOLHeader>("GetDistinctDestination", commandType: CommandType.StoredProcedure).ToList();
                return distinctDestination;
            }
        }

        public List<BOLHeader> GetDistinctModes()
        {
            using (var connection = Common.ClientDatabase)
            {
                var distinctModes = connection.Query<BOLHeader>("GetDistinctModes", commandType: CommandType.StoredProcedure).ToList();
                return distinctModes;
            }
        }

        public List<BOLHeader> GetDistinctPayor()
        {
            using (var connection = Common.ClientDatabase)
            {
                var distinctpayor = connection.Query<BOLHeader>("GetDistinctPayor", commandType: CommandType.StoredProcedure).ToList();
                return distinctpayor;
            }
        }

        public List<BOLHeader> GetDistinctConsignee()
        {
            using (var connection = Common.ClientDatabase)
            {
                var distinctConsignee = connection.Query<BOLHeader>("GetDistinctConsignee", commandType: CommandType.StoredProcedure).ToList();
                return distinctConsignee;
            }
        }

        public List<Generic> GetBOLCustomers()
        {
            using (var connection = Common.ClientDatabase)
            {
                var bOLCustomers = connection.Query<Generic>("GetBOLCustomers", commandType: CommandType.StoredProcedure).ToList();
                return bOLCustomers;
            }
        }

        public List<Modes> GetAllModes()
        {
            const string query = @"SELECT * FROM Modes;";

            using (var connection = Common.ClientDatabase)
            {
                var modes= connection.Query<Modes>(query, commandType: CommandType.Text).ToList();
                return modes;
            }
        }

        public List<Participants> GetAllParticipants()
        {
            const string query = @"SELECT * FROM Participants;";

            using (var connection = Common.ClientDatabase)
            {
                var participants = connection.Query<Participants>(query, commandType: CommandType.Text).ToList();
                return participants;
            }
        }

        private ICacheManager<List<BOLHeader>> _manager = CacheFactory.Build<List<BOLHeader>>(Core.Settings.DefaultCacheSettings);

        public BOLHeader GetResult(int queryId, int idx)
        {
            var results = GetResults(queryId, "");
            var bol = results.FirstOrDefault(x => x.Idx == idx) ?? new BOLHeader();
            bol.ResultCount = results.Count;
            return bol;
        }

        public virtual List<BOLHeader> GetResults(int queryId, string type)
        {
            var key = "cached-" + queryId;
            var results = _manager.Get(key);
            if (results == null || !results.Any())
            {
                var qr = new QueryResultsImpl().GetQueryResults(queryId);
                var searchOptions = Newtonsoft.Json.JsonConvert.DeserializeObject<SearchOptions>(qr.QueryJson);
                if (type == "Stats")
                {
                    results = GetAuditResultStats(searchOptions, ActiveClient.Info.Name);
                    return results;
                }
                else if (type == "StatsByOrigin")
                {
                    results = GetAuditResultStatsByOrigin(searchOptions, ActiveClient.Info.Name);
                    return results;
                }
                else if (type == "StatsByShipper")
                {
                    results = GetAuditResultStatsByShipper(searchOptions, ActiveClient.Info.Name);
                    return results;
                }
                else if (type == "StatsByPOL")
                {
                    results = GetAuditResultStatsByPOL(searchOptions, ActiveClient.Info.Name);
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

        public void FixWildcards(ref SearchOptions so)
        {
            so.ShipperReferenceNumber = Common.WildCardSearchToNormal(so.ShipperReferenceNumber);
            so.ForwarderReferenceNumber = Common.WildCardSearchToNormal(so.ForwarderReferenceNumber);
            so.ConsigneeReferenceNumber = Common.WildCardSearchToNormal(so.ConsigneeReferenceNumber);
            so.NotifyPartyReferenceNumber = Common.WildCardSearchToNormal(so.NotifyPartyReferenceNumber);
            so.PayorReferenceNumber = Common.WildCardSearchToNormal(so.PayorReferenceNumber);
            so.CommodityCode = Common.WildCardSearchToNormal(so.CommodityCode);
            so.CommodityDescription = Common.WildCardSearchToNormal(so.CommodityDescription);
            so.ReferenceValue = Common.WildCardSearchToNormal(so.ReferenceValue);
            so.ChargeCode = Common.WildCardSearchToNormal(so.ChargeCode);
            so.ChargeDesc = Common.WildCardSearchToNormal(so.ChargeDesc);
            so.ContainerId = Common.WildCardSearchToNormal(so.ContainerId);
            so.ImageType = Common.WildCardSearchToNormal(so.ImageType);
        }

        public BOLHeader GetResults(string sqlCmd, int idx)
        {
            var results = GetResults(null, sqlCmd, false);
            var bol = results.FirstOrDefault(x => x.Idx == idx) ?? new BOLHeader();
            bol.ResultCount = results.Count;
            return bol;
        }

        public List<BOLHeader> GetResults(SearchOptions so, string sqlCmd, bool Table = false)
        {
            string sql = "";
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

                var columns = @"b.*,
                        (SELECT TOP(1) ISNULL(ParticipantName,'') FROM Participants p WHERE b.ShipperID=p.ParticipantID) AS ParticipantName,
                        (SELECT TOP(1) CommodityCode FROM bolcommodity d WHERE d.BOL#=b.BOL#) AS CommodityCod";

                if (Table)
                {
                    columns += $", (SELECT TOP(1) ast.EventType AS BOLViewed FROM ActivityStats ast " +
                               $"WHERE ast.BolNo=b.BOL# AND ast.EventType='BOLViewed' AND ast.ClientId='{so.CompanyId}') AS BOLViewed," +
                               $"(SELECT TOP(1) c.Type FROM BOLContainers c WHERE c.BOL#=b.BOL#) AS ContainerType," +
                               $"(SELECT MIN(eventdatetime) FROM ContainerEventHistory con WHERE con.BOL#=b.BOL# OR (con.BookingId LIKE '%'+ b.BookingID + '%' " +
                               $"AND con.ContainerID=(SELECT TOP(1) bc.ContainerID FROM BOLContainers bc WHERE bc.BOL#=b.Bol#))) AS EventDatetime," +
                               $"(SELECT TOP(1) c.size FROM BOLContainers c WHERE c.BOL#=b.BOL#) AS ContainerSize," +
                               $"(SELECT TOP(1) CBF FROM bolcommodity c WHERE c.BOL#=b.BOL#) AS CBF," +
                               $"(SELECT TOP(1) d.CommodityDescription FROM bolcommodity d WHERE d.BOL#=b.BOL#) AS CommodityDescription," +
                               $"(SELECT TOP(1) SUM(usamount) FROM BOLCharges bc WHERE bc.BOL#=b.BOL#) AS BOLCharges," +
                               $"(SELECT TOP(1) SUM(usamount) FROM BOLCharges bc WHERE bc.BOL#=b.BOL# AND ChargeDescription LIKE '%ocean%') AS OceanCharges";

                    if (!string.IsNullOrWhiteSpace(so.BillType))
                    {
                        columns += ",(SELECT TOP 1 CONVERT(VARCHAR, eventdatetime, 110) FROM ContainerEventHistory ceh WHERE ceh.eventtype='dfi' AND ceh.bol#=b.BOL#) AS DischargeDate," +
                                   "(SELECT sum(usamount) from bolcharges WHERE chargecode='pd' and bol#=b.BOL#) AS PerDiemCharges,(SELECT TOP 1 convert(varchar, eventdatetime, 110) from containereventhistory ce " +
                                   "WHERE ce.eventtype='RMT' AND ce.bol#=b.BOL#) AS ReturnDate";
                    }

                    sql += $"SELECT b.* FROM (SELECT b.*,ROW_NUMBER() OVER ({columns}) AS Idx FROM(SELECT {sortingOrder} FROM BOLHeader b) AS b";

                    FixWildcards(ref so);
                    BuildCmdInnerJoin1(ref sql, so);
                    BuildCmdInnerJoin2(ref sql, so);
                    BuildCmdWhereCondition1(ref sql, so);
                    BuildCmdWhereCondition2(ref sql, so, Table);

                    if (!string.IsNullOrWhiteSpace(so.BillType))
                    {
                        AppendConditionOperator(ref sql, Table);
                        sql += $"b.BillType={so.BillType}";
                    }

                    sql += ") AS b";

                    if (so.CompanyId > 0)
                    {
                        ActiveClient.Set(so.CompanyId);
                    }

                }
            }
            else
            {
                sql = sqlCmd;
            }

            using (var connection = Common.ClientDatabase)
            {
                var results = connection.Query<BOLHeader>(sql).ToList();
                return results;
            }
        }

        public List<BOLHeader> GetResults(SearchOptions so, bool Table = false)
        {
            if (Table)
            {
                return GetResults(so, null, true);
            }
            return GetResults(so, null);
        }

        public List<BOLHeader> GetAuditResultStats(SearchOptions so, string clientName)
        {
            string cmd = @"SELECT b.ShipperID,p.ParticipantName, b.OriginLocationCode AS Origin,b.DestinationLocationCode AS Destination, COUNT(distinct b.bol#) AS ShipmentCount,
                                MIN(BOLCharges) AS MinCharges, MAX(BOLCharges) AS MaxCharges,
                                (MAX(BOLCharges) - MIN(BOLCharges)) AS Difference, STDEV(BOLCharges) AS StandardDeviation
                                FROM BOLHeader b
                                INNER JOIN Participants p ON b.shipperid=p.ParticipantID
                                INNER JOIN BOLCharges bc ON bc.BOL#= b.BOL#
                                LEFT  JOIN (SELECT bol#, SUM(usamount) AS BOLCharges FROM BOLCharges GROUP BY bol#)  bc2  ON bc2.BOL#=b.BOL#";

            FixWildcards(ref so);
            BuildCmdInnerJoin1(ref cmd, so);
            BuildCmdInnerJoin2(ref cmd, so);
            BuildCmdWhereCondition1(ref cmd, so);
            BuildCmdWhereCondition2(ref cmd, so);

            if (!string.IsNullOrWhiteSpace(so.BillType))
            {
                AppendConditionOperator(ref cmd);
                cmd += $"b.BillType= {so.BillType}";
            }

            cmd += $"AND p.ParticipantName NOT LIKE '%{ clientName }%'";
            cmd += @"GROUP BY b.shipperid,p.ParticipantName,b.OriginLocationCode,b.DestinationLocationCode";
            cmd += @"ORDER BY StandardDeviation DESC";

            using (var connection = Common.ClientDatabase)
            {
                var resultStats = connection.Query<BOLHeader>(cmd).ToList();
                return resultStats;
            }
        }

        public List<BOLHeader> GetAuditResultStatsByOrigin(SearchOptions so, string clientName)
        {
            string cmd = @"SELECT b.OriginLocationCode AS Origin,b.DestinationLocationCode AS Destination,
                                 (SELECT TOP 1 ShipperID FROM BOLHeader WHERE OriginLocationCode=b.OriginLocationCode) AS ShipperID,
                                 count(distinct b.bol#) AS ShipmentCount,
                                 MIN(BOLCharges) AS MinCharges, MAX(BOLCharges) AS MaxCharges,
                                 (MAX(BOLCharges) - MIN(BOLCharges)) AS Difference, STDEV(BOLCharges) AS StandardDeviation  FROM BOLHeader b
                                 INNER JOIN Participants p ON b.shipperid=p.ParticipantID
                                 INNER JOIN BOLCharges bc ON bc.BOL#= b.BOL#
                                 LEFT  JOIN (SELECT bol#, sum(usamount) AS BOLCharges FROM BOLCharges GROUP BY bol#)  bc2 ON bc2.BOL#=b.BOL#";

            FixWildcards(ref so);
            BuildCmdInnerJoin1(ref cmd, so);
            BuildCmdInnerJoin2(ref cmd, so);
            BuildCmdWhereCondition1(ref cmd, so);
            BuildCmdWhereCondition2(ref cmd, so);

            if (!string.IsNullOrWhiteSpace(so.BillType))
            {
                AppendConditionOperator(ref cmd);
                cmd += $"b.BillType= {so.BillType}";
            }

            cmd += $"AND p.ParticipantName NOT LIKE '%{clientName}%'";
            cmd += @"GROUP BY b.OriginLocationCode,b.DestinationLocationCode";
            cmd += @"ORDER BY StandardDeviation DESC";


            using (var connection = Common.ClientDatabase)
            {
                var resultStatsByOrigin = connection.Query<BOLHeader>(cmd).ToList();
                return resultStatsByOrigin;
            }
        }

        public List<BOLHeader> GetAuditResultStatsByShipper(SearchOptions so, string clientName)
        {
            string cmd = @"SELECT  b.ShipperID,
                                   (SELECT TOP 1 ParticipantName FROM Participants WHERE ParticipantID=b.shipperid) AS ParticipantName,
                                   (SELECT TOP 1 OriginLocationCode FROM BOLHeader WHERE ShipperID=b.shipperid) AS Origin,
                                   (SELECT TOP 1 DestinationLocationCode FROM BOLHeader WHERE ShipperID=b.shipperid) AS Destination,
                                   COUNT(distinct b.bol#) AS ShipmentCount,
                                   MIN(BOLCharges) AS MinCharges, MAX(BOLCharges) AS MaxCharges,
                                   (MAX(BOLCharges) - MIN(BOLCharges)) AS Difference, STDEV(BOLCharges) AS StandardDeviation
                                   FROM BOLHeader b
                                   INNER JOIN Participants p ON b.shipperid=p.ParticipantID
                                   INNER JOIN BOLCharges bc ON bc.BOL#= b.BOL#
                                   left  JOIN (SELECT bol#, sum(usamount) AS BOLCharges FROM BOLCharges GROUP BY bol#)  bc2 ON bc2.BOL#=b.BOL#";

            FixWildcards(ref so);
            BuildCmdInnerJoin1(ref cmd, so);
            BuildCmdInnerJoin2(ref cmd, so);
            BuildCmdWhereCondition1(ref cmd, so);
            BuildCmdWhereCondition2(ref cmd, so);

            if (!string.IsNullOrWhiteSpace(so.BillType))
            {
                AppendConditionOperator(ref cmd);
                cmd += $"b.BillType= {so.BillType}";
            }

            cmd += $"AND p.ParticipantName NOT LIKE '%{clientName}%'";
            cmd += @"GROUP BY b.shipperid";
            cmd += @"ORDER BY StandardDeviation DESC";

            using (var connection = Common.ClientDatabase)
            {
                var resultStatsByShipper = connection.Query<BOLHeader>(cmd).ToList();
                return resultStatsByShipper;
            }

        }

        public List<BOLHeader> GetAuditResultStatsByPOL(SearchOptions so, string clientName)
        {
            string cmd = @"SELECT b.POL,
                                  (SELECT TOP 1 ShipperID FROM BOLHeader where POL=b.POL) AS ShipperID,
                                  (SELECT TOP 1 OriginLocationCode FROM BOLHeader where POL=b.POL) AS Origin,
                                  (SELECT TOP 1 DestinationLocationCode FROM BOLHeader where POL=b.POL) AS Destination,
                                  COUNT(distinct b.bol#) AS ShipmentCount,MIN(BOLCharges) AS MinCharges, MAX(BOLCharges) AS MaxCharges,
                                  (MAX(BOLCharges) - MIN(BOLCharges)) AS Difference, STDEV(BOLCharges) AS StandardDeviation
                                  FROM BOLHeader b
                                  INNER JOIN Participants p ON b.shipperid=p.ParticipantID
                                  INNER JOIN BOLCharges bc ON bc.BOL#= b.BOL#
                                  LEFT  JOIN (SELECT bol#, sum(usamount) AS BOLCharges FROM BOLCharges group by bol#)  bc2 ON bc2.BOL#=b.BOL#";

            FixWildcards(ref so);
            BuildCmdInnerJoin1(ref cmd, so);
            BuildCmdInnerJoin2(ref cmd, so);
            BuildCmdWhereCondition1(ref cmd, so);
            BuildCmdWhereCondition2(ref cmd, so);

            if (!string.IsNullOrWhiteSpace(so.BillType))
            {
                AppendConditionOperator(ref cmd);
                cmd += $"b.BillType= {so.BillType}";
            }

            cmd += $"AND p.ParticipantName NOT LIKE '%{clientName}%'";
            cmd += @"GROUP BY b.POL";
            cmd += @"ORDER BY StandardDeviation DESC";

            using (var connection = Common.ClientDatabase)
            {
                var resultStatsByPOL = connection.Query<BOLHeader>(cmd).ToList();
                return resultStatsByPOL;
            }
        }

        public void BuildCmdInnerJoin1(ref string cmd, SearchOptions so)
        {
            if (so.EquipmentSize != null || so.EquipmentType != null)
            {
                cmd += "INNER JOIN BOLContainers c ON b.BOL#=c.BOL#";

                if (so.EquipmentSize != null)
                {
                    cmd += $"AND (c.Size IN ({string.Join(",", so.EquipmentSize.Select(es => $"'{es}'"))})";
                }
                if (so.EquipmentType != null)
                {
                    cmd += $"AND (c.Type IN ({string.Join(",", so.EquipmentType.Select(et => $"'{et}'"))})";
                }
            }

            if (!string.IsNullOrWhiteSpace(so.CommodityCode) || !string.IsNullOrWhiteSpace(so.CommodityDescription))
            {
                cmd += "INNER JOIN BOLCommodity d ON d.BOL#=b.BOL#";

                if (!string.IsNullOrWhiteSpace(so.CommodityCode))
                {
                    cmd += $" AND d.CommodityCode LIKE {so.CommodityCode}";
                }
                if (!string.IsNullOrWhiteSpace(so.CommodityDescription))
                {
                    cmd += $" AND d.CommodityDescription LIKE {so.CommodityDescription}";
                }
            }
        }

        public void BuildCmdInnerJoin2(ref string cmd, SearchOptions so)
        {
            if (so.SITFlag || !string.IsNullOrWhiteSpace(so.ContainerId) || (so.MinTemp != 0 || so.MaxTemp != 0))
            {
                if (cmd.IndexOf("BOLCommodity", StringComparison.CurrentCulture) <= 0)
                {
                    cmd += " INNER JOIN BOLCommodity d ON d.BOL#=b.BOL#";
                }
                if (so.MinTemp != 0)
                {
                    cmd += $" AND d.MinTemperature > {so.MinTemp} AND d.MinTemperature IS NOT NULL";
                }
                if (so.MaxTemp != 0)
                {
                    cmd += $"AND d.MaxTemperature < {so.MaxTemp}";
                    cmd += "AND d.MaxTemperature IS NOT NULL";
                }
                if (!string.IsNullOrWhiteSpace(so.ContainerId))
                {
                    cmd += $"AND d.ContainerID LIKE {so.ContainerId}";
                }
                if (so.SITFlag)
                {
                    cmd += "AND d.SITFlag = 'Y' ";
                }
            }
            if (so.UNHazmatCode != null)
            {
                cmd += "INNER JOIN BOLHazardous h ON h.BOL#=b.BOL#";

                if (so.UNHazmatCode != null)
                {
                    cmd += $"AND (h.UNHazmatCode IN ({string.Join(",", so.UNHazmatCode.Select(uc => $"'{uc}'"))})";
                }
            }
        }

        public void BuildCmdWhereCondition1(ref string cmd, SearchOptions so)
        {
            var depStartDateFormatted = so.DepartureStartDate.ToDateTime();
            var depEndDateFormatted = so.DepartureEndDate.ToDateTime();

            if (depStartDateFormatted != DateTime.MinValue && depEndDateFormatted != DateTime.MinValue)
            {
                var strDepartureStartDate = depStartDateFormatted.ToString("yyyy-MM-dd");
                var strDepartureEndDate = depEndDateFormatted.ToString("yyyy-MM-dd");
                cmd += $"AND b.ActualDepartureDate BETWEEN {strDepartureStartDate} AND {strDepartureEndDate}";
            }
            if (so.OriginLocationCode != null)
            {
                cmd += $"AND (b.OriginLocationCode IN ( {string.Join(",", so.OriginLocationCode.Select(olc => $"'{olc}'"))} )";
            }
            if (so.POLCode != null)
            {
                cmd += $"AND (b.POL IN ( {string.Join(",", so.POLCode.Select(pc => $"'{pc}'"))} )";
            }
            if (so.PODCode != null)
            {
                cmd += $"AND (b.POD IN ( {string.Join(",", so.PODCode.Select(pd => $"'{pd}'"))} )";
            }
            if (so.ShipperID != null)
            {
                cmd += $"AND (b.ShipperID IN ( {string.Join(",", so.ShipperID.Select(si => $"'{si}'"))} )";
            }
            if (so.ConsigneeID != null)
            {
                cmd += $"AND (b.ConsigneeID IN ( {string.Join(",", so.ConsigneeID.Select(ci => $"'{ci}'"))} )";
            }
        }

        public void BuildCmdWhereCondition2(ref string cmd, SearchOptions so, bool Table = false)
        {
            if (so.CurrentMaxBalance > 0 || so.CurrentMinBalance > 0)
            {
                cmd += $" AND ((SELECT SUM(usamount) FROM BOLCharges WHERE BOL#=b.bol#)-(SELECT SUM(AmountPaid) FROM arcash where BOL#=b.bol#) )  BETWEEN {so.CurrentMinBalance} AND {so.CurrentMaxBalance}";
            }

            if (so.Mode != null)
            {
                var temp = "";
                var strOR = " OR ";

                foreach (var item in so.Mode)
                {
                    temp += $"b.MODE = '{item}'" + strOR;
                }
                temp = temp.Remove(temp.Length - strOR.Length, strOR.Length);
                cmd += "(" + temp + ")";
            }

            if (so.PayorID != null)
            {
                var temp = "";
                var strOR = " OR ";

                foreach (var item in so.PayorID)
                {
                    temp += $"b.PayorID = '{item}'" + strOR;
                }
                temp = temp.Remove(temp.Length - strOR.Length, strOR.Length);
                cmd += "(" + temp + ")";
            }

            if (!string.IsNullOrWhiteSpace(so.BookingId))
            {
                cmd += $" AND b.BookingID = {so.BookingId}";
            }

            if (!string.IsNullOrWhiteSpace(so.BOLNo))
            {
                cmd += $" AND b.BOL# = {so.BOLNo}";
            }

            if (so.EliminateBDResearchItems)
            {
                AppendConditionOperator(ref cmd, Table);
                cmd += $"b.BOL# NOT IN (SELECT d.Bol FROM {_dbName}.dbo.BalanceDues d WHERE d.CompanyId = {so.CompanyId} UNION SELECT r.Bol FROM {_dbName}.dbo.ResearchItems r WHERE r.CompanyId = {so.CompanyId} );";
            }

            if (!string.IsNullOrWhiteSpace(so.BOLViews))
            {
                AppendConditionOperator(ref cmd, Table);
                cmd += $"(SELECT COUNT(*) AS ViewCount FROM {_dbName}.dbo.ActivityStats st WHERE ";
                if (so.UserIds.Any() && so.UserIds[0] != "00000000-0000-0000-0000-000000000000")
                {
                    string list = Core.Utility.JoinStrings(so.UserIds.ToArray());
                    cmd += $"st.userId IN ('{list}') AND";
                }
                cmd += $" st.BOLNo=b.BOL#) <= {Convert.ToInt32(so.BOLViews)}";
            }

            if (!string.IsNullOrWhiteSpace(so.ImageType))
            {
                AppendConditionOperator(ref cmd, Table);
                cmd += $"EXISTS(SELECT i.filename,i.ScanDate FROM DocumentImages i WHERE b.BookingID=i.BookingID AND i.Type LIKE {so.ImageType})";
            }

            if (so.BookingMinWeight > 0 && so.BookingMaxWeight < 70000)
            {
                AppendConditionOperator(ref cmd, Table);
                cmd += $" (SELECT SUM(WeightPounds) FROM BOLCommodity c WHERE c.BOL#= b.BOL#) BETWEEN {so.BookingMinWeight} AND {so.BookingMaxWeight}";
            }

            if (!string.IsNullOrWhiteSpace(so.ChargeCode) || !string.IsNullOrWhiteSpace(so.ChargeDesc) || (!string.IsNullOrWhiteSpace(so.ChargeMinAmount) || !string.IsNullOrWhiteSpace(so.ChargeMaxAmount)))
            {
                if (!string.IsNullOrWhiteSpace(so.ChargeCode) || !string.IsNullOrWhiteSpace(so.ChargeDesc))
                {
                    AppendConditionOperator(ref cmd, Table);
                    var temp = "EXISTS(SELECT TOP(1) ch.Bol# FROM BOLCharges ch WHERE ch.BOL#=b.BOL#";
                    if (!string.IsNullOrWhiteSpace(so.ChargeCode))
                    {
                        var temp1 = temp;
                        if (so.ChargeCodeOperator != "=")
                        {
                            temp1 = "NOT " + temp1;
                        }
                        cmd += $"{temp1} AND ChargeCode LIKE {so.ChargeCode}";
                        cmd += ")";
                    }
                    if (!string.IsNullOrWhiteSpace(so.ChargeDesc))
                    {
                        if (!string.IsNullOrWhiteSpace(so.ChargeCode))
                        {
                            AppendConditionOperator(ref cmd, Table);
                        }
                        var temp1 = temp;
                        if (so.ChargeDescOperator != "=")
                        {
                            temp1 = "NOT " + temp1;
                        }
                        cmd += $"{temp1} AND ChargeDescription LIKE {so.ChargeDesc}";
                        cmd += ")";
                    }
                    if (so.ChargeMinAmount != null && so.ChargeMaxAmount != null)
                    {
                        AppendConditionOperator(ref cmd, Table);
                        if (so.ChargeMinAmount == "0" && so.ChargeMaxAmount == "0")
                        {
                            cmd += "(SELECT USAmount FROM BOLCharges WHERE USAmount=0 OR USAmount IS NULL)";
                        }
                        else
                        {
                            cmd += "(SELECT SUM(USAmount) FROM BOLCharges WHERE BOl#=b.Bol#";
                            if (!string.IsNullOrWhiteSpace(so.ChargeDesc))
                            {
                                if (so.ChargeDescOperator != "=")
                                {
                                    cmd += $"AND ChargeDescription NOT LIKE {so.ChargeDesc}";
                                }
                                else
                                {
                                    cmd += $"AND ChargeDescription LIKE {so.ChargeDesc}";
                                }
                            }
                            cmd += $") BETWEEN {Convert.ToDecimal(so.ChargeMinAmount)} AND {Convert.ToDecimal(so.ChargeMaxAmount)} ";
                        }
                    }
                }
            }

            if (so.TotalChargeMinAmount != null && so.TotalChargeMaxAmount != null)
            {
                AppendConditionOperator(ref cmd, Table);
                cmd += " (SELECT SUM(USAmount) FROM BOLCharges WHERE BOl#=b.Bol#";
                cmd += $") between {Convert.ToDecimal(so.TotalChargeMinAmount)} AND {Convert.ToDecimal(so.TotalChargeMaxAmount)}";
            }

            if (so.ShipperRefNoNotNullValues || so.ForwarderRefNoNotNullValues || so.ConsigneeRefNoNotNullValues || so.PayorRefNoNotNullValues|| 
                so.NotifyNoNotNullValues || !string.IsNullOrWhiteSpace(so.ReferenceValue) || !string.IsNullOrWhiteSpace(so.ReferenceType) || 
                !string.IsNullOrWhiteSpace(so.ShipperReferenceNumber) || !string.IsNullOrWhiteSpace(so.ForwarderReferenceNumber) || 
                !string.IsNullOrWhiteSpace(so.ConsigneeReferenceNumber) || !string.IsNullOrWhiteSpace(so.PayorReferenceNumber) || !string.IsNullOrWhiteSpace(so.NotifyPartyReferenceNumber))
            {
                AppendConditionOperator(ref cmd, Table);
                cmd += "Exists(SELECT TOP(1) r.BOL# FROM BOLReference r WHERE r.BOL#=b.BOL#";

                if (so.SITFlag)
                {
                    cmd += " AND ReferenceType= 'SITFLAG' AND Reference= 'Y' ";
                }

                if (!string.IsNullOrWhiteSpace(so.ReferenceType))
                {
                    cmd += $" AND ReferenceType = {so.ReferenceType}";
                }

                if (!string.IsNullOrWhiteSpace(so.ReferenceValue))
                {
                    cmd += $" AND Reference LIKE {so.ReferenceValue}";
                }

                if (so.ShipperRefNoNotNullValues || !string.IsNullOrWhiteSpace(so.ShipperReferenceNumber))
                {
                    cmd += " AND ReferenceType = 'SHPREF' ";
                    if (so.ShipperRefNoNotNullValues)
                    {
                        cmd += " AND Reference IS NOT NULL AND Reference <> ''";
                    }
                    if (!string.IsNullOrWhiteSpace(so.ShipperReferenceNumber))
                    {
                        cmd += $" AND Reference LIKE {so.ShipperReferenceNumber} ";
                    }
                }

                if (so.ForwarderRefNoNotNullValues || !string.IsNullOrWhiteSpace(so.ForwarderReferenceNumber))
                {
                    cmd += " AND ReferenceType = 'FWDREF' ";
                    if (so.ForwarderRefNoNotNullValues)
                    {
                        cmd += " AND Reference IS NOT NULL AND Reference <> ''";
                    }
                    if (!string.IsNullOrWhiteSpace(so.ForwarderReferenceNumber))
                    {
                        cmd += $" AND Reference LIKE {so.ForwarderReferenceNumber} ";
                    }
                }

                if (so.ConsigneeRefNoNotNullValues || !string.IsNullOrWhiteSpace(so.ConsigneeReferenceNumber))
                {
                    cmd += " AND ReferenceType = 'CONREF' ";
                    if (so.ConsigneeRefNoNotNullValues)
                    {
                        cmd += " AND Reference IS NOT NULL AND Reference <> ''";
                    }
                    if (!string.IsNullOrWhiteSpace(so.ConsigneeReferenceNumber))
                    {
                        cmd += $" AND Reference LIKE {so.ConsigneeReferenceNumber} ";
                    }
                }

                if (so.NotifyNoNotNullValues || !string.IsNullOrWhiteSpace(so.NotifyPartyReferenceNumber))
                {
                    cmd += " AND ReferenceType = 'NTFYPTY' ";
                    if (so.NotifyNoNotNullValues)
                    {
                        cmd += " AND Reference IS NOT NULL AND Reference <> ''";
                    }
                    if (!string.IsNullOrWhiteSpace(so.NotifyPartyReferenceNumber))
                    {
                        cmd += $" AND Reference LIKE {so.NotifyPartyReferenceNumber} ";
                    }
                }

                if (so.PayorRefNoNotNullValues || !string.IsNullOrWhiteSpace(so.PayorReferenceNumber))
                {
                    cmd += " AND ReferenceType = 'PAYREF' ";
                    if (so.PayorRefNoNotNullValues)
                    {
                        cmd += " AND Reference IS NOT NULL AND Reference <> ''";
                    }
                    if (!string.IsNullOrWhiteSpace(so.PayorReferenceNumber))
                    {
                        cmd += $" AND Reference LIKE {so.PayorReferenceNumber} ";
                    }
                }
                cmd += ")";
            }

            var trackingStartDateFormatted = so.TrackingStartDate.ToDateTime();
            var trackingEndDateFormatted = so.TrackingEndDate.ToDateTime();
            if ((trackingStartDateFormatted != DateTime.MinValue && trackingEndDateFormatted != DateTime.MinValue) || so.ContainerEventType != null)
            {
                AppendConditionOperator(ref cmd, Table);
                cmd += "EXISTS (SELECT DISTINCT con.BOL# FROM BOLContainers con INNER JOIN ContainerEventHistory eh ON(RIGHT(b.BOL#,10) = RIGHT(eh.bol#,10) OR " +
                       "RIGHT(b.bookingid,10) = RIGHT(eh.bookingid,10)) AND LEFT(con.containerid,10) = LEFT(eh.ContainerID,10) AND con.BOL#=b.BOL#";

                if (trackingStartDateFormatted != DateTime.MinValue && trackingEndDateFormatted != DateTime.MinValue)
                {
                    cmd += $" AND eh.EventDateTime BETWEEN {so.TrackingStartDate} AND {so.TrackingEndDate}";
                }

                if (so.ContainerEventType != null && so.ContainerEventType[0] != "All Events")
                {
                    var temp = "";
                    var strOR = " OR ";

                    foreach (var item in so.ContainerEventType)
                    {
                        temp += temp += $" eh.EventType = '{item}'" + strOR;
                    }
                    temp = temp.Remove(temp.Length - strOR.Length, strOR.Length);
                    cmd += $"AND ('{temp}')";
                }
                cmd += ")";
            }

        }

        public void AppendConditionOperator(ref string cmd, bool Table = false)
        {
            if (Table)
            {
                if (cmd.IndexOf("b) AS b\n WHERE", StringComparison.CurrentCulture) > 0 ||
                    cmd.IndexOf("b) AS b\nWHERE", StringComparison.CurrentCulture) > 0 ||
                    cmd.IndexOf("WHERE (b.ActualDepartureDate", StringComparison.CurrentCulture) > 0)
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
                if (cmd.IndexOf("WHERE", StringComparison.CurrentCultureIgnoreCase) > 0)
                {
                    cmd += " AND ";
                }
                else
                {
                    cmd += " WHERE ";
                }
            }
        }

        public void SaveBalanceDueItems(List<BalanceDues_Item> listDues)
        {
            using (var connection = Common.Database)
            {
                foreach (var item in listDues)
                {
                    item.ItemId = 0;
                    connection.Insert(item);
                }
            }
        }

        public int DeleteBDItem(int itemId)
        {
            const string query = @"DELETE FROM [BalanceDues.Item] WHERE  ItemId=@ItemId;"; 

            using (var connection = Common.Database)
            {
                var result = connection.Execute(query, new { @ItemId = itemId });
                return result;
            }
        }

        public int DeleteBDItem(int companyId, string bolNo, string customerId, string region)
        {
            const string query = @"DELETE FROM [BalanceDues.Item] WHERE CompanyId=@CompanyId AND Bol=@BolNo AND CustomerId=@CustomerId AND Region=@Region;";

            using (var connection = Common.Database)
            {
                var result = connection.Execute(query, new { @CompanyId = companyId, @BolNo = bolNo, @CustomerId = customerId, @Region = region });
                return result;
            }
        }
    }
}
