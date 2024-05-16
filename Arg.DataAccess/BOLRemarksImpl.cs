using Arg.DataModels;
using Dapper;

namespace Arg.DataAccess
{
    public  class BOLRemarksImpl
    {
        public List<BOLRemarks> GetBOLRemarks(string bolNo)
        {
            const string query = @"SELECT BOL#,Remarks,Sequence FROM BOLRemarks
                                   WHERE BOL#=@BolNo AND Remarks <> ''
                                   ORDER BY Sequence;";

            using var connection = Common.ClientDatabase;
            var bOLRemarks = connection.Query<BOLRemarks>(query, new { bolNo }).ToList();
            return bOLRemarks;
        }
    }
}
