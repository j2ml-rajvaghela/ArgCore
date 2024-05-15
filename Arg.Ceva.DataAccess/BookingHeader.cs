using Arg.DataAccess;
using CacheManager.Core;
using CustomExtensions;
using Dapper;
using Dapper.Contrib.Extensions;
using MySqlX.XDevAPI.Relational;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlTypes;
using System.Xml.Linq;

namespace Arg.Ceva.DataAccess
{
    public class BookingHeader
    {
        [Table("BookingHeader")]
        public class BookingHeaderImp
        {
            [Key]
            public string Region { get; set; }
            public DateTime? CREATEDATE { get; set; }
            public string MODE { get; set; }
            public string TYPE { get; set; }
            public string BRANCH { get; set; }
            public string MATRSERV { get; set; }
            public string BOKPRT { get; set; }
            public string DOSSIER { get; set; }
            public string PCCLOCA { get; set; }
            public DateTime? PCCDATE { get; set; }
            public string MATRCLOCA { get; set; }
            public string CNTRYCODE { get; set; }
            public DateTime? MATRCDATE { get; set; }
            public string MATRDLOCA { get; set; }
            public string CNTRYCOD01 { get; set; }
            public DateTime? MATRDDATE { get; set; }
            public string ONCDLOCA { get; set; }
            public DateTime? ONCDDATE { get; set; }
            public string CSEENO { get; set; }
            public string CSORNO { get; set; }
            public string AWBNO { get; set; }
            public string HAWBNO { get; set; }
            public string HOUSENO { get; set; }
            public int? PCKGS { get; set; }
            public string PCKGSCODE { get; set; }
            public decimal? WGHT { get; set; }
            public decimal? MEAS { get; set; }
            public decimal? CHRGWGHT { get; set; }
            public string MEASUNITTP { get; set; }
            public decimal? WGHTLB { get; set; }
            public decimal? CHRGWGHTLB { get; set; }
            public string GOODSDSC { get; set; }
            public string GOODSDSC2 { get; set; }
            public string GOODSDSC3 { get; set; }
            public string GOODSDSC4 { get; set; }
            public string GOODSDSC5 { get; set; }
            public string GOODSDSC6 { get; set; }
            public string GOODSDSC7 { get; set; }
            public string GOODSDSC8 { get; set; }
            public string GOODSDSC9 { get; set; }
            public string GOODSDSC10 { get; set; }
            public string GOODSDSC11 { get; set; }
            public string GOODSDSC12 { get; set; }
            public string TODCODE { get; set; }
            public string SERVLEVEL { get; set; }
            public string SERVTYPE { get; set; }
            public string GOODSTYPE { get; set; }
            public string MAINFLGH { get; set; }
            public string CARR { get; set; }
            public string MARKS { get; set; }
            public string REMARKS { get; set; }
            public string REMARKS2 { get; set; }
            public string REMARKS3 { get; set; }
            public string REMARKS4 { get; set; }
            public string SADCREF { get; set; }
            public string PRODUCT1 { get; set; }
            public string PRODUCT2 { get; set; }
            public string AIRSTATUS { get; set; }
            public string FWDSTATUS { get; set; }
            public string CARRCODE { get; set; }
            public string VESSCODE { get; set; }
            public string VOYNO { get; set; }
            public string HBLNO { get; set; }
            public string LBLNO { get; set; }
            public string LSONO { get; set; }
            public string HANDLNO { get; set; }
            public string HANDLREM { get; set; }
            public string HANDLREM2 { get; set; }
            public string HANDLREM3 { get; set; }
            public string CLAUSNO { get; set; }
            public string CLAUSREM1 { get; set; }
            public string CLAUSREM2 { get; set; }
            public string CLAUSREM3 { get; set; }
            public string CLAUSREM4 { get; set; }
            public string CLAUSREM5 { get; set; }
            public string CLAUSREM6 { get; set; }
            public string CLAUSREM7 { get; set; }
            public string CLAUSREM8 { get; set; }
            public string HAULNO { get; set; }
            public string HAULNADR1 { get; set; }
            public string SERVLVL { get; set; }
            public string COBOKPRT { get; set; }
            public string PRECONSOL { get; set; }
            public string MAINCONSOL { get; set; }
            public string ONCONSOL { get; set; }
            public string PAYRNO { get; set; }
            public string PCSERV { get; set; }
            public string ONCSERV { get; set; }
            public string PRECONHANO { get; set; }
            public string PRECONHANA { get; set; }
            public string MNCONHANO { get; set; }
            public string MNCONHANA { get; set; }
            public string ONCONHANO { get; set; }
            public string ONCONHANA { get; set; }
            public string SHPRNO { get; set; }
            public string CSETCODE { get; set; }
            public string KNOWNSHIP { get; set; }
            public DateTime? AddedOn { get; set; }

            [Computed]
            public string POLCode { get; set; }

            [Computed]
            public string POL { get; set; }

            [Computed]
            public string OriginCountryCode { get; set; }

            [Computed]
            public string OriginCountryName { get; set; }

            [Computed]
            public string DestinationCountryCode { get; set; }

            [Computed]
            public string DestinationCountryName { get; set; }

            [Computed]
            public string PODCode { get; set; }

            [Computed]
            public string POD { get; set; }

            [Computed]
            public string Consignee { get; set; }

            [Computed]
            public string PackageType { get; set; }

            [Computed]
            public string OceanCompanyName { get; set; }

            [Computed]
            public string AirCompanyName { get; set; }

            [Computed]
            public int Idx { get; set; }

            //[Computed]
            //public string Mode { get; set; }
            [Computed]
            public int ResultCount { get; set; }

            [Computed]
            public string ParticipantName { get; set; }

            [Computed]
            public string INVCURAMT { get; set; }

            [Computed]
            public string BookingType { get; set; }

            [Computed]
            public decimal GrossRate { get; set; }

            [Computed]
            public decimal NetRate { get; set; }

            [Computed]
            public string BookingTypeDescription { get; set; }

            [Computed]
            public string IATACodeDescription { get; set; }

            [Computed]
            public string SCACCompanyName { get; set; }

            [Computed]
            public string SERVLEVELDescription { get; set; }

            [Computed]
            public string SERVLVLDescription { get; set; }

            [Computed]
            public string PickLocationName { get; set; }

            [Computed]
            public string DelLocationName { get; set; }

            [Computed]
            public string CountryName { get; set; }

            [Computed]
            public string CountryName01 { get; set; }

            [Computed]
            public string PODDetail { get; set; }

            [Computed]
            public string POLDetail { get; set; }

            [Computed]
            public string Shipper { get; set; }

            [Computed]
            public string PartConsignee { get; set; }

            [Computed]
            public string Payor { get; set; }

            [Computed]
            public string PackagingCode { get; set; }

            [Computed]
            public string GoodType { get; set; }

            [Computed]
            public string ContainerType { get; set; }

            [Computed]
            public string ServiceType { get; set; }

            [Computed]
            public string GOODSDESC { get; set; }

            [Computed]
            public string BOLViewed { get; set; }

            [Computed]
            public string Bol { get; set; }

            [Computed]
            public string Voyage { get; set; }

            [Computed]
            public string TotalCharges { get; set; }

            [Computed]
            public string OriginStation { get; set; }

            [Computed]
            public int ShipmentCount { get; set; }

            [Computed]
            public decimal MinCharges { get; set; }

            [Computed]
            public decimal MaxCharges { get; set; }

            [Computed]
            public decimal Difference { get; set; }

            [Computed]
            public decimal StandardDeviation { get; set; }

            [Computed]
            public string Customer { get; set; }

            [Computed]
            public string CustomerID { get; set; }

            [Computed]
            public string Currency { get; set; }

            [Computed]
            public string INVOICENO { get; set; }

            [Computed]
            public string HBLNo_HAWBNo { get; set; }

            [Computed]
            public string DEBTOR { get; set; }

            [Computed]
            public string BOLNo { get; set; }

            [Computed]
            public string InvoiceType { get; set; }
        }

        private string _dbName = Common.DBName;

        public List<BookingHeaderImp> GetRegions()
        {
            const string query = @"SELECT DISTINCT Region FROM BookingHeader
                                   ORDER BY Region;";

            using (var connection = Common.ClientDatabase)
            {
                var regions = connection.Query<BookingHeaderImp>(query, commandType: CommandType.Text).ToList();
                return regions;
            }
        }

        public BookingHeaderImp GetBookingHeader(string bolNo)
        {
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(bolNo))
            {
                parameters.Add("@AWBNO", bolNo, DbType.String);
                parameters.Add("@HOUSENO", bolNo, DbType.String);
                parameters.Add("@HBLNO", bolNo, DbType.String);
                parameters.Add("@HAWBNO", bolNo, DbType.String);
                parameters.Add("@LBLNO", bolNo, DbType.String);

            } 
            const string query = @"SELECT * FROM BookingHeader 
                                   WHERE AWBNO=@AWBNO OR HOUSENO=@HOUSENO OR HBLNO=@HBLNO OR HAWBNO=@HAWBNO OR LBLNO=@LBLNO;";

            using (var connection = Common.ClientDatabase)
            {
                var bookingHeader = connection.QueryFirstOrDefault<BookingHeaderImp>(query, parameters);
                return bookingHeader;
            }
        }

        public List<BookingHeaderImp> GetBookingBranch()
        {
            const string query = @"SELECT DISTINCT BRANCH FROM BookingHeader
                                   ORDER BY BRANCH;";

            using (var connection = Common.ClientDatabase)
            {
                var bookingBranches = connection.Query<BookingHeaderImp>(query, commandType: CommandType.Text).ToList();
                return bookingBranches;
            }

        }

        public List<BookingHeaderImp> GetOriginCountryCode()
        {
            const string query = @"SELECT CONCAT(xc.Name,' (',xc.Code,')') AS OriginCountryName,bh.CNTRYCODE AS OriginCountryCode FROM BookingHeader bh
                                   INNER JOIN XrefCountries xc ON bh.CNTRYCODE=xc.Code
                                   GROUP BY CONCAT(xc.Name,' (',xc.Code,')'),bh.CNTRYCODE
                                   ORDER BY CONCAT(xc.Name,' (',xc.Code,')'),bh.CNTRYCODE;";

            using (var connection = Common.ClientDatabase)
            {
                var originCountryCodes = connection.Query<BookingHeaderImp>(query, commandType: CommandType.Text).ToList();
                return originCountryCodes;
            }
        }

        public List<BookingHeaderImp> GetDestinationCountryCode()
        {
            const string query = @"SELECT CONCAT(xc.Name,' (',xc.Code,')') AS DestinationCountryName,bh.CNTRYCOD01 AS DestinationCountryCode FROM BookingHeader bh
                                   INNER JOIN XrefCountries xc ON bh.CNTRYCOD01=xc.Code
                                   GROUP BY CONCAT(xc.Name,' (',xc.Code,')'),bh.CNTRYCOD01
                                   ORDER BY CONCAT(xc.Name,' (',xc.Code,')'),bh.CNTRYCOD01;";

            using (var connection = Common.ClientDatabase)
            {
                var destinationCountryCodes = connection.Query<BookingHeaderImp>(query, commandType: CommandType.Text).ToList();
                return destinationCountryCodes;
            }
        }

        public List<BookingHeaderImp> GetDistinctPOL()
        {
            const string query = @"SELECT DISTINCT CONCAT(l.Name,' (',h.MATRCLOCA,')') AS POL, h.MATRCLOCA AS POLCode FROM BookingHeader h
                                   LEFT JOIN Locations l ON h.MATRCLOCA=l.LocationID
                                   WHERE h.MATRCLOCA <> ''
                                   ORDER BY POL;";

            using (var connection = Common.ClientDatabase)
            {
                var distinctPOLs = connection.Query<BookingHeaderImp>(query, commandType: CommandType.Text).ToList();
                return distinctPOLs;
            }
        }

        public List<BookingHeaderImp> GetDistinctPOD()
        {
            const string query = @"SELECT DISTINCT CONCAT(l.Name,' (',h.MATRDLOCA,')') AS POD, h.MATRDLOCA AS PODCode FROM BookingHeader h
                                   LEFT JOIN Locations l ON h.MATRDLOCA=l.LocationID
                                   WHERE h.MATRDLOCA <> ''
                                   ORDER BY POD;";

            using (var connection = Common.ClientDatabase)
            {
                var distinctPODs = connection.Query<BookingHeaderImp>(query, commandType: CommandType.Text).ToList();
                return distinctPODs;
            }
        }

        public List<BookingHeaderImp> GetOceanServiceType()
        {
            const string query = @"SELECT DISTINCT ONCSERV FROM BookingHeader 
                                   WHERE ONCSERV <> ''
                                   ORDER BY ONCSERV;";

            using (var connection = Common.ClientDatabase)
            {
                var oceanServiceTypes = connection.Query<BookingHeaderImp>(query, commandType: CommandType.Text).ToList();
                return oceanServiceTypes;
            }
        }

        public List<BookingHeaderImp> GetDistinctConsignee()
        {
            const string query = @"SELECT CONCAT(p.Name,' (',h.CSEENO,')') AS Consignee,h.CSEENO FROM BookingHeader h
                                   INNER JOIN Participants p ON p.ParticipantID=h.CSEENO
                                   WHERE h.CSEENO <> '' AND p.Type= 'Consignor'
                                   GROUP BY CONCAT(p.Name,' (',h.CSEENO,')'),h.CSEENO
                                   ORDER BY CONCAT(p.Name,' (',h.CSEENO,')');";

            using (var connection = Common.ClientDatabase)
            {
                var distinctConsignees = connection.Query<BookingHeaderImp>(query, commandType: CommandType.Text).ToList();
                return distinctConsignees;
            }
        }

        public List<BookingHeaderImp> GetCustomer()
        {
            using (var connection = Common.ClientDatabase)
            {
                var customers = connection.Query<BookingHeaderImp>("GetAllCustomer", commandType: CommandType.StoredProcedure).ToList();
                return customers;

            }
        }

        public List<BookingHeaderImp> GetShipper()
        {
            const string query = @"SELECT CONCAT(p.Name,' (',h.SHPRNO,')') AS Consignee,h.SHPRNO FROM BookingHeader h
                                   INNER JOIN Participants p ON p.ParticipantID=h.SHPRNO
                                   WHERE h.SHPRNO <> '' AND p.Type= 'Shipper'
                                   GROUP BY CONCAT(p.Name,' (',h.SHPRNO,')'),h.SHPRNO
                                   ORDER BY CONCAT(p.Name,' (',h.SHPRNO,')');";

            using (var connection = Common.ClientDatabase)
            {
                var shippers = connection.Query<BookingHeaderImp>(query, commandType: CommandType.Text).ToList();
                return shippers;
            }
        }

        public List<BookingHeaderImp> GetPackageType()
        {
            const string query = @"SELECT CONCAT(p.DESCRIPTION,' (',h.PCKGSCODE,')') AS PackageType, h.PCKGSCODE FROM BookingHeader h
                                   INNER JOIN XrefPackagingCodes p ON p.CODE=h.PCKGSCODE
                                   WHERE h.PCKGSCODE <> ''
                                   GROUP BY h.PCKGSCODE, CONCAT(p.DESCRIPTION, ' (', h.PCKGSCODE, ')')
                                   ORDER BY CONCAT(p.DESCRIPTION,' (',h.PCKGSCODE,')');";

            using (var connection = Common.ClientDatabase)
            {
                var packageTypes = connection.Query<BookingHeaderImp>(query, commandType: CommandType.Text).ToList();
                return packageTypes;
            }
        }

        public List<BookingHeaderImp> GetOceanCarrier()
        {
            const string query = @"SELECT CONCAT(o.CompanyName,' (',h.CARRCODE,')') AS OceanCompanyName,h.CARRCODE FROM BookingHeader h
                                   INNER JOIN XrefOceanCarriers o ON o.SCAC=h.CARRCODE
                                   WHERE h.CARRCODE <> ''
                                   GROUP BY h.CARRCODE, CONCAT(o.CompanyName,' (',h.CARRCODE,')')
                                   ORDER BY CONCAT(o.CompanyName,' (',h.CARRCODE,')');";

            using (var connection = Common.ClientDatabase)
            {
                var oceanCarriers = connection.Query<BookingHeaderImp>(query, commandType: CommandType.Text).ToList();
                return oceanCarriers;
            }
        }

        public List<BookingHeaderImp> GetAirCarrier()
        {
            const string query = @"SELECT CONCAT(a.CompanyName,' (',h.CARR,')') AS AirCompanyName,h.CARR FROM BookingHeader h
                                   INNER JOIN XrefAirCarriers a ON a.IATACode=h.CARR
                                   WHERE h.CARR <> ''
                                   GROUP BY CONCAT(a.CompanyName,' (',h.CARR,')'),h.CARR
                                   ORDER BY CONCAT(a.CompanyName,' (',h.CARR,')');";

            using (var connection = Common.ClientDatabase)
            {
                var airCarriers = connection.Query<BookingHeaderImp>(query, commandType: CommandType.Text).ToList();
                return airCarriers;
            }
        }

        public List<BookingHeaderImp> GetAirServiceType()
        {
            const string query = @"SELECT Distinct SERVTYPE FROM BookingHeader 
                                   WHERE SERVTYPE <> ''
                                   ORDER BY SERVTYPE;";

            using (var connection = Common.ClientDatabase)
            {
                var airServiceTypes = connection.Query<BookingHeaderImp>(query, commandType: CommandType.Text).ToList();
                return airServiceTypes;
            }
        }

        public List<BookingHeaderImp> GetBookingType()
        {
            const string query = @"SELECT CONCAT(b.Description,' (',h.TYPE,')') AS BookingType,h.TYPE FROM BookingHeader h
                                   INNER JOIN XrefBookingType b ON b.BookingType=h.TYPE
                                   WHERE h.TYPE <> ''
                                   GROUP BY CONCAT(b.Description,' (',h.TYPE,')'),h.TYPE
                                   ORDER BY CONCAT(b.Description,' (',h.TYPE,')');";

            using (var connection = Common.ClientDatabase)
            {
                var bookingTypes = connection.Query<BookingHeaderImp>(query, commandType: CommandType.Text).ToList();
                return bookingTypes;
            }
        }

        public int GetResultCount(SearchOptions so)
        {
            const string query = @"SELECT COUNT(*) AS ResultCount FROM BookingHeader b;";
            FixWildcards(ref so);

            using (var connection = Common.ClientDatabase)
            {
                var resultCount = Convert.ToInt32(connection.ExecuteScalar<object>(query));
                return resultCount;
            }

        }

        public void FixWildcards(ref SearchOptions so)
        {
            so.AWBNO = Common.WildCardSearchToNormal(so.AWBNO);
            so.HAWBNO = Common.WildCardSearchToNormal(so.HAWBNO);
            so.HOUSENO = Common.WildCardSearchToNormal(so.HOUSENO);
            so.HBLNO = Common.WildCardSearchToNormal(so.HBLNO);
            so.Bol = Common.WildCardSearchToNormal(so.Bol);

            so.GoodsDescription = Common.WildCardSearchToNormal(so.GoodsDescription);
            so.BookingRemarks = Common.WildCardSearchToNormal(so.BookingRemarks);
            so.HandlingRemarks = Common.WildCardSearchToNormal(so.HandlingRemarks);
            so.ChargeDesc = Common.WildCardSearchToNormal(so.ChargeDesc);
            so.ContainerId = Common.WildCardSearchToNormal(so.ContainerId);
            //so.ImageType = Common.WildCardSearchToNormal(so.ImageType);
        }

        public List<Generic> GetBOLCustomers()
        {
            const string query = @"SELECT DISTINCT CONCAT(p.Name,' (',ih.DEBTOR,')') AS Name,ih.DEBTOR AS StrId FROM InvoiceCharges ih
                                   INNER JOIN Participants p ON p.ParticipantID=ih.DEBTOR AND p.Region=ih.Region 
                                   WHERE ih.DEBTOR <> '' AND p.Type = 'customer'
                                   ORDER BY Name;";

            using (var connection = Common.ClientDatabase)
            {
                var bolCustomers = connection.Query<Generic>(query, commandType: CommandType.Text).ToList();
                return bolCustomers;
            }
        }

        public List<Generic> GetBalanceDueCustomersCeva(int companyId)
        {
            var parameters = new DynamicParameters();

            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }
            const string query = @"SELECT DISTINCT c.CustomerId AS StrId,CONCAT(c.customername,' (',c.CustomerId, ')') AS Name FROM [BalanceDues.Customers] c
                                   LEFT JOIN BalanceDues b ON b.CustomerId=c.CustomerId
                                   WHERE b.CompanyId = @CompanyId
                                   ORDER BY NAME;";

            using (var connection = Common.Database)
            {
                var balanceDueCustomersCeva = connection.Query<Generic>(query, parameters).ToList();
                return balanceDueCustomersCeva;
            }
        }

        public Generic GetCustomersbyBOL(string bol)
        {
            //var parameters = new DynamicParameters();

            //parameters.Add("@HAWBNO", bol, DbType.String);
            //parameters.Add("@HOUSENO", bol, DbType.String);
            //parameters.Add("@HBLNO", bol, DbType.String);
            const string query = @"SELECT TOP 1 CONCAT(p.Name,' (',ih.DEBTOR,')') AS Name,ih.DEBTOR AS StrId FROM InvoiceCharges ih
                                   INNER JOIN Participants p ON p.ParticipantID=ih.DEBTOR AND p.Region=ih.Region
                                   WHERE ih.DEBTOR <> '' AND p.Type = 'customer' AND ih.HAWBNO=@HAWBNO OR ih.HOUSENO=@HOUSENO OR ih.HBLNO=@HBLNO
                                   ORDER BY Name;";

            using (var connection = Common.ClientDatabase)
            {
                var customersbyBOL = connection.QueryFirstOrDefault<Generic>(query, new { @HAWBNO = bol, @HOUSENO = bol, @HBLNO = bol });
                return customersbyBOL;
            }
        }

        public List<BookingHeaderImp> GetAllModes()
        {
            const string query = @"SELECT DISTINCT CASE WHEN SERVTYPE = '' OR SERVTYPE IS NULL THEN ONCSERV ELSE SERVTYPE END AS Mode FROM BookingHeader
                                   WHERE SERVTYPE <> '' OR ONCSERV <> ''
                                   ORDER BY Mode;";

            using (var connection = Common.ClientDatabase)
            {
                var allModes = connection.Query<BookingHeaderImp>(query, commandType: CommandType.Text).ToList();
                return allModes;
            }

        }

        private ICacheManager<List<BookingHeaderImp>> _manager = CacheFactory.Build<List<BookingHeaderImp>>(Arg.Core.Settings.DefaultCacheSettings);

        public virtual List<BookingHeaderImp> GetResults(int queryId, string type)
        {
            var key = "cached-" + queryId;
            var results = _manager.Get(key);

            if (results == null || !results.Any())
            {
                var qr = new QueryResultsImpl().GetQueryResults(queryId);
                var searchOptions = JsonConvert.DeserializeObject<SearchOptions>(qr.QueryJson);

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

        public BookingHeaderImp GetResult(int queryId, int idx)
        {
            var results = GetResults(queryId, "");
            var bol = results.FirstOrDefault(x => x.Idx == idx) ?? new BookingHeaderImp();
            bol.ResultCount = results.Count;
            return bol;
        }

        public BookingHeaderImp GetResults(string sqlCmd, int idx)
        {
            var results = GetResults(null, sqlCmd, false);
            var bol = results.FirstOrDefault(x => x.Idx == idx) ?? new BookingHeaderImp();
            bol.ResultCount = results.Count;
            return bol;
        }

        public List<BookingHeaderImp> GetResults(SearchOptions so, bool Table = false)
        {
            if (Table)
            {
                return GetResults(so, null, true);
            }
            return GetResults(so, null);
        }

        public List<BookingHeaderImp> GetResults(SearchOptions so, string sqlCmd, bool Table = false)
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

                if (!Table)
                {
                    var columns = @"b.*,
                        (SELECT TOP(1) ISNULL(Name,'') FROM Participants p WHERE b.SHPRNO=p.ParticipantID) AS ParticipantName,
                        (SELECT TOP 1 ih.DEBTOR FROM InvoiceHeader ih WHERE b.BOKPRT=ih.BLPRT) AS Customer,
                        (SELECT TOP 1 LEFT(b.hblno,3) + '  ' +  ISNULL((l.Name ),'') AS OriginStation FROM Locations l) AS OriginStation";

                    sql = $"SELECT b.* FROM (SELECT b.*,ROW_NUMBER() OVER ({sortingOrder}) AS Idx FROM(SELECT {columns} FROM BookingHeader b) AS b";

                    FixWildcards(ref so);
                    BuildCmdInnerJoin2(ref sql, so, Table);
                    BuildCmdWhereCondition1(ref sql, so);
                    BuildCmdWhereCondition2(ref sql, so, Table);

                    sql += ") AS b";

                }
                else
                {
                    var columns = @"SELECT b.*, CAST((TotalCharges/CHRGWGHT ) AS numeric(18,2)) AS NetRate FROM
                                           (SELECT b.CNTRYCODE,b.BOKPRT,(Select top 1 ih.DEBTOR FROM InvoiceHeader ih WHERE b.BOKPRT=ih.BLPRT) AS Customer,
                                           'HBLNo_HAWBNo' = CASE WHEN b.HBLNO IS NOT NULL AND b.hblno<>'' THEN b.HBLNO ELSE b.HAWBNO END, 
                                            ba.INVOICENO, b.[MATRCDATE], ISNULL(p.Name,'') + ' (' +b.PAYRNO + ')' AS ParticipantName,
                                            MATRCLOCA, MATRDLOCA, SERVLEVEL, 'ServiceType' = CASEA WHEN [ONCSERV] IS NOT NULL AND [ONCSERV] <>'' THEN [ONCSERV] ELSE SERVTYPE END,
                                            ISNULL(b.PCKGSCODE,'') AS PCKGSCODE, b.WGHT, 'CHRGWGHT' = CAST(CASE  WHEN  b.CHRGWGHT <=0  OR b.CHRGWGHT IS NULL  THEN .1 
                                            ELSE b.CHRGWGHT END AS NUMERIC(18,1)),
                                            (ISNULL(b.GOODSDSC,'') +' '+ ISNULL(b.GOODSDSC2,'') + ' ' + ISNULL(b.GOODSDSC3,'') + ' ' + ISNULL(b.GOODSDSC4,'') + ' ' + 
                                            ISNULL(b.GOODSDSC5,'') + ' ' + ISNULL(GOODSDSC6,'') + ' ' + ISNULL(GOODSDSC7,'') + ' ' + ISNULL(GOODSDSC8,'') + ' ' + 
                                            ISNULL(GOODSDSC9,'') + ' ' +  ISNULL(b.GOODSDSC10,'') + ' ' +
                                            ISNULL(b.GOODSDSC11,'') + ' ' + ISNULL(b.GOODSDSC12,'')) AS GOODSDESC, 
                                            CAST(sum(isnull(ba.INVCURAMT,0)) AS NUMERIC(18,2)) AS TotalCharges, ba.CURR as Currency
                                            FROM bookingheader b
                                            LEFT JOIN invoicecharges ba  ON b.BOKPRT=ba.BLPRT
                                            LEFT JOIN Participants p ON p.ParticipantID =ba.DEBTOR  AND P.Type='Customer'";

                    sql = columns;
                    FixWildcards(ref so);
                    FixWildcards(ref so);
                    BuildCmdInnerJoin2(ref sql, so, Table);
                    BuildCmdWhereCondition1(ref sql, so);
                    BuildCmdWhereCondition2(ref sql, so, Table);

                    sql += "GROUP BY  b.HBLNO, b.HAWBNO,ba.INVOICENO,b.[MATRCDATE],b.MATRCLOCA, MATRDLOCA, ONCSERV,SERVTYPE, SERVLEVEL,p.Name, b.PAYRNO," +
                           "b.PCKGSCODE,b.WGHT,b.CHRGWGHT,GOODSDSC, GOODSDSC2, +GOODSDSC3, GOODSDSC4, GOODSDSC5, GOODSDSC6, GOODSDSC7, GOODSDSC8, GOODSDSC9, " +
                           "b.GOODSDSC10, b.GOODSDSC11, b.GOODSDSC12, ba.CURR,b.CNTRYCODE,b.BOKPRT) b  " + sortingOrder + "";
                }

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
                var results = connection.Query<BookingHeaderImp>(sql).ToList();
                return results;
            }
        }

        public List<BookingHeaderImp> GetAuditResultStats(SearchOptions so, string clientName)
        {
            string cmd = @"SELECT MIN(b.CustomerID) AS CustomerID, ISNULL(b.Customer,'No invoice') AS Customer,b.MATRCLOCA,b.MATRDLOCA ,COUNT(*) AS ShipmentCount, 
                                   MIN(shipmentcharges) AS MinCharges, MAX(shipmentcharges) AS MaxCharges, (MAX(shipmentcharges) - MIN(shipmentcharges)) AS Difference,
                                   CAST(ISNULL(STDEV(shipmentcharges),0) AS numeric(18,2)) AS StandardDeviation,Currency FROM
                                   (SELECT MIN(p.ParticipantID) AS CustomerID, (ISNULL(LEFT(P.Name,10),' ') + ' (' + I.DEBTOR +  ')') AS Customer,
                                   b.MATRCLOCA AS MATRCLOCA,B.MATRDLOCA AS MATRDLOCA, B.BOKPRT, SUM(ISNULL(ic.INVCURAMT,0)) AS ShipmentCharges, ISNULL(ic.CURR,'') AS Currency
                                   FROM BookingHeader b
                                   LEFT JOIN invoiceheader i ON b.BOKPRT=i.BLPRT
                                   LEFT JOIN Participants p ON i.debtor=p.ParticipantID AND p.Type='Customer'
                                   LEFT JOIN InvoiceCharges ic ON ic.BLPRT=i.BLPRT AND i.DEBTOR=ic.DEBTOR AND i.INVOICENO=ic.INVOICENO";
            FixWildcards(ref so);
            BuildCmdInnerJoin3(ref cmd, so);
            BuildCmdWhereCondition1(ref cmd, so);
            BuildCmdWhereCondition2(ref cmd, so);

            cmd += @"GROUP BY B.BOKPRT,i.DEBTOR,p.Name,B.MATRCLOCA,B.MATRDLOCA,ic.CURR) b";
            cmd += @"GROUP BY b.Customer,b.MATRCLOCA,b.MATRDLOCA,Currency";
            cmd += @"ORDER BY b.Customer,b.MATRCLOCA,b.MATRDLOCA";

            using (var connetion = Common.ClientDatabase)
            {
                var resultStats = connetion.Query<BookingHeaderImp>(cmd).ToList();
                return resultStats;
            }
        }

        public List<BookingHeaderImp> GetAuditResultStatsByOrigin(SearchOptions so, string clientName)
        {
            string cmd = @"SELECT ISNULL(b.MATRCLOCA,'') AS MATRCLOCA,ISNULL(b.MATRDLOCA,'') AS MATRDLOCA,COUNT(*) AS ShipmentCount, MIN(shipmentcharges) AS MinCharges, 
                                  MAX(shipmentcharges) AS MaxCharges,(MAX(shipmentcharges) - MIN(shipmentcharges)) AS Difference,CAST(ISNULL(STDEV(shipmentcharges),0) AS numeric(18,2))
                                  AS StandardDeviation,Currency FROM
                                  (SELECT (ISNULL(LEFT(P.Name,10),' ') + ' (' + I.DEBTOR +  ')') AS Customer,b.MATRCLOCA AS MATRCLOCA,B.MATRDLOCA AS MATRDLOCA, B.BOKPRT, 
                                  SUM(ISNULL(ic.INVCURAMT,0)) AS ShipmentCharges, ISNULL(ic.CURR,'') AS Currency
                                  FROM BookingHeader b
                                  LEFT JOIN invoiceheader i ON b.BOKPRT=i.BLPRT
                                  LEFT JOIN Participants p ON i.debtor=p.ParticipantID AND p.Type='Customer'
                                  LEFT JOIN InvoiceCharges ic ON ic.BLPRT=i.BLPRT AND i.DEBTOR=ic.DEBTOR AND i.INVOICENO=ic.INVOICENO";

            FixWildcards(ref so);
            BuildCmdInnerJoin3(ref cmd, so);
            BuildCmdWhereCondition1(ref cmd, so);
            BuildCmdWhereCondition2(ref cmd, so);

            cmd += @"GROUP BY B.BOKPRT,i.DEBTOR,p.Name,B.MATRCLOCA,B.MATRDLOCA,ic.CURR) b";
            cmd += @"GROUP BY b.MATRCLOCA,b.MATRDLOCA,Currency";
            cmd += @"ORDER BY b.MATRCLOCA,b.MATRDLOCA";

            using (var connetion = Common.ClientDatabase)
            {
                var resultStatsByOrigin = connetion.Query<BookingHeaderImp>(cmd).ToList();
                return resultStatsByOrigin;
            }
        }

        public List<BookingHeaderImp> GetAuditResultStatsByPOL(SearchOptions so, string clientName)
        {
            string cmd = @"SELECT ISNULL(b.MATRCLOCA,'') AS MATRCLOCA,COUNT(*) AS ShipmentCount, MIN(shipmentcharges) AS MinCharges, MAX(shipmentcharges) AS MaxCharges,
                           (MAX(shipmentcharges) -  MIN(shipmentcharges)) AS Difference, CAST(ISNULL(STDEV(shipmentcharges),0) AS numeric(18,2)) AS StandardDeviation,Currency FROM
                           (select (isnull(LEFT(P.Name,10),' ') + ' (' + I.DEBTOR +  ')') AS Customer,b.MATRCLOCA AS MATRCLOCA,B.MATRDLOCA AS MATRDLOCA, 
                           B.BOKPRT, SUM(ISNULL(ic.INVCURAMT,0)) AS ShipmentCharges, ISNULL(ic.CURR,'') AS Currency
                           FROM BookingHeader b
                           LEFT JOIN invoiceheader i ON b.BOKPRT=i.BLPRT
                           LEFT JOIN Participants p ON i.debtor=p.ParticipantID AND p.Type='Customer'
                           LEFT JOIN InvoiceCharges ic ON ic.BLPRT=i.BLPRT AND i.DEBTOR=ic.DEBTOR AND i.INVOICENO=ic.INVOICENO";

            FixWildcards(ref so);
            BuildCmdWhereCondition1(ref cmd, so);
            BuildCmdWhereCondition2(ref cmd, so);

            cmd += @"GROUP BY B.BOKPRT,i.DEBTOR,p.Name,B.MATRCLOCA,B.MATRDLOCA,ic.CURR) b";
            cmd += @"GROUP BY b.MATRCLOCA,Currency";
            cmd += @"ORDER BY b.MATRCLOCA";

            using (var connetion = Common.ClientDatabase)
            {
                var resultStatsByPOL = connetion.Query<BookingHeaderImp>(cmd).ToList();
                return resultStatsByPOL;
            }
        }

        public void BuildCmdWhereCondition1(ref string cmd, SearchOptions so)
        {
            var estStartDateFormatted = so.EstimatedStartDate.ToDateTime();
            var estEndDateFormatted = so.EstimatedEndDate.ToDateTime();
            if (estStartDateFormatted != DateTime.MinValue && estEndDateFormatted != DateTime.MinValue)
            {
                var strEstStartDate = estStartDateFormatted.ToString("yyyy-MM-dd");
                var strEstEndDate = estEndDateFormatted.ToString("yyyy-MM-dd");
                AppendConditionOperator(ref cmd);
                cmd += $"b.MATRCDATE BETWEEN '{strEstStartDate}' AND '{strEstEndDate}'";
            }
            if (so.Mode != null)
            {
                AppendListCondition(ref cmd, "b.MODE", so.Mode);
            }
            if (so.TYPE != null)
            {
                AppendListCondition(ref cmd, "b.TYPE", so.TYPE);
            }
            if (so.Region != null)
            {
                AppendListCondition(ref cmd, "b.Region", so.Region);
            }
            if (so.CNTRYCODE != null)
            {
                AppendListCondition(ref cmd, "b.CNTRYCODE", so.CNTRYCODE);
            }
            if (so.CNTRYCOD01 != null)
            {
                AppendListCondition(ref cmd, "b.CNTRYCOD01", so.CNTRYCOD01);
            }
            if (so.POL != null)
            {
                AppendListCondition(ref cmd, "b.MATRCLOCA", so.POL);
            }
            if (so.POD != null)
            {
                AppendListCondition(ref cmd, "b.MATRDLOCA", so.POD);
            }
            if (so.ONCSERV != null)
            {
                AppendListCondition(ref cmd, "b.ONCSERV", so.ONCSERV);
            }
            if (so.CSEENO != null)
            {
                AppendListCondition(ref cmd, "b.CSEENO", so.CSEENO);
            }
            if (so.SHPRNO != null)
            {
                AppendListCondition(ref cmd, "b.SHPRNO", so.SHPRNO);
            }
            if (so.PCKGSCODE != null)
            {
                AppendListCondition(ref cmd, "b.PCKGSCODE", so.PCKGSCODE);
            }
            if (so.CARRCODE != null)
            {
                AppendListCondition(ref cmd, "b.CARRCODE", so.CARRCODE);
            }
            if (so.CARR != null)
            {
                AppendListCondition(ref cmd, "b.CARR", so.CARR);
            }
            if (so.SERVLEVEL != null)
            {
                AppendListCondition(ref cmd, "b.SERVLEVEL", so.SERVLEVEL);
            }
            if (so.SERVLVL != null)
            {
                AppendListCondition(ref cmd, "b.SERVLVL", so.SERVLVL);
            }
            if (so.SERVTYPE != null)
            {  
                AppendListCondition(ref cmd, "b.SERVTYPE", so.SERVTYPE);
            }
        }

        public void BuildCmdWhereCondition2(ref string cmd, SearchOptions so, bool Table = false)
        {
            if (so.BookingMinPieceCount > 0 || so.BookingMinPieceCount > 0)
            {
                AppendConditionOperator(ref cmd);
                cmd += $"b.PCKGS BETWEEN {so.BookingMinPieceCount} AND {so.BookingMaxPieceCount}";
            }

            if (so.BookingMinActualWeight > 0 || so.BookingMaxActualWeight > 0)
            {
                AppendConditionOperator(ref cmd);
                cmd += $"b.WGHT BETWEEN {so.BookingMinActualWeight} AND {so.BookingMaxActualWeight}";
            }

            if (so.BookingMinMeasure > 0 || so.BookingMaxMeasure > 0)
            {
                AppendConditionOperator(ref cmd);
                cmd += $"b.MEAS BETWEEN {so.BookingMinMeasure} AND {so.BookingMaxMeasure}";
            }

            if (so.BookingMinChargeableWeight > 0 || so.BookingMaxChargeableWeight > 0)
            {
                AppendConditionOperator(ref cmd);
                cmd += $"b.CHRGWGHT BETWEEN {so.BookingMinChargeableWeight} AND {so.BookingMaxChargeableWeight}";
            }

            if (!string.IsNullOrWhiteSpace(so.Bol))
            {
                AppendConditionOperator(ref cmd);
                cmd += $"(b.AWBNO='{so.Bol}' OR b.HOUSENO='{so.Bol}' OR b.HBLNO='{so.Bol}' OR b.HAWBNO='{so.Bol}' OR b.LBLNO='{so.Bol}')";
            }

            if (!string.IsNullOrWhiteSpace(so.GoodsDescription))
            {
                AppendConditionOperator(ref cmd);
                if (so.GoodsDescOperator != "=")
                {
                    cmd += $"(b.GOODSDSC NOT LIKE '%{so.GoodsDescription}%' OR b.GOODSDSC2 NOT LIKE '{so.GoodsDescription}' OR b.GOODSDSC3 NOT LIKE '{so.GoodsDescription}' OR " +
                        $"b.GOODSDSC4 NOT LIKE '{so.GoodsDescription}' OR b.GOODSDSC5 NOT LIKE '%{so.GoodsDescription}%' OR " +
                        $"b.GOODSDSC6 NOT LIKE '%{so.GoodsDescription}%' OR b.GOODSDSC7 NOT LIKE '%{so.GoodsDescription}%' OR b.GOODSDSC8 NOT LIKE '%{so.GoodsDescription}%' OR " +
                        $"b.GOODSDSC9 NOT LIKE '%{so.GoodsDescription}%' OR b.GOODSDSC10 NOT LIKE '%{so.GoodsDescription}%' OR " +
                        $"b.GOODSDSC11 NOT LIKE '%{so.GoodsDescription}%' OR b.GOODSDSC12 NOT LIKE '%{so.GoodsDescription}%')";
                }
                else
                {
                    cmd += $"(b.GOODSDSC LIKE '%{so.GoodsDescription}%' OR b.GOODSDSC2 LIKE '%{so.GoodsDescription}%' OR b.GOODSDSC3 LIKE '%{so.GoodsDescription}%' OR " +
                        $"b.GOODSDSC4 LIKE '%{so.GoodsDescription}%' OR b.GOODSDSC5 LIKE '%{so.GoodsDescription}%' OR b.GOODSDSC6 LIKE '%{so.GoodsDescription}%' OR " +
                        $"b.GOODSDSC7 LIKE '%{so.GoodsDescription}%' OR b.GOODSDSC8 LIKE '%{so.GoodsDescription}%' OR b.GOODSDSC9 LIKE '%{so.GoodsDescription}%' OR " +
                        $"b.GOODSDSC10 LIKE '%{so.GoodsDescription}%' OR b.GOODSDSC11 LIKE '%{so.GoodsDescription}%' OR b.GOODSDSC12 LIKE '%{so.GoodsDescription}%')";
                }
            }

            if (!string.IsNullOrWhiteSpace(so.BookingRemarks))
            {
                AppendConditionOperator(ref cmd);
                if (so.BookingRemarksOperator != "=")
                {
                    cmd += $"(b.MARKS NOT LIKE '%{so.BookingRemarks}%' OR b.REMARKS2 NOT LIKE '%{so.BookingRemarks}%' OR " +
                           $"b.REMARKS3 NOT LIKE '%{so.BookingRemarks}%' OR b.REMARKS4 NOT LIKE '%{so.BookingRemarks}%')";
                }
                else
                {
                    cmd += $"(b.MARKS LIKE '%{so.BookingRemarks}%' OR b.REMARKS2 LIKE '%{so.BookingRemarks}%' OR" +
                           $"b.REMARKS3 LIKE '%{so.BookingRemarks}%' OR b.REMARKS4 LIKE '%{so.BookingRemarks}%')";
                }
            }

            if (!string.IsNullOrWhiteSpace(so.HandlingRemarks))
            {
                AppendConditionOperator(ref cmd);
                if (so.HandlingRemarksOperator != "=")
                {
                    cmd += $"(b.HANDLNO NOT LIKE '%{so.HandlingRemarks}%' OR b.HANDLREM NOT LIKE '%{so.HandlingRemarks}%' OR " +
                        $"b.HANDLREM2 NOT LIKE '%{so.HandlingRemarks}%' OR b.HANDLREM3 NOT LIKE '%{so.HandlingRemarks}%' OR " +
                        $"b.CLAUSNO NOT LIKE '%{so.HandlingRemarks}%' OR b.CLAUSREM1 NOT LIKE '%{so.HandlingRemarks}%' OR b.CLAUSREM2 NOT LIKE '%{{0}}%' OR " +
                        $"b.CLAUSREM3 NOT LIKE '%{so.HandlingRemarks}%' OR b.CLAUSREM4 NOT LIKE '%{so.HandlingRemarks}%' OR " +
                        $"b.CLAUSREM5 NOT LIKE '%{so.HandlingRemarks}%' OR b.CLAUSREM6 NOT LIKE '%{so.HandlingRemarks}%' OR " +
                        $"b.CLAUSREM7 NOT LIKE '%{so.HandlingRemarks}%' OR b.CLAUSREM8 NOT LIKE '%{so.HandlingRemarks}%')";
                }
                else
                {
                    cmd += $"(b.HANDLNO LIKE '%{so.HandlingRemarks}%' OR b.HANDLREM LIKE '%{so.HandlingRemarks}%' OR b.HANDLREM2 LIKE '%{so.HandlingRemarks}%' OR " +
                           $"b.HANDLREM3 LIKE '%{so.HandlingRemarks}%' OR b.CLAUSNO LIKE '%{so.HandlingRemarks}%' OR b.CLAUSREM1 LIKE '%{so.HandlingRemarks}%' OR " +
                           $"b.CLAUSREM2 LIKE '%{so.HandlingRemarks}%' OR b.CLAUSREM3 LIKE '%{so.HandlingRemarks}%' OR b.CLAUSREM4 LIKE '%{so.HandlingRemarks}%' OR" +
                           $" b.CLAUSREM5 LIKE '%{so.HandlingRemarks}%'  OR b.CLAUSREM6 LIKE '%{so.HandlingRemarks}%' OR b.CLAUSREM7 LIKE '%{so.HandlingRemarks}%' OR " +
                           $"b.CLAUSREM8 LIKE '%{so.HandlingRemarks}%')";
                }
            }

            if (!string.IsNullOrWhiteSpace(so.BOLViews))
            {
                AppendConditionOperator(ref cmd, Table);
                cmd += $"(SELECT COUNT(*) AS ViewCount FROM {_dbName}.dbo.ActivityStats st WHERE";
                if (so.UserIds.Any() && so.UserIds[0] != "00000000-0000-0000-0000-000000000000")
                {
                    string list = Core.Utility.JoinStrings(so.UserIds.ToArray());
                    cmd += $" st.userId IN {list}";
                }
                cmd += $" AND ((b.HBLNo=st.BOLNo) OR (st.BOLNo=b.HAWBNo))) <= {so.BOLViews}";
            }

            if (so.ImageType != null)
            {
                var temp = "(";
                var strOR = " OR ";
                AppendConditionOperator(ref cmd, Table);
                cmd += "EXISTS(SELECT i.filename, i.ScanDate FROM DocumentImages i WHERE ((b.HBLNo=i.HAWBBLNO) OR (i.HAWBBLNO=b.HAWBNo)) AND ";

                foreach (var item in so.ImageType)
                {
                    temp += $"(i.Type LIKE' %{item}%')" + strOR;
                }
                temp = temp.Remove(temp.Length - strOR.Length, strOR.Length) + "))";
                cmd += temp;
            }

            if (!string.IsNullOrWhiteSpace(so.ChargeDesc))
            {
                AppendConditionOperator(ref cmd, Table);
                var temp = "EXISTS(SELECT TOP(1) ch.HAWBNO FROM InvoiceCharges ch WHERE ch.BLPRT =b.BOKPRT";
                var temp1 = temp;

                if (so.ChargeDescOperator != "=")
                {
                    temp1 = "NOT " + temp1;
                }
                cmd += temp1 + $" AND (INVTEXT1 LIKE '%{so.ChargeDesc}%' or INVTEXT2 LIKE '%{so.ChargeDesc}%') " + ")";
            }

            if (so.ChargeMinAmount != null && so.ChargeMaxAmount != null)
            {

                AppendConditionOperator(ref cmd, Table);
                if (so.ChargeMinAmount == "0" && so.ChargeMaxAmount == "0")
                {
                    cmd += "(SELECT INVCURAMT FROM InvoiceCharges WHERE INVCURAMT=0 OR INVCURAMT IS NULL)";
                }
                else
                {
                    cmd += "(SELECT SUM(INVCURAMT) FROM InvoiceCharges WHERE BLPRT=b.BOKPRT";

                    if (!string.IsNullOrWhiteSpace(so.ChargeDesc))
                    {
                        if (so.ChargeDescOperator != "=")
                        {
                            cmd += $"AND ((INVTEXT1 NOT LIKE '%{so.ChargeDesc}%') OR (INVTEXT2 NOT LIKE '%{so.ChargeDesc}%'))";
                        }
                        else
                        {
                            cmd += $"AND ((INVTEXT1 LIKE '%{so.ChargeDesc}%') OR (INVTEXT2 LIKE '%{so.ChargeDesc}%'))";
                        }
                    }
                    cmd += $") BETWEEN {Convert.ToDecimal(so.ChargeMinAmount)} AND {Convert.ToDecimal(so.ChargeMaxAmount)}";
                }
            }
        }

        public void BuildCmdInnerJoin2(ref string cmd, SearchOptions so, bool IsTable = false)
        {
            if ((!string.IsNullOrWhiteSpace(so.ContainerId)) || (so.CNTRTYPE != null))
            {
                cmd += " LEFT JOIN [BookingHeader.ContainerDetail] h ON h.JOBREF = b.BOKPRT AND h.Region = b.Region";
            }

            if (so.CSORNO != null)
            {
                if (!IsTable)
                {
                    if (!so.CSORNO.Contains("null") && !so.CSORNO.Contains(""))
                    {
                        string temp = "EXISTS(SELECT TOP 1 ih.DEBTOR FROM InvoiceHeader ih WHERE b.BOKPRT = ih.BLPRT AND (";
                        string strOR = " OR ";

                        foreach (var item in so.CSORNO)
                        {
                            if (item != "null" && item != "")
                            {
                                temp += $"ih.DEBTOR = '{item}'" + strOR;
                            }
                            else
                            {
                                temp += "ih.DEBTOR IS NULL" + strOR;
                            }
                        }
                        temp = temp.Remove(temp.Length - strOR.Length) + "))";
                        AppendConditionOperator(ref cmd);
                        cmd += temp;
                    }
                    else
                    {
                        cmd += " LEFT JOIN invoicecharges ba ON b.BOKPRT = ba.BLPRT";
                        var temp = "(";
                        var strOR = " OR ";

                        foreach (var item in so.CSORNO)
                        {
                            if (item != "null" && item != "")
                            {
                                temp += $"ba.DEBTOR = '{item}'" + strOR;
                            }
                            else
                            {
                                temp += "ba.DEBTOR IS NULL" + strOR;
                            }
                        }
                    }
                }
                else
                {
                    var temp = "(";
                    var strOR = " OR ";

                    foreach (var item in so.CSORNO)
                    {
                        if (item != "null" && item != "")
                        {
                            temp += $"ba.DEBTOR = '{item}'" + strOR;
                        }
                        else
                        {
                            temp += "ba.DEBTOR Is Null" + strOR;
                        }
                    }
                    temp = temp.Remove(temp.Length - strOR.Length, strOR.Length) + ")";
                    AppendConditionOperator(ref cmd);
                    cmd += temp ;
                }
            }

            if ((!string.IsNullOrWhiteSpace(so.ContainerId)) || (so.CNTRTYPE != null))
            {
                if (!string.IsNullOrWhiteSpace(so.ContainerId))
                {
                    if (so.ContainerIdOperator != "=")
                    {
                        cmd += $" AND h.CNTRNO <> '{so.ContainerId}'"; 
                    }

                    cmd += $" AND h.CNTRNO LIKE '%{so.ContainerId}%'"; 
                }
                if (so.CNTRTYPE != null)
                {
                    var temp = "";
                    var strOR = " OR ";

                    foreach (var item in so.CNTRTYPE)
                    {
                        temp += $"h.CNTRTYPE = '{item}'" + strOR;
                    }
                    temp = temp.Remove(temp.Length - strOR.Length, strOR.Length);
                    AppendConditionOperator(ref cmd);
                    cmd += $"({temp})";
                }
            }

            if (so.NetRateMinAmount > 0 || so.NetRateMaxAmount > 0)
            {
                cmd += $" AND ((SELECT SUM(i.INVCURAMT) FROM InvoiceCharges i WHERE i.BLPRT = b.BOKPRT))/ b.CHRGWGHT BETWEEN {so.NetRateMinAmount} AND {so.NetRateMaxAmount} )";
            }
        }

        public void BuildCmdInnerJoin3(ref string cmd, SearchOptions so, bool Table = false)
        {
            if (so.CSORNO != null)
            {
                string temp = "(";
                string strOR = " OR ";

                foreach (var item in so.CSORNO)
                {
                    if (item != "null" && item != "")
                    {
                        temp += $"i.DEBTOR = '{item}'" + strOR;
                    }
                    else
                    {
                        temp += "i.DEBTOR Is Null" + strOR;
                    }
                }
                temp = temp.Remove(temp.Length - strOR.Length, strOR.Length) + ")";
                cmd += temp;
            }

            if ((!string.IsNullOrWhiteSpace(so.ContainerId)) || (so.CNTRTYPE != null))
            {
                cmd += " LEFT JOIN [BookingHeader.ContainerDetail] h ON h.JOBREF = b.BOKPRT AND h.Region = b.Region";

                if (!string.IsNullOrWhiteSpace(so.ContainerId))
                {
                    if (so.ContainerIdOperator != "=")
                    {
                        cmd += $" AND h.CNTRNO <> '{so.ContainerId}'";
                    }

                    cmd += $" AND h.CNTRNO LIKE '%{so.ContainerId}%'";
                }

                if (so.CNTRTYPE != null)
                {
                    string temp = $" AND h.CNTRTYPE IN ({string.Join(",", so.CNTRTYPE.Select(ct => $"'{ct}'"))})";
                }
            }

            if (so.NetRateMinAmount > 0 || so.NetRateMaxAmount > 0)
            {
                cmd += $" AND ((SELECT SUM(i.INVCURAMT) FROM InvoiceCharges i WHERE i.BLPRT = b.BOKPRT))/ b.CHRGWGHT BETWEEN {so.NetRateMinAmount} AND {so.NetRateMaxAmount} )";
            }
        }

        public void AppendConditionOperator(ref string cmd, bool Table = false)
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

        private void AppendListCondition(ref string cmd, string columnName, List<string> values)
        {
            var temp = "";
            var strOR = " OR ";

            foreach (var item in values)
            {
                temp += $"{columnName} = '{item}'" + strOR;
            }

            temp = temp.Remove(temp.Length - strOR.Length, strOR.Length);
            AppendConditionOperator(ref cmd);
            cmd += temp;

            //var temp = "";
            //var strOR = " OR ";
            //foreach (var item in so.Mode)
            //{
            //    temp += $"b.MODE = '{item}'" + strOR;
            //}
            //temp = temp.Remove(temp.Length - strOR.Length, strOR.Length);
            //AppendConditionOperator(ref cmd);
            //cmd += temp;
        }
    }
}
