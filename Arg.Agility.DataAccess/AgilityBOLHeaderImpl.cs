using Arg.Agility.DataModels;
using Arg.Core;
using Arg.DataAccess;
using Arg.DataModels;
using CacheManager.Core;
using CustomExtensions;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;

namespace Arg.Agility.DataAccess
{
    public class AgilityBOLHeaderImpl
    {
        private string _dbName = Common.DBName;
        private readonly SqlConnection _connection;
        public AgilityBOLHeaderImpl()
        {
            _connection = Common.ClientDatabase;
        }

        public BOLHeaders GetBOLHeader(string jobNumber)
        {
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(jobNumber))
            {
                parameters.Add("@JobNumber", jobNumber, DbType.String);
            }
            const string query = @"SELECT * 
                                   FROM BOLHeaders
                                   WHERE JobNumber=@JobNumber;";

            return _connection.QueryFirstOrDefault<BOLHeaders>(query, parameters);
        }

        public List<BOLHeaders> GetServiceMovementType()
        {
            const string query = @"SELECT DISTINCT b.ServiceMovementType , b.ServiceMovementType AS ServiceMovementTypeCode 
                                   FROM BOLHeaders b
                                   WHERE ServiceMovementType <> 'null'
                                   ORDER BY ServiceMovementType;";

            return _connection.Query<BOLHeaders>(query, commandType: CommandType.Text).ToList();
        }

        public List<BOLHeaders> GetServiceLevel()
        {
            const string query = @"SELECT DISTINCT b.ServiceLevel , b.ServiceLevel AS ServiceLevelCode 
                                   FROM BOLHeaders b
                                   WHERE ServiceLevel <> 'null'
                                   ORDER BY ServiceLevel;";

            return _connection.Query<BOLHeaders>(query, commandType: CommandType.Text).ToList();
        }

        public List<BOLHeaders> GetServiceType()
        {
            const string query = @"SELECT DISTINCT b.ServiceType , b.ServiceType AS ServiceTypeCode 
                                   FROM BOLHeaders b
                                   WHERE ServiceType <> 'null'
                                   ORDER BY ServiceType;";

            return _connection.Query<BOLHeaders>(query, commandType: CommandType.Text).ToList();
        }

        public List<BOLHeaders> GetDistinctOrigin()
        {
            const string query = @"SELECT DISTINCT b.Origin + ' ' + ISNULL(c.LocationName, '') AS origin , b.Origin AS OriginLocationCode 
                                   FROM BOLHeaders b
                                   LEFT JOIN LocationCodes c ON b.Origin = c.LocationCode
                                   WHERE b.Origin IS NOT NULL
                                   ORDER BY Origin;";

            return _connection.Query<BOLHeaders>(query, commandType: CommandType.Text).ToList();
        }

        public List<BOLHeaders> GetPortOfExit()
        {
            const string query = @"SELECT DISTINCT b.PortOfExit + ' ' + ISNULL(c.LocationName, '') AS PortOfExit , b.PortOfExit AS PortOfExitCode 
                                   FROM BOLHeaders b
                                   LEFT JOIN LocationCodes c ON b.PortOfExit = c.LocationCode
                                   WHERE b.PortOfExit IS NOT NULL
                                   ORDER BY PortOfExit;";

            return _connection.Query<BOLHeaders>(query, commandType: CommandType.Text).ToList();
        }

        public List<BOLHeaders> GetPortofEntry()
        {
            const string query = @"SELECT DISTINCT b.PortofEntry + ' ' + ISNULL(c.LocationName, '') AS PortofEntry , b.PortofEntry AS PortofEntryCode 
                                   FROM BOLHeaders b
                                   LEFT JOIN LocationCodes c ON b.PortofEntry = c.LocationCode
                                   WHERE b.PortofEntry IS NOT NULL
                                   ORDER BY PortofEntry;";

            return _connection.Query<BOLHeaders>(query, commandType: CommandType.Text).ToList();
        }

        public List<BOLHeaders> GetDestination()
        {
            const string query = @"SELECT DISTINCT b.Destination + ' ' + ISNULL(c.LocationName, '') AS Destination , b.Destination AS DestinationLocationCode 
                                   FROM BOLHeaders b
                                   LEFT JOIN LocationCodes c ON b.Destination = c.LocationCode
                                   WHERE b.Destination IS NOT NULL
                                   ORDER BY Destination;";

            return _connection.Query<BOLHeaders>(query, commandType: CommandType.Text).ToList();
        }

        public List<BOLHeaders> GetShipper()
        {
            const string query = @"SELECT DISTINCT b.Shipper + ' ' + ISNULL(p.ParticipantName, '') AS Shipper , b.Shipper AS ShipperID 
                                   FROM BOLHeaders b
                                   LEFT JOIN Participants p ON b.Shipper = p.ParticipantID
                                   WHERE b.Shipper IS NOT NULL
                                   ORDER BY Shipper;";

            return _connection.Query<BOLHeaders>(query, commandType: CommandType.Text).ToList();
        }

        public List<BOLHeaders> GetConsignee()
        {
            const string query = @"SELECT DISTINCT b.Consignee + ' ' + ISNULL(p.ParticipantName, '') AS Consignee , b.Consignee AS ConsigneeID 
                                   FROM BOLHeaders b
                                   LEFT JOIN Participants p ON b.ShConsigneeipper = p.ParticipantID
                                   WHERE b.Consignee IS NOT NULL
                                   ORDER BY Consignee;";

            return _connection.Query<BOLHeaders>(query, commandType: CommandType.Text).ToList();
        }

        public List<BOLHeaders> GetNotifyParty()
        {
            const string query = @"SELECT DISTINCT b.NotifyParty + ' ' + ISNULL(p.ParticipantName, '') AS NotifyParty , b.NotifyParty AS NotifyPartyCode 
                                   FROM BOLHeaders b
                                   LEFT JOIN Participants p ON b.NotifyParty = p.ParticipantID
                                   WHERE b.NotifyParty IS NOT NULL
                                   ORDER BY NotifyParty;";

            return _connection.Query<BOLHeaders>(query, commandType: CommandType.Text).ToList();
        }

        public List<BOLHeaders> GetExportingCarrier()
        {
            const string query = @"SELECT DISTINCT b.ExportingCarrier + ' ' + ISNULL(c.CarrierName, '') AS ExportingCarrier , b.ExportingCarrier AS ExportingCarrierCode 
                                   FROM BOLHeaders b
                                   LEFT JOIN Carriers c ON b.CarrierCode = c.CarrierCode
                                   WHERE b.ExportingCarrier IS NOT NULL
                                   ORDER BY ExportingCarrier;";

            return _connection.Query<BOLHeaders>(query, commandType: CommandType.Text).ToList();
        }

        public List<BOLHeaders> GetPrepaidCollect()
        {
            const string query = @"SELECT b.PrepaidCollect, b.PrepaidCollect AS PrepaidCollectCode 
                                   FROM BOLHeaders b
                                   WHERE b.PrepaidCollect IS NOT NULL
                                   ORDER BY PrepaidCollect;";

            return _connection.Query<BOLHeaders>(query, commandType: CommandType.Text).ToList();
        }

        public List<DataModels.Participants> GetAllParticipants()
        {
            const string query = @"SELECT * 
                                   FROM Participants;";

            return _connection.Query<DataModels.Participants>(query, commandType: CommandType.Text).ToList();
        }

        public int GetResultCount(Arg.DataModels.SearchOptions so)
        {
            const string query = @"SELECT COUNT(*) AS ResultCount 
                                   FROM BOLHeaders b;";

            return Convert.ToInt32(_connection.ExecuteScalar<object>(query, commandType: CommandType.Text));
        }

        public List<DataModels.Generic> GetBOLCustomers()
        {
            const string query = @"SELECT ParticipantID, CONCAT(ParticipantID, '  ' + ParticipantName) AS ParticipantName 
                                   FROM Participants
                                   WHERE ParticipantType = 'Payor'
                                   ORDER BY ParticipantName;";

            return _connection.Query<DataModels.Generic>(query, commandType: CommandType.Text).ToList();
        }

        public BOLHeaders GetShipper(string jobNumber)
        {
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(jobNumber))
            {
                parameters.Add("@JobNumber", jobNumber, DbType.String);

            }
            const string query = @"SELECT DISTINCT b.ConsignmentID 
                                   FROM BOLHeaders b
                                   WHERE b.JobNumber=@JobNumber;";

            return _connection.QueryFirstOrDefault<BOLHeaders>(query, parameters);
        }

        public BOLHeaders GetConsigneeReference(string jobNumber)
        {
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(jobNumber))
            {
                parameters.Add("@JobNumber", jobNumber, DbType.String);

            }
            const string query = @"SELECT DISTINCT b.ConsignmentID 
                                   FROM BOLHeaders b
                                   WHERE b.JobNumber=@JobNumber;";

            return _connection.QueryFirstOrDefault<BOLHeaders>(query, parameters);
        }

        public List<BalanceDue> GetCustomerBalanceDues(string bolNo, int companyId)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@CompanyId", companyId, DbType.Int32);
            if (!string.IsNullOrWhiteSpace(bolNo))
            {
                parameters.Add("@Bol", bolNo, DbType.String);
            }
            const string query = @"SELECT * 
                                   FROM BalanceDues
                                   WHERE CompanyId=@CompanyId AND Bol=@Bol;";

            return _connection.Query<Arg.DataModels.BalanceDue>(query, parameters).ToList();
        }

        private ICacheManager<List<BOLHeaders>> _manager = CacheFactory.Build<List<BOLHeaders>>(Core.Settings.DefaultCacheSettings);

        public BOLHeaders GetResult(int queryId, int idx)
        {
            var results = GetResults(queryId, "");
            var bol = results.FirstOrDefault(x => x.Idx == idx) ?? new BOLHeaders();
            bol.ResultCount = results.Count;
            return bol;
        }

        public virtual List<BOLHeaders> GetResults(int queryId, string type)
        {
            var key = "cached-" + queryId;
            var results = _manager.Get(key);
            if (results == null || !results.Any())
            {
                var qr = new AgilityQueryResultsImpl().GetQueryResults(queryId);
                var searchOptions = Newtonsoft.Json.JsonConvert.DeserializeObject<Arg.DataModels.SearchOptions>(qr.QueryJson);

                if (type == "Stats")
                {
                    results = GetAgilityAuditResultStats(searchOptions, ActiveClient.Info.Name);
                    return results;
                }
                else if (type == "StatsByOrigin")
                {
                    results = GetAuditResultStatsByOrigin(searchOptions, ActiveClient.Info.Name);
                    return results;
                }
                else if (type == "StatsByPOL")
                {
                    results = GetAuditResultStatsByPOL(searchOptions, ActiveClient.Info.Name);
                    return results;
                }
                else if (type == "StatsByShipper")
                {
                    results = GetAuditResultStatsByShipper(searchOptions, ActiveClient.Info.Name);
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

        public List<BOLHeaders> GetResults(Arg.DataModels.SearchOptions so, bool Table = false)
        {
            if (so.ResultTableFormat == true)
            {
                return GetResults(so, null, true);
            }
            return GetResults(so, null);
        }

        public BOLHeaders GetResult(Arg.DataModels.SearchOptions so, int idx)
        {
            var results = GetResults(so);
            var bol = results.FirstOrDefault(x => x.Idx == idx) ?? new BOLHeaders();
            bol.ResultCount = results.Count;
            return bol;
        }

        public List<BOLHeaders> GetResults(Arg.DataModels.SearchOptions so, string sqlCmd, bool Table = false)
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
                var columns = Table ? "" : "*";

                sortingOrder = sortingOrder.Remove(sortingOrder.LastIndexOf(","));

                if (Table)
                {
                    columns = $"b.JobNumber, b.JobConfirmationDate, bh.Shipper, bh.Origin, bh.PortOfExit, bh.PortOfEntry, bh.Destination, bh.ServiceLevel, " +
                              $"bh.ServiceType, bh.ServiceMovementType, bh.Pieces, bh.GrossWeight, bh.ChargeableWeight, bh.FreightRevenue, bh.OtherRevenue, " +
                              $"(SELECT TOP(1) ast.EventType AS BOLViewed FROM {_dbName}.dbo.ActivityStats ast  WHERE ast.BolNo=b.JobNumber AND ast.EventType='BOLViewed' AND ast.ClientId={so.CompanyId}) AS BOLViewed";
                }
                else
                {
                    columns = @"b.JobNumber, bh.Origin, bh.Destination, bh.shipper, 
                                b.PortOfExit, b.PortOfEntry, b.JobConfirmationDate";
                }

                sql += $"SELECT DISTINCT *, ROW_NUMBER() OVER ({sortingOrder}) AS Idx FROM (SELECT DISTINCT {columns} FROM BookingHeaders b LEFT JOIN BOLHeaders bh on bh.JobNumber = b.JobNumber";
                sql += "WHERE b.JobNumber IS NOT NULL";

                BuildCmdWhereCondition1(ref sql, so);
                BuildCmdWhereCondition2(ref sql, so, Table);

                sql += @") AS SubQuery";
                sql += " ORDER BY Idx";

                if (so.CompanyId > 0)
                {
                    ActiveClient.Set(so.CompanyId);
                }
            }
            else
            {
                sql = sqlCmd;
            }

            return _connection.Query<BOLHeaders>(sql).ToList();
        }

        public List<BOLHeaders> GetAgilityAuditResultStats(Arg.DataModels.SearchOptions so, string clientName)
        {
            string cmd = @"SELECT bh.Shipper,p.ParticipantName,bh.Origin,bh.Destination,
                                  COUNT(DISTINCT b.JobNumber) AS ShipmentCount,
                                  MIN(FreightRevenue+OtherRevenue) AS MinCharges ,
                                  MAX(FreightRevenue+OtherRevenue) AS MaxCharges,
                                  (MAX(FreightRevenue+OtherRevenue) - MIN(FreightRevenue+OtherRevenue)) AS Difference,
                                  CAST(STDEV(FreightRevenue+OtherRevenue) AS numeric(18,2)) AS StandardDeviation
                                  FROM ClientAgility.dbo.BookingHeaders b
                                  LEFT JOIN ClientAgility.dbo.BOLHeaders bh ON b.JobNumber=bh.JobNumber
                                  LEFT JOIN clientagility.dbo.participants p ON p.ParticipantID =bh.Shipper";

            BuildCmdWhereCondition1(ref cmd, so);
            BuildCmdWhereCondition2(ref cmd, so);

            cmd += @"GROUP BY bh.Shipper,p.ParticipantName,bh.Origin,bh.Destination";
            cmd += @"ORDER BY StandardDeviation DESC";

            return _connection.Query<BOLHeaders>(cmd).ToList();
        }

        public List<BOLHeaders> GetAuditResultStatsByOrigin(Arg.DataModels.SearchOptions so, string clientName)
        {
            string cmd = @"SELECT bh.Origin,bh.Destination,
                                  (SELECT TOP 1 Shipper FROM BOLHeaders WHERE Origin=bh.Origin) AS Shipper,
                                  COUNT(DISTINCT b.JobNumber) AS ShipmentCount,
                                  MIN(FreightRevenue+OtherRevenue) AS MinCharges ,
                                  MAX(FreightRevenue+OtherRevenue) AS MaxCharges,
                                  (MAX(FreightRevenue+OtherRevenue) - MIN(FreightRevenue+OtherRevenue)) AS Difference,
                                  CAST(STDEV(FreightRevenue+OtherRevenue) AS numeric(18,2)) AS StandardDeviation
                                  FROM ClientAgility.dbo.BookingHeaders b
                                  LEFT JOIN ClientAgility.dbo.BOLHeaders bh ON b.JobNumber=bh.JobNumber
                                  LEFT JOIN clientagility.dbo.participants p ON p.ParticipantID =bh.Shipper";

            BuildCmdWhereCondition1(ref cmd, so);
            BuildCmdWhereCondition2(ref cmd, so);

            cmd += @"GROUP BY bh.Origin,bh.Destination";
            cmd += @"ORDER BY StandardDeviation DESC";

            return _connection.Query<BOLHeaders>(cmd).ToList();
        }

        public List<BOLHeaders> GetAuditResultStatsByPOL(Arg.DataModels.SearchOptions so, string clientName)
        {
            string cmd = @"SELECT bh.PortOfExit,
                                  (SELECT TOP 1 Shipper FROM BOLHeaders WHERE PortOfExit=bh.PortOfExit) AS Shipper,
                                  (SELECT TOP 1 Origin FROM BOLHeaders WHERE PortOfExit=bh.PortOfExit) AS Origin,
                                  (SELECT TOP 1 Destination FROM BOLHeaders WHERE PortOfExit=bh.PortOfExit) AS Destination,
                                  COUNT(DISTINCT b.JobNumber) AS ShipmentCount,
                                  MIN(FreightRevenue+OtherRevenue) AS MinCharges ,
                                  MAX(FreightRevenue+OtherRevenue) AS MaxCharges,
                                  (MAX(FreightRevenue+OtherRevenue) - MIN(FreightRevenue+OtherRevenue)) AS Difference,
                                  CAST(STDEV(FreightRevenue+OtherRevenue) AS numeric(18,2)) AS StandardDeviation
                                  FROM ClientAgility.dbo.BookingHeaders b
                                  LEFT JOIN ClientAgility.dbo.BOLHeaders bh ON b.JobNumber=bh.JobNumber
                                  LEFT JOIN clientagility.dbo.participants p ON p.ParticipantID =bh.Shipper";

            BuildCmdWhereCondition1(ref cmd, so);
            BuildCmdWhereCondition2(ref cmd, so);

            cmd += @"GROUP BY bh.PortOfExit";
            cmd += @"ORDER BY StandardDeviation DESC";

            return _connection.Query<BOLHeaders>(cmd).ToList();
        }

        public List<BOLHeaders> GetAuditResultStatsByShipper(Arg.DataModels.SearchOptions so, string clientName)
        {
            string cmd = @"SELECT bh.Shipper,
                                  (SELECT TOP 1 ParticipantName FROM Participants WHERE ParticipantID=bh.Shipper) AS ParticipantName,
                                  (SELECT TOP 1 Origin FROM BOLHeaders WHERE Shipper=bh.shipper) AS Origin,
                                  (SELECT TOP 1 Destination FROM BOLHeaders WHERE Shipper=bh.Shipper) AS Destination,
                                  COUNT(DISTINCT b.JobNumber) AS ShipmentCount,
                                  MIN(FreightRevenue+OtherRevenue) AS MinCharges ,
                                  MAX(FreightRevenue+OtherRevenue) AS MaxCharges,
                                  (MAX(FreightRevenue+OtherRevenue) - MIN(FreightRevenue+OtherRevenue)) AS Difference,
                                  CAST(STDEV(FreightRevenue+OtherRevenue) AS numeric(18,2)) as StandardDeviation
                                  FROM ClientAgility.dbo.BookingHeaders b
                                  LEFT JOIN ClientAgility.dbo.BOLHeaders bh ON b.JobNumber=bh.JobNumber
                                  LEFT JOIN clientagility.dbo.participants p ON p.ParticipantID =bh.Shipper";


            BuildCmdWhereCondition1(ref cmd, so);
            BuildCmdWhereCondition2(ref cmd, so);

            cmd += @"GROUP BY bh.Shipper";
            cmd += @"ORDER BY StandardDeviation DESC";

            return _connection.Query<BOLHeaders>(cmd).ToList();
        }

        public void BuildCmdWhereCondition1(ref string cmd, Arg.DataModels.SearchOptions so)
        {
            var depStartDateFormatted = so.DepartureStartDate.ToDateTime();
            var depEndDateFormatted = so.DepartureEndDate.ToDateTime();

            var loadstartDateFormatted = so.LoadStartDate.ToDateTime();
            var loadendDateFormatted = so.DepartureEndDate.ToDateTime();

            var JobStartDate = so.JobConfirmationStartDate.ToDateTime();
            var JobEndDate = so.JobConfirmationEndDate.ToDateTime();

            if (depStartDateFormatted != DateTime.MinValue && depEndDateFormatted != DateTime.MinValue)
            {
                var strDepartureStartDate = depStartDateFormatted.ToString("yyyy -MM-dd");
                var strDepartureEndDate = depEndDateFormatted.ToString("yyyy-MM-dd");
                cmd += $" AND b.DepartureDate BETWEEN {strDepartureStartDate} AND {strDepartureEndDate}";
            }

            if (loadstartDateFormatted != DateTime.MinValue && loadstartDateFormatted != DateTime.MinValue)
            {
                var strloadstartDateFormatted = loadstartDateFormatted.ToString("yyyy-MM-dd");
                var strloadendDateFormatted = loadendDateFormatted.ToString("yyyy-MM-dd");
                cmd += $" AND bh.LoadDate BETWEEN  {strloadstartDateFormatted} AND {strloadendDateFormatted}";
            }

            if (JobStartDate != DateTime.MinValue && JobEndDate != DateTime.MinValue)
            {
                var StrJobStartDate = JobStartDate.ToString("MM/dd/yy");
                var StrJobEndDate = JobEndDate.ToString("MM/dd/yy");
                cmd += $" AND b.JobConfirmationDate BETWEEN  {StrJobStartDate} AND {StrJobEndDate}";
            }

            if (so.ServiceMovementTypeCode != null)
            {
                cmd += $"AND b.ServiceMovementType IN ({string.Join(",", so.ServiceMovementTypeCode.Select(smt => $"'{smt}'"))})";
            }

            if (so.ServiceLevelCode != null)
            {
                cmd += $"AND b.ServiceLevel IN ({string.Join(",", so.ServiceLevelCode.Select(slc => $"'{slc}'"))})";
            }

            if (so.ServiceTypeCode != null)
            {
                cmd += $"AND b.ServiceType IN ({string.Join(",", so.ServiceTypeCode.Select(stc => $"'{stc}'"))})";
            }

            if (so.OriginLocationCode != null)
            {
                cmd += $"AND bh.Origin IN ({string.Join(",", so.OriginLocationCode.Select(olc => $"'{olc}'"))})";
            }

            if (so.DestinationLocationCode != null)
            {
                cmd += $"AND bh.Destination IN ({string.Join(",", so.DestinationLocationCode.Select(dlc => $"'{dlc}'"))})";
            }

            if (so.PortOfExitCode != null)
            {
                cmd += $"AND b.PortOfExit IN ({string.Join(",", so.PortOfExitCode.Select(poe => $"'{poe}'"))})";
            }

            if (so.PortofEntryCode != null)
            {
                cmd += $"AND b.PortofEntry IN ({string.Join(",", so.PortofEntryCode.Select(poe => $"'{poe}'"))})";
            }

            if (so.HazMatFlagCode != null)
            {
                var temp = "";
                var strOR = " OR ";

                foreach (var item in so.HazMatFlagCode)
                {
                    temp += $"(SELECT COUNT(*) FROM BookingItemDetails WHERE jobnumber=b.JobNumber AND HazmatFlag ='{item}') >0" + strOR;
                }
                temp = temp.Remove(temp.Length - strOR.Length, strOR.Length);
                cmd += $"({temp})";
            }

            if (so.TempFromZone != null)
            {
                var temp = "";
                temp += $"(SELECT COUNT(*) FROM BookingItemDetails WHERE jobnumber=b.JobNumber AND TempZoneFrom >= {so.TempFromZone} and TempZoneTo <= {so.TempFromZone}) > 0";
                cmd += $"({temp})";
            }

            if (so.UnitTypeCode != null)
            {
                var temp = "";
                var strOR = " OR ";

                temp += " (SELECT COUNT(*) FROM BookingItemDetails WHERE jobnumber=b.JobNumber AND (";

                foreach (var item in so.UnitTypeCode)
                {
                    temp += $"UnitType = {item}" + strOR;
                }
                temp = temp.Remove(temp.Length - strOR.Length, strOR.Length);
                temp += " )) >0";
                cmd += $"({temp})";
            }

            if (so.ShipperID != null)
            {
                cmd += $"AND bh.shipper IN ({string.Join(",", so.ShipperID.Select(si => $"'{si}'"))})";
            }

            if (so.ConsigneeID != null)
            {
                cmd += $"AND bh.Consignee IN ({string.Join(",", so.ConsigneeID.Select(ci => $"'{ci}'"))})";
            }

            if (so.NotifyPartyCode != null)
            {
                cmd += $"AND bh.NotifyParty IN ({string.Join(",", so.NotifyPartyCode.Select(npc => $"'{npc}'"))})";
            }

            if (so.ExportingCarrierCode != null)
            {
                cmd += $"AND bh.ExportingCarrier IN ({string.Join(",", so.ExportingCarrierCode.Select(ecc => $"'{ecc}'"))})";
            }

            if (so.PrepaidCollectCode != null)
            {
                cmd += $"AND bh.PrepaidCollect IN ({string.Join(",", so.PrepaidCollectCode.Select(pcc => $"'{pcc}'"))})";
            }

        }

        public void BuildCmdWhereCondition2(ref string cmd, Arg.DataModels.SearchOptions so, bool Table = false)
        {
            if (!string.IsNullOrWhiteSpace(so.JobNumber))
            {
                cmd += $"AND b.JobNumber = {so.JobNumber}";
            }

            if (!string.IsNullOrWhiteSpace(so.BOLNo))
            {
                var temp = "";
                temp += $" (SELECT COUNT(*) FROM BookingHeaders bh WHERE bh.JobNumber=b.JobNumber AND bh.BookingReference LIKE '%{so.BOLNo}%') > 0";
                cmd += $" AND ({temp})";
            }

            if (!string.IsNullOrWhiteSpace(so.ConsignmentID))
            {
                cmd += $"AND b.ConsignmentID = {so.ConsignmentID}";
            }

            if (!string.IsNullOrEmpty(so.unitNumber))
            {
                var temp = "";
                temp += $" (SELECT COUNT(*) FROM BookingItemDetails bid WHERE bid.JobNumber=b.JobNumber AND bid.UnitNumber LIKE '%{so.unitNumber}%') > 0";
                cmd += $" AND ({temp})";
            }

            if (!string.IsNullOrEmpty(so.ImageType))
            {
                var temp = "";
                temp += $" (SELECT COUNT(*) FROM DocumentImages d WHERE d.JobNumber=b.JobNumber AND d.Type LIKE '%{so.ImageType}%') > 0";
                cmd += $" AND ({temp})";
            }

            if (!string.IsNullOrWhiteSpace(so.BOLViews))
            {
                var temp = "";
                string list = Utility.JoinStrings(so.UserIds.ToArray());

                if (list.Contains("00000000-0000-0000-0000-000000000000"))
                {
                    temp += $"(SELECT COUNT(*) AS ViewCount FROM {_dbName}.dbo.ActivityStats d WHERE d.BolNo = b.JobNumber ) <= {Convert.ToInt32(so.BOLViews)}";
                }
                else
                {
                    temp += $"(SELECT COUNT(*) AS ViewCount FROM {_dbName}.dbo.ActivityStats d WHERE d.UserId IN ({list}) AND d.BolNo = b.JobNumber ) <= {Convert.ToInt32(so.BOLViews)}";
                }

                cmd += $" AND ({temp})";
            }

            if (so.EliminateBDResearchItems)
            {
                cmd += $"AND b.JobNumber NOT IN (SELECT d.Bol from {_dbName}.dbo.BalanceDues d WHERE d.CompanyId= {so.CompanyId} UNION SELECT r.Bol FROM {_dbName}.dbo.ResearchItems r WHERE r.CompanyId= {so.CompanyId})";
            }
        }
    }
}
