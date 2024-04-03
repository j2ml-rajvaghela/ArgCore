using System.Data;
using System.Data.SqlClient;
using Arg.DataModels;
using Arg.Terms;

namespace Arg.DAL
{
    public class groups : _basedbclass
    {
        public List<group> listgroups(int userid)
        {
            List<group> result = new List<group>();
            try
            {
                using SqlConnection sqlConnection = new SqlConnection(getConnectionString());
                sqlConnection.Open();
                using SqlCommand sqlCommand = new SqlCommand("arg.lst_groups", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@userid", userid));
                using DataTable dataTable = new DataTable();
                using (IDataReader dataReader = sqlCommand.ExecuteReader())
                {
                    if (dataReader != null)
                    {
                        dataTable.Load(dataReader);
                    }
                }

                result = dataTable.DataTableToList<group>();
            }
            catch (Exception x)
            {
                data_logging.AddAppLogEntry(x);
            }

            return result;
        }

        public group get_group(int userid, int groupid)
        {
            group result = new group();
            try
            {
                using SqlConnection sqlConnection = new SqlConnection(getConnectionString());
                sqlConnection.Open();
                using SqlCommand sqlCommand = new SqlCommand("arg.get_group", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@userid", userid));
                sqlCommand.Parameters.Add(new SqlParameter("@groupid", groupid));
                using DataTable dataTable = new DataTable();
                using (IDataReader dataReader = sqlCommand.ExecuteReader())
                {
                    if (dataReader != null)
                    {
                        dataTable.Load(dataReader);
                    }
                }

                result = dataTable.DataTableToList<group>().Single();
            }
            catch (Exception x)
            {
                data_logging.AddAppLogEntry(x);
            }

            return result;
        }

        public dbresult update_group(int userid, group REC)
        {
            dbresult dbresult = new dbresult();
            try
            {
                if (REC.groupid == 0)
                {
                    dbresult.action = "insert";
                }
                else
                {
                    dbresult.action = "update";
                }

                using SqlConnection sqlConnection = new SqlConnection(getConnectionString());
                sqlConnection.Open();
                using SqlCommand sqlCommand = new SqlCommand("arg.upd_group", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@userid", userid));
                sqlCommand.Parameters.Add(new SqlParameter("@groupid", REC.groupid));
                sqlCommand.Parameters.Add(new SqlParameter("@groupname", REC.groupname));
                sqlCommand.Parameters.Add(new SqlParameter("@clientid", REC.clientid));
                sqlCommand.Parameters.Add(new SqlParameter("@isactive", REC.isactive));
                dbresult.newid = (int)sqlCommand.ExecuteScalar();
                dbresult.issuccessful = true;
                dbresult.message = Resource1.msgUpdateSuccess;
                dbresult.messagetype = "ok";
            }
            catch (Exception x)
            {
                data_logging.AddAppLogEntry(x);
            }

            return dbresult;
        }

        public List<groupmember> list_groupmembersall(int userid, int groupid)
        {
            List<groupmember> result = new List<groupmember>();
            try
            {
                using SqlConnection sqlConnection = new SqlConnection(getConnectionString());
                sqlConnection.Open();
                using SqlCommand sqlCommand = new SqlCommand("arg.lst_groupmembersall", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@userid", userid));
                sqlCommand.Parameters.Add(new SqlParameter("@groupid", groupid));
                using DataTable dataTable = new DataTable();
                using (IDataReader dataReader = sqlCommand.ExecuteReader())
                {
                    if (dataReader != null)
                    {
                        dataTable.Load(dataReader);
                    }
                }

                result = dataTable.DataTableToList<groupmember>();
            }
            catch (Exception x)
            {
                data_logging.AddAppLogEntry(x);
            }

            return result;
        }

        public dbresult update_groupmembers(int userid, int groupid, List<groupmember> LST)
        {
            dbresult dbresult = new dbresult();
            try
            {
                using SqlConnection sqlConnection = new SqlConnection(getConnectionString());
                sqlConnection.Open();
                using SqlCommand sqlCommand = new SqlCommand("arg.upd_groupmember", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@groupid", groupid));
                sqlCommand.Parameters.Add(new SqlParameter("@groupmemberid", DbType.Int32));
                sqlCommand.Parameters.Add(new SqlParameter("@userid", DbType.Int32));
                sqlCommand.Parameters.Add(new SqlParameter("@ismember", DbType.Boolean));
                int num = 0;
                foreach (groupmember item in LST)
                {
                    if ((item.groupmemberid == 0 && item.ismember) || (item.groupmemberid > 0 && !item.ismember))
                    {
                        sqlCommand.Parameters["@groupmemberid"].Value = item.groupmemberid;
                        sqlCommand.Parameters["@userid"].Value = item.userid;
                        sqlCommand.Parameters["@ismember"].Value = item.ismember;
                        int num2 = sqlCommand.ExecuteNonQuery();
                        num += num2;
                    }
                }

                dbresult.issuccessful = true;
                dbresult.action = Resource1.msgActionMultipleUpdates;
                dbresult.message = Resource1.msgMultipleUpdatesSuccess;
                dbresult.updates = num;
                dbresult.messagetype = "ok";
            }
            catch (Exception x)
            {
                data_logging.AddAppLogEntry(x);
            }

            return dbresult;
        }

        public List<groupmember> list_groupmembershipsall(int appuserid, int userid)
        {
            List<groupmember> result = new List<groupmember>();
            try
            {
                using SqlConnection sqlConnection = new SqlConnection(getConnectionString());
                sqlConnection.Open();
                using SqlCommand sqlCommand = new SqlCommand("arg.lst_groupmembershipsall", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@appuserid", appuserid));
                sqlCommand.Parameters.Add(new SqlParameter("@userid", userid));
                using DataTable dataTable = new DataTable();
                using (IDataReader dataReader = sqlCommand.ExecuteReader())
                {
                    if (dataReader != null)
                    {
                        dataTable.Load(dataReader);
                    }
                }

                result = dataTable.DataTableToList<groupmember>();
            }
            catch (Exception x)
            {
                data_logging.AddAppLogEntry(x);
            }

            return result;
        }
    }
}
