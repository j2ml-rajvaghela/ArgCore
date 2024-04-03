using Arg.DataModels;
using Arg.Terms;
using System.Data;
using System.Data.SqlClient;

namespace Arg.DAL
{
    public class security : _basedbclass
    {
        public List<permission> permissions_list(int userid, int clientid = 0)
        {
            List<permission> result = new List<permission>();
            try
            {
                using SqlConnection sqlConnection = new SqlConnection(getConnectionString());
                sqlConnection.Open();
                using SqlCommand sqlCommand = new SqlCommand("arg.lst_permissions", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@userid", userid));
                sqlCommand.Parameters.Add(new SqlParameter("@clientid", clientid));
                using DataTable dataTable = new DataTable();
                using (IDataReader dataReader = sqlCommand.ExecuteReader())
                {
                    if (dataReader != null)
                    {
                        dataTable.Load(dataReader);
                    }
                }

                result = dataTable.DataTableToList<permission>();
            }
            catch (Exception x)
            {
                data_logging.AddAppLogEntry(x);
            }

            return result;
        }

        public dbresult permissions_update(int userid, List<permission> LST)
        {
            dbresult dbresult = new dbresult();
            try
            {
                using SqlConnection sqlConnection = new SqlConnection(getConnectionString());
                sqlConnection.Open();
                using SqlCommand sqlCommand = new SqlCommand("arg.upd_permission", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@userid", userid));
                sqlCommand.Parameters.Add(new SqlParameter("@permissionid", DbType.Int32));
                sqlCommand.Parameters.Add(new SqlParameter("@groupobjectid", DbType.Int32));
                sqlCommand.Parameters.Add(new SqlParameter("@groupid", DbType.Int32));
                sqlCommand.Parameters.Add(new SqlParameter("@permissionlevel", DbType.Int32));
                sqlCommand.Parameters.Add(new SqlParameter("@markfordelete", DbType.Boolean));
                int num = 0;
                foreach (permission item in LST)
                {
                    sqlCommand.Parameters["@permissionid"].Value = item.permissionid;
                    sqlCommand.Parameters["@groupobjectid"].Value = item.groupobjectid;
                    sqlCommand.Parameters["@groupid"].Value = item.groupid;
                    sqlCommand.Parameters["@permissionlevel"].Value = item.permissionlevel;
                    sqlCommand.Parameters["@markfordelete"].Value = item.markfordelete;
                    int num2 = sqlCommand.ExecuteNonQuery();
                    num += num2;
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

        public List<groupobject> groupobjects_list(int userid, int clientid = 0)
        {
            List<groupobject> list = new List<groupobject>();
            List<permission> list2 = new List<permission>();
            try
            {
                using SqlConnection sqlConnection = new SqlConnection(getConnectionString());
                sqlConnection.Open();
                using SqlCommand sqlCommand = new SqlCommand("arg.lst_permissions", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@userid", userid));
                sqlCommand.Parameters.Add(new SqlParameter("@clientid", clientid));
                using (DataTable dataTable = new DataTable())
                {
                    using (IDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        if (dataReader != null)
                        {
                            dataTable.Load(dataReader);
                        }
                    }

                    list2 = dataTable.DataTableToList<permission>();
                }

                string value = "";
                groupobject groupobject = new groupobject();
                foreach (permission item in list2)
                {
                    if (!item.objectname.Equals(value))
                    {
                        groupobject = new groupobject();
                        groupobject.objectname = item.objectname;
                        groupobject.groupobjectid = item.groupobjectid;
                        list.Add(groupobject);
                    }

                    groupobject.permissionlist.Add(item);
                    value = item.objectname;
                }
            }
            catch (Exception x)
            {
                data_logging.AddAppLogEntry(x);
            }

            return list;
        }
    }
}
