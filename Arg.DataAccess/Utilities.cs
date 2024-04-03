using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arg.DataAccess
{
    public class Utilities
    {
        public static object ExecuteCmd(string cmd)
        {
            using (var connection = Common.ClientDatabase)
            {
                var result = connection.Execute(cmd);
                System.Diagnostics.Trace.TraceInformation(cmd);
                return result;
            }
        }
    }
}
