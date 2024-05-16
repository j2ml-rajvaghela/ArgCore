using Dapper;

namespace Arg.DataAccess
{
    public class Utilities
    {
        public static object ExecuteCmd(string cmd)
        {
            using var connection = Common.ClientDatabase;
            var result = connection.Execute(cmd);
            System.Diagnostics.Trace.TraceInformation(cmd);
            return result;
        }
    }
}
