using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;

namespace Arg.DAL
{
    public static class data_logging
    {
        private static string thisconnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public static void AddAppLogEntry(string source, string message, int linenumber = 0, int severity = 0, string description = "")
        {
            using SqlConnection sqlConnection = new SqlConnection(thisconnectionString);
            sqlConnection.Open();
            using SqlCommand sqlCommand = new SqlCommand("dbo.ins_errorapplogentry", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.Add(new SqlParameter("@errorsource", source));
            sqlCommand.Parameters.Add(new SqlParameter("@severity", severity));
            sqlCommand.Parameters.Add(new SqlParameter("@linenumber", linenumber));
            sqlCommand.Parameters.Add(new SqlParameter("@errormessage", message));
            sqlCommand.Parameters.Add(new SqlParameter("@errordescription", description));
            try
            {
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("dal error " + ex.ToString());
            }
        }

        public static void AddAppLogEntry(Exception x)
        {
            StackFrame stackFrame = new StackFrame(1, needFileInfo: true);
            StackTrace stackTrace = new StackTrace(stackFrame);
            string name = stackFrame.GetMethod().Name;
            string text = x.StackTrace.Substring(x.StackTrace.Length - 5);
            int i = 0;
            int num = 0;
            int result = 0;
            StringBuilder stringBuilder = new StringBuilder();
            for (; i < text.Length; i++)
            {
                if (int.TryParse(text[i].ToString(), out var _))
                {
                    stringBuilder.Append(text[i]);
                    num++;
                }
            }

            if (stringBuilder.ToString().Length > 0)
            {
                int.TryParse(stringBuilder.ToString(), out result);
            }

            using SqlConnection sqlConnection = new SqlConnection(thisconnectionString);
            sqlConnection.Open();
            using SqlCommand sqlCommand = new SqlCommand("dbo.ins_errorapplogentry", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.Add(new SqlParameter("@errorsource", name));
            sqlCommand.Parameters.Add(new SqlParameter("@errormessage", x.Message));
            sqlCommand.Parameters.Add(new SqlParameter("@errordescription", stackTrace.ToString()));
            sqlCommand.Parameters.Add(new SqlParameter("@linenumber", result));
            try
            {
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("dal error " + ex.ToString());
            }
        }
    }
}
