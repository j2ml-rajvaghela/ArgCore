using Arg.DataAccess;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;
using System.Data;
namespace Arg.Ceva.DataAccess
{
    public class InvoiceHeaders
    {
        private readonly SqlConnection _connection;

        public InvoiceHeaders()
        {
            _connection = Common.ClientDatabase;
        }

        [Table("InvoiceHeader")]
        public class InvoiceHeader
        {
            public string Region { get; set; }
            public string INVOICENO { get; set; }
            public DateTime DATEINV { get; set; }
            public string REGNO { get; set; }
            public string NOTETYPE { get; set; }
            public string NOTECLASS { get; set; }
            public string DEBTOR { get; set; }
            public string NAMEADR1 { get; set; }
            public string BLPRT { get; set; }
            public string POL { get; set; }
            public string POD { get; set; }
            public string CNTRYCODE { get; set; }
            public string CTRY { get; set; }
            public string AWBNO { get; set; }
            public string HAWBNO { get; set; }
            public string CLAUSNO { get; set; }
            public string PRODUCT2 { get; set; }
            public DateTime? PCCDATE { get; set; }
            public DateTime? MATRCDATE { get; set; }
            public DateTime? MATRDDATE { get; set; }
            public DateTime? ONCDDATE { get; set; }
            public string CARR { get; set; }
            public string MAINFLGH { get; set; }
            public string SADCREF { get; set; }
            public int? PCKGS { get; set; }
            public string PCKGSCODE { get; set; }
            public decimal? WGHT { get; set; }
            public decimal? CHRGWGHT { get; set; }
            public decimal? MEAS { get; set; }
            public string MAINTRPPCC { get; set; }
            public string TODCODE { get; set; }
            public string GOODSDSC02 { get; set; }
            public string GOODSDSC03 { get; set; }
            public string GOODSDSC04 { get; set; }
            public string GOODSDSC05 { get; set; }
            public string GOODSDSC06 { get; set; }
            public string GOODSDSC07 { get; set; }
            public string GOODSDSC08 { get; set; }
            public string GOODSDSC09 { get; set; }
            public string GOODSDSC10 { get; set; }
            public string GOODSDSC11 { get; set; }
            public string GOODSDSC12 { get; set; }
            public string REMARKS { get; set; }
            public string REMARKS2 { get; set; }
            public string REMARKS3 { get; set; }
            public string REMARKS4 { get; set; }
            public string DOSSIER { get; set; }
            public string SHPRREF { get; set; }
            public string LBLNO { get; set; }
            public string HBLNO { get; set; }
            public string GOODSTYPE { get; set; }
            public string CARRCODE { get; set; }
            public string VESSCODE { get; set; }
            public string VOYNO { get; set; }
            public string CUSTNO { get; set; }
            public string HOUSENO { get; set; }
            public string INVREM1 { get; set; }
            public string INVREM2 { get; set; }
            public string INVREM3 { get; set; }
            public string INVREM4 { get; set; }
            public DateTime? AddedOn { get; set; }

            [Computed]
            public decimal INVCURAMT { get; set; }

            [Computed]
            public decimal CURR { get; set; }

            [Computed]
            public string INVTEXT1 { get; set; }

            [Computed]
            public string INVTEXT2 { get; set; }

            [Computed]
            public string INVREMDescription { get; set; }

            [Computed]
            public string INVTEXTDescription { get; set; }

            [Computed]
            public decimal ChargeAmount { get; set; }
        }

        public List<InvoiceHeader> GetInvoicedChargesDetail(string BOKPRT)
        {
            const string query = @"SELECT CONCAT(ic.INVTEXT1,' ',ic.INVTEXT2) AS INVTEXTDescription,ic.INVTEXT2,rate, ic.INVCURAMT,ic.CURR 
                                   FROM InvoiceCharges ic
                                   INNER JOIN BookingHeader bh ON bh.Region=ic.Region AND bh.BOKPRT=ic.BLPRT
                                   WHERE bh.BOKPRT=@BOKPRT
                                   ORDER BY ic.DEBTOR,ic.INVCURAMT DESC;";

            return _connection.Query<InvoiceHeader>(query, new { BOKPRT = BOKPRT }).ToList();
        }
    }
}
