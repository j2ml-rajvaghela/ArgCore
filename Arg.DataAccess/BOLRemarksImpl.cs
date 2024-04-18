using Arg.DataModels;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arg.DataAccess
{
    public  class BOLRemarksImpl
    {
        public List<BOLRemarks> GetBOLRemarks(string bolNo)
        {
            const string query = @"SELECT BOL#,Remarks,Sequence FROM BOLRemarks
                                   WHERE BOL#=@BolNo AND Remarks <> ''
                                   ORDER BY Sequence;";

            using (var connection = Common.ClientDatabase)
            {
                var bOLRemarks = connection.Query<BOLRemarks>(query, new { BolNo = bolNo }).ToList();
                return bOLRemarks;
            }
        }
    }
}
