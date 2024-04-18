using Arg.DataModels;
using Dapper;
using System.ComponentModel.Design;
using System.Data;

namespace Arg.DataAccess
{
    public class BalanceDues_DescriptionsImpl
    {
        public List<BalanceDues_Descriptions> GetBalanceDuesDesc(int companyId, string invoiceType = "")
        {

            const string query = @"SELECT CONCAT(ErrorType, ' ', LEFT(Description, 100)) AS BDDescription, Description
                                   FROM [BalanceDues.Descriptions]
                                   WHERE (CompanyID = @CompanyId OR @CompanyId = 0)
                                   AND ((@InvoiceType = '')
                                   OR (@InvoiceType = 'overcharge' AND ErrorType LIKE 'OC%')
                                   OR (@InvoiceType = 'under' AND ErrorType LIKE 'UB%'))
                                   ORDER BY ErrorType, Description;";

            using (var connection = Common.Database)
            {
                var balanceDuesDesc = connection.Query<BalanceDues_Descriptions>(query, new { CompanyId = companyId, InvoiceType = invoiceType }).ToList();
                return balanceDuesDesc;
            }
        }
    }
}
