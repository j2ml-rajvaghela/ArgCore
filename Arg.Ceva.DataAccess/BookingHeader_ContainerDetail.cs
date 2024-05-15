using Arg.DataAccess;
using Dapper;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Arg.Ceva.DataAccess
{
    public class BookingHeader_ContainerDetail
    {
        private readonly SqlConnection _connection;

        public BookingHeader_ContainerDetail()
        {
            _connection = Common.ClientDatabase;
        }

        [Dapper.Contrib.Extensions.Table("BookingHeader.ContainerDetail")]
        public class ContainerDetail
        {
            public string Region { get; set; }

            [Column("COUNTRY#")]
            public string COUNTRY { get; set; }

            [Column("COMPANYN#")]
            public string COMPANYN { get; set; }

            [Column("DATALIB#")]
            public string DATALIB { get; set; }

            [Column("EXTRACTD#")]
            public string EXTRACTD { get; set; }

            public string IMPEXP { get; set; }
            public string SERVICE { get; set; }
            public string BRANCH { get; set; }
            public int? ITEMCOUNT { get; set; }
            public string JOBREF { get; set; }
            public DateTime? CREATED { get; set; }
            public string HBLNO { get; set; }
            public string MBLNO { get; set; }
            public string CONSOLNO { get; set; }
            public string FCLLCL { get; set; }
            public string CNTRTYPE { get; set; }
            public string CNTRDESC { get; set; }
            public decimal? TEUS { get; set; }
            public string CNTRNO { get; set; }
            public string CNTRPOD { get; set; }
            public DateTime? EDS { get; set; }
            public string EDSYEAR { get; set; }
            public string EDSMONTH { get; set; }
            public string EDSDAY { get; set; }
            public string PICKUP { get; set; }
            public string POL { get; set; }
            public string SRVTYPE { get; set; }
            public string POLCNTRY { get; set; }
            public string POLREGION { get; set; }
            public DateTime? EDA { get; set; }
            public string EDAYEAR { get; set; }
            public string EDAMONTH { get; set; }
            public string EDADAY { get; set; }
            public string POD { get; set; }
            public string PODCNTRY { get; set; }
            public string ONCARR { get; set; }
            public string PODREGION { get; set; }
            public string CARRCODE { get; set; }
            public string SCACCODE { get; set; }
            public string CARRNAME { get; set; }
            public string PREFCARR { get; set; }
            public string SHPRNO { get; set; }
            public string SHPRNAME { get; set; }
            public string CSEENO { get; set; }
            public string CSEENAME { get; set; }
            public string CUSTNO { get; set; }
            public string CUSTNAME { get; set; }
            public decimal? GROSSKGS { get; set; }
            public decimal? CUBE { get; set; }
            public decimal? CHRGKGS { get; set; }
            public string TERMSDEL { get; set; }
            public decimal? GROSSREV { get; set; }
            public decimal? NETREV { get; set; }
            public decimal? FRGTREV { get; set; }
            public decimal? FRGTCST { get; set; }
            public decimal? OTHERREV { get; set; }
            public decimal? TOTALCST { get; set; }
            public string LASTUSER { get; set; }
            public string CREATEUSER { get; set; }
            public decimal? OCSPAMT { get; set; }
            public string VESSCODE { get; set; }
            public string VESSNAME { get; set; }
            public string VOYNO { get; set; }
            public string LSONO { get; set; }
        }

        public List<ContainerDetail> GetContainerDetails()
        {
            const string query = @"SELECT DISTINCT CNTRTYPE 
                                   FROM [BookingHeader.ContainerDetail]
                                   WHERE CNTRTYPE IS NOT NULL AND CNTRTYPE <> ''
                                   ORDER BY CNTRTYPE;";

            return _connection.Query<ContainerDetail>(query, commandType: CommandType.Text).ToList();
        }

        public List<ContainerDetail> GetContainerDetail(string HBLNO)
        {
            const string query = @"SELECT * 
                                   FROM [BookingHeader.ContainerDetail]
                                   WHERE HBLNO=@HBLNO;";

            return _connection.Query<ContainerDetail>(query, new { @HBLNO = HBLNO }).ToList();
        }

        public List<ContainerDetail> GetDistinctSize()
        {
            const string query = @"SELECT DISTINCT CNTRTYPE 
                                   FROM [BookingHeader.ContainerDetail] 
                                   WHERE CNTRTYPE <> '' 
                                   ORDER BY CNTRTYPE;";

            return _connection.Query<ContainerDetail>(query, commandType: CommandType.Text).ToList();
        }

        public List<DataModels.BalanceDues_Item> GetBalanceDuesContainerDetail(string bolNo)
        {
            const string query = @"SELECT bh.PCKGSCODE AS Type,
                                   bh.CHRGWGHT AS ChargeableWeight,
                                   bc.CNTRNO AS Container,
                                   bc.CNTRTYPE AS ContainerSize,
                                   bh.GOODSDSC AS CommodityDesc,
                                   bh.PCKGS AS Quantity,
                                   CASE WHEN bh.HBLNO <> '' AND bh.HBLNo <> '0' AND bh.HBLNo IS NOT NULL THEN(SELECT SUM(INVCURAMT)
                                   FROM invoicecharges WHERE INVTEXT1 LIKE '%OCEAN%'  AND HBLNO = bh.HBLNO)
                                   WHEN bh.HAWBNO <> '' AND bh.HAWBNO <> '0' AND bh.HAWBNO IS NOT NULL THEN(SELECT SUM(INVCURAMT) 
                                   FROM invoicecharges 
                                   WHERE INVTEXT1 LIKE '%AIR KILO%'  AND HAWBNO = bh.HAWBNO) Else 0 END AS AmountDue
                                   FROM [BookingHeader.ContainerDetail] bc
                                   LEFT JOIN BookingHeader bh ON bc.HBLNo=bh.HBLNo
                                   WHERE bc.HBLNO=@HBLNO;";

            return _connection.Query<DataModels.BalanceDues_Item>(query, new { HBLNO = bolNo }).ToList();
        }
    }
}
