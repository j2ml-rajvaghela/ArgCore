using Arg.DataModels;
using Dapper;
using System.Data;

namespace Arg.DataAccess
{
    public class BOLChargesImpl
    {

        public List<BOLChargesModel> GetBOLCharges(string bolNo)
        {
            const string query = @"SELECT CONCAT(c.ChargeCode,' ',c.ChargeDescription) AS Charge,c.ChargeDescription,c.USAmount, CONCAT(c.PayorID,' ',p.ParticipantName) AS Payor
                                   FROM BOLCharges c
                                   INNER JOIN Participants p ON p.ParticipantID=c.PayorID
                                   WHERE c.BOL#=@BolNo
                                   ORDER BY c.USAmount DESC;";

            using (var connection = Common.ClientDatabase)
            {
                var bOLCharges = connection.Query<BOLChargesModel>(query, new { BolNo = bolNo }).ToList();
                return bOLCharges;
            }
        }

        public List<BOLChargesModel> GetBOLOceanCharges(string bolNo, bool oceanCharges = false)
        {
            const string query = @"SELECT * FROM BOLCharges
                                   WHERE  (@OceanCharges = 1 AND ChargeDescription LIKE 'Ocean%') OR 
                                   (@OceanCharges = 0 AND ChargeDescription NOT LIKE 'Ocean%')
                                   AND BOL# = @BOLNo;";

            using (var connection = Common.ClientDatabase)
            {
                var bOLOceanCharges = connection.Query<BOLChargesModel>(query, new { OceanCharges = oceanCharges, BOLNo = bolNo}).ToList();
                return bOLOceanCharges;
            }
        }

        public List<BOLChargesModel> GetDistinctCurrency()
        {
            const string query = @"SELECT DISTINCT Currency FROM BOLCharges 
                                   WHERE Currency <> '';";

            using (var connection = Common.ClientDatabase)
            {
                var distinctCurrency = connection.Query<BOLChargesModel>(query, commandType: CommandType.Text).ToList();
                return distinctCurrency;
            }
        }

        public BOLChargesModel GetBOLCharge(string bolNo)
        {
            const string query = @"SELECT* FROM BOLCharges 
                                   WHERE (BOL#=@BolNo OR @BolNo = 0);";

            using (var connection = Common.ClientDatabase)
            {
                var bOLCharges = connection.QueryFirstOrDefault<BOLChargesModel>(query, new { @BolNo = bolNo });
                return bOLCharges;
            }
        }

        public decimal GetPashaAmountDue(string bolNo)
        {
            const string query = @"SELECT c.USAmount FROM BOLCharges c 
                                   WHERE c.Bol#=@BolNo AND (chargeCode='OF' OR chargeDescription LIKE '%Ocean Freight%');";

            using (var connection = Common.ClientDatabase)
            {
                var pashaAmountDue = connection.QueryFirstOrDefault<decimal>(query, new { BolNo = bolNo });
                return pashaAmountDue;
            }
        }

        public List<BOLChargesModel> GetPashaBalanceDuesOtherChargesWithDesc(string bolNo)
        {
            const string query = @"SELECT ROW_NUMBER() OVER(PARTITION BY o.Bol# order by o.Bol#) AS ItemId, o.USAmount, o.Bol#,o.ChargeDescription, o.ChargeCode AS ChargeCode From [BOLCharges] o
                                   WHERE o.Bol#=@BolNo 
                                   ORDER BY ItemId;";

            using (var connection = Common.ClientDatabase)
            {
                var PashaBDOtherCharges = connection.Query<BOLChargesModel>(query, new { BolNo = bolNo }).ToList();
                return PashaBDOtherCharges;
            }
        }
    }
}
