using Arg.DataAccess;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;
namespace Arg.Ceva.DataAccess
{
    public class InvoiceCharges
    {
        [Table("InvoiceCharges")]
        public class InvoiceCharge
        {
            public string Region { get; set; }
            public string INVOICENO { get; set; }
            public DateTime? DATEINV { get; set; }
            public int? SEQNO { get; set; }
            public string DEBTOR { get; set; }
            public string BLPRT { get; set; }
            public string CNTRYCODE { get; set; }
            public string HAWBNO { get; set; }
            public string LBLNO { get; set; }
            public string HBLNO { get; set; }
            public string HOUSENO { get; set; }
            public decimal? RATE { get; set; }
            public string CURR { get; set; }
            public string INVTEXT1 { get; set; }
            public string INVTEXT2 { get; set; }
            public decimal? INVCURAMT { get; set; }
            public decimal? AMNTBAS { get; set; }
            public string VATCLASS { get; set; }
            public string VATREASON { get; set; }
            public decimal? VATAMT { get; set; }
            public decimal? VATAMTBAS { get; set; }
            public DateTime? AddedOn { get; set; }

            [Computed]
            public decimal GrossRate { get; set; }

            [Computed]
            public decimal NetRate { get; set; }

            [Computed]
            public string INVREMDescription { get; set; }

            [Computed]
            public string INVTEXTDescription { get; set; }

            [Computed]
            public decimal ChargeAmount { get; set; }

            [Computed]
            public string ParticipantName { get; set; }
        }

        public InvoiceCharge GetGrossRate(string HBLNO, string HAWBNO)
        {
            var parameters = new DynamicParameters();

            if ((!string.IsNullOrWhiteSpace(HBLNO) && HBLNO != "0") || (!string.IsNullOrWhiteSpace(HAWBNO) && HAWBNO != "0"))
            {
                if (!string.IsNullOrWhiteSpace(HBLNO) && HAWBNO != "0")
                {
                    parameters.Add("@HBLNO", HBLNO, DbType.String);
                }
                if (!string.IsNullOrWhiteSpace(HAWBNO) && HAWBNO != "0")
                {
                    parameters.Add("@HAWBNO", HAWBNO, DbType.String);
                }
                const string query = @"SELECT SUM(INVCURAMT) AS GrossRate FROM InvoiceCharges
                                       WHERE INVTEXT1 LIKE '%OCEAN%' AND HBLNO = @HBLNO
                                       AND INVTEXT1 LIKE '%AIR KILO%' AND [HAWBNO] = @HAWBNO;";

                using (var connection = Common.ClientDatabase)
                {
                    var grossRate = connection.QueryFirstOrDefault<InvoiceCharge>(query, parameters);
                     return grossRate;
                }
            }
            return new InvoiceCharge();
        }

        public InvoiceCharge GetNetRate(string HBLNO, string HAWBNO)
        {
            var parameters = new DynamicParameters();

            if ((!string.IsNullOrWhiteSpace(HBLNO) && HBLNO != "0") || (!string.IsNullOrWhiteSpace(HAWBNO) && HAWBNO != "0"))
            {
                if (!string.IsNullOrWhiteSpace(HBLNO) && HAWBNO != "0")
                {
                    parameters.Add("@HBLNO", HBLNO, DbType.String);
                }
                if (!string.IsNullOrWhiteSpace(HAWBNO) && HAWBNO != "0")
                {
                    parameters.Add("@HAWBNO", HAWBNO, DbType.String);
                }
                const string query = @"SELECT SUM(INVCURAMT) AS NetRate FROM InvoiceCharges
                                       WHERE HBLNO = @HBLNO AND [HAWBNO] = @HAWBNO;";

                using (var connection = Common.ClientDatabase)
                {
                    var netRate = connection.QueryFirstOrDefault<InvoiceCharge>(query, parameters);
                    return netRate;
                }
            }
            return new InvoiceCharge();
        }

        public List<InvoiceCharge> GetDistinctCurrency()
        {
            const string query = @"SELECT DISTINCT CURR FROM InvoiceCharges 
                                   WHERE CURR <> '';";

            using (var connection = Common.ClientDatabase)
            {
                var distinctCurrency = connection.Query<InvoiceCharge>(query, commandType: CommandType.Text).ToList();
                return distinctCurrency;
            }
        }

        public InvoiceCharge GetBOLCharge(string BOLNo)
        {
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(BOLNo))
            {
                parameters.Add("@HBLNO", BOLNo, DbType.String);
                parameters.Add("@HAWBNO", BOLNo, DbType.String);
                parameters.Add("@HOUSENO", BOLNo, DbType.String);
            }
            const string query = @"SELECT * FROM InvoiceCharges
                                   WHERE HBLNO=@HBLNO OR HAWBNO=@HAWBNO OR HOUSENO=@HOUSENO;";

            using (var connection = Common.ClientDatabase)
            {
                var bolCharge = connection.QueryFirstOrDefault<InvoiceCharge>(query,parameters);
                return bolCharge;
            }
        }

        public List<InvoiceCharge> GetInvoicedChargesDetail(string code)
        {
            const string query = @"SELECT b.*,Concat(b.INVTEXT1,' ',b.INVTEXT2) AS INVTEXTDescription,
                                   (SELECT TOP 1 CONCAT(b.DEBTOR,' ',left(p.Name,10)) FROM Participants p WHERE b.DEBTOR=p.ParticipantID) AS ParticipantName
                                   FROM BookingHeader a
                                   WHERE a.BOKPRT=@BOKPRT
                                   ORDER BY b.DEBTOR,b.INVOICENO, INVCURAMT DESC;";

            using (var connection = Common.ClientDatabase)
            {
                var invoicedChargesDetail = connection.Query<InvoiceCharge>(query, new { BOKPRT = code}).ToList();
                return invoicedChargesDetail;
            }
        }

        public List<DataModels.BalanceDues_OtherCharges> GetBalanceDuesInvoicedChargesDetail(string bolNo)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@HBLNo", bolNo, DbType.String);
            parameters.Add("@HAWBNo", bolNo, DbType.String);
            const string query = @"SELECT b.INVTEXT1 AS ChargeCode,b.INVTEXT2 AS Description,b.INVCURAMT AS AmountDue,b.CURR AS Currency FROM InvoiceCharges b
                                   INNER JOIN BookingHeader a ON a.Region=b.Region and  (a.HBLNo=b.HBLNo or a.HAWBNo = b.HAWBNo)
                                   WHERE INVTEXT1 NOT LIKE '%Ocean%' AND INVTEXT1 NOT LIKE '%Air Cargo%'
                                   AND a.HBLNo=@HBLNo OR a.HAWBNo=@HAWBNo
                                   ORDER BY b.DEBTOR,b.INVOICENO, INVCURAMT DESC;";

            using (var connection = Common.ClientDatabase)
            {
                var balanceDuesInvoicedChargesDetail = connection.Query<DataModels.BalanceDues_OtherCharges>(query, parameters).ToList();
                return balanceDuesInvoicedChargesDetail;
            }
        }
    }
}
