using Arg.DataModels;
using Dapper;

namespace Arg.DataAccess
{
    public class InvoiceSummaryImpl
    {
        public List<InvoiceSummary> GetInvoiceSummary(string bolNo)
        {
            const string query = @"SELECT CONCAT(a.PayorID,' ',a.ParticipantName) AS Payor, ISNULL(a.TotalCharges,0) AS TotalCharges,ISNULL(b.TotalPayment,0) AS TotalPayment
                                   FROM (SELECT PayorID,p.ParticipantName, SUM(usamount) AS TotalCharges FROM BOLCharges c
                                   JOIN Participants p on p.ParticipantID=c.PayorID 
                                   WHERE BOL#=@BOL
                                   GROUP BY PayorID,p.ParticipantName) a
                                   LEFT JOIN (
                                                SELECT PayorID AS PayorID2, SUM(AmountPaid) AS TotalPayment FROM ARCash c
                                                JOIN Participants p ON p.ParticipantID=c.PayorID
                                                WHERE BOL#=@BOL
                                                GROUP BY PayorID
                                              ) b
                                   ON a.PayorID=b.PayorID2;";

            using var connection = Common.ClientDatabase;
            var invoiceSummary = connection.Query<InvoiceSummary>(query, new { bolNo }).ToList();
            return invoiceSummary;
        }

        public InvoiceSummary GetInvoiceNo(string bolNo)
        {
            const string query = @"SELECT * FROM InvoiceSummary 
                                   WHERE BOL#=@BOL;";

            using var connection = Common.ClientDatabase;
            var invoiceNo = connection.QueryFirstOrDefault<InvoiceSummary>(query, new { bolNo });
            return invoiceNo;
        }
    }
}
