using Arg.DataModels;
using Arg.Terms;
using System.Data;
using System.Data.SqlClient;

namespace Arg.DAL
{
    public class misc : _basedbclass
    {
        public List<region> listregions(int userid)
        {
            List<region> result = new List<region>();
            try
            {
                using SqlConnection sqlConnection = new SqlConnection(getConnectionString());
                sqlConnection.Open();
                using SqlCommand sqlCommand = new SqlCommand("arg.lst_regions", sqlConnection);
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

                result = dataTable.DataTableToList<region>();
            }
            catch (Exception x)
            {
                data_logging.AddAppLogEntry(x);
            }

            return result;
        }

        public dbresult regions_update(int userid, List<region> LST)
        {
            dbresult dbresult = new dbresult();
            try
            {
                using SqlConnection sqlConnection = new SqlConnection(getConnectionString());
                sqlConnection.Open();
                using SqlCommand sqlCommand = new SqlCommand("arg.upd_region", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@userid", userid));
                sqlCommand.Parameters.Add(new SqlParameter("@regionid", DbType.Int32));
                sqlCommand.Parameters.Add(new SqlParameter("@regioncode", DbType.String));
                sqlCommand.Parameters.Add(new SqlParameter("@regionname", DbType.String));
                sqlCommand.Parameters.Add(new SqlParameter("@isactive", DbType.Boolean));
                int num = 0;
                foreach (region item in LST)
                {
                    sqlCommand.Parameters["@regionid"].Value = item.regionid;
                    sqlCommand.Parameters["@regioncode"].Value = item.regioncode;
                    sqlCommand.Parameters["@regionname"].Value = item.regionname;
                    sqlCommand.Parameters["@isactive"].Value = item.isactive;
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

        public List<container> containers_list(int userid, int clientid, int billofladingid)
        {
            List<container> result = new List<container>();
            try
            {
                using SqlConnection sqlConnection = new SqlConnection(getConnectionString(clientid));
                sqlConnection.Open();
                using SqlCommand sqlCommand = new SqlCommand("dbo.lst_containers", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@userid", userid));
                sqlCommand.Parameters.Add(new SqlParameter("@clientid", clientid));
                sqlCommand.Parameters.Add(new SqlParameter("@billofladingid", billofladingid));
                using DataTable dataTable = new DataTable();
                using (IDataReader dataReader = sqlCommand.ExecuteReader())
                {
                    if (dataReader != null)
                    {
                        dataTable.Load(dataReader);
                    }
                }

                result = dataTable.DataTableToList<container>();
            }
            catch (Exception x)
            {
                data_logging.AddAppLogEntry(x);
            }

            return result;
        }

        public List<selectlistitem> countries_ddl(int userid, bool activeonly)
        {
            List<selectlistitem> result = new List<selectlistitem>();
            try
            {
                using SqlConnection sqlConnection = new SqlConnection(getConnectionString());
                sqlConnection.Open();
                using SqlCommand sqlCommand = new SqlCommand("argocean.dbo.ddl_countries", sqlConnection);
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

