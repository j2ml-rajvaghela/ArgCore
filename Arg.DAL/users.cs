using Arg.DataModels;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Arg.DataAccess;

namespace Arg.DAL
{
    public class users : _basedbclass
    {
        public void insert_loginattempt(LogInAttempt ATT)
        {
            try
            {
                using (var conn = new SqlConnection(getConnectionString()))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("dbo.ins_loginattempt", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@username", ATT.UserName));
                        cmd.Parameters.Add(new SqlParameter("@ipaddress", ATT.IPAddress));
                        cmd.Parameters.Add(new SqlParameter("@authenticated", ATT.Autheticated));
                        cmd.Parameters.Add(new SqlParameter("@errormessage", ATT.ErrorMessage));
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("conn error:" + e.ToString());
                //logdata.AddAppLogEntry("listprojects", "", 0, 0, e.ToString());
            }
        }

        public int insertuser(AppUser USR)
        {
            int result = 0;
            try
            {
                using SqlConnection sqlConnection = new SqlConnection(getConnectionString());
                sqlConnection.Open();
                using SqlCommand sqlCommand = new SqlCommand("dbo.ins_appuser", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@aspnetuserid", USR.AspnetUserId));
                sqlCommand.Parameters.Add(new SqlParameter("@firstname", USR.FirstName));
                sqlCommand.Parameters.Add(new SqlParameter("@lastname", USR.LastName));
                sqlCommand.Parameters.Add(new SqlParameter("@title", USR.Title));
                sqlCommand.Parameters.Add(new SqlParameter("@clientcode", USR.ClientCode));
                result = (int)sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("conn error:" + ex.ToString());
            }

            return result;
        }

        public AppUser getuserprofile(string aspNetUserId)
        {
            using (var connection = Common.Database)
            {
                var parameters = new DynamicParameters();
                parameters.Add("@AspNetuUserId", aspNetUserId, DbType.String);

                var appUser = connection.QueryFirstOrDefault<AppUser>("getuserprofile", parameters, commandType: CommandType.StoredProcedure);
                return appUser;
            }
        }

        public List<selectlistitem> ddl_users(int userid, bool activeonly = true)
        {
            List<selectlistitem> result = new List<selectlistitem>();
            try
            {
                using SqlConnection sqlConnection = new SqlConnection(getConnectionString());
                sqlConnection.Open();
                using SqlCommand sqlCommand = new SqlCommand("arg.ddl_users", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@userid", userid));
                sqlCommand.Parameters.Add(new SqlParameter("@activeonly", activeonly));
                using DataTable dataTable = new DataTable();
                using (IDataReader dataReader = sqlCommand.ExecuteReader())
                {
                    if (dataReader != null)
                    {
                        dataTable.Load(dataReader);
                    }
                }

                result = dataTable.DataTableToList<selectlistitem>();
            }
            catch (Exception x)
            {
                data_logging.AddAppLogEntry(x);
            }

            return result;
        }
    }
}
