using Arg.DataModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arg.DAL
{
    public class billofladings : _basedbclass
    {
        public List<billoflading> billofladings_list(int userid, int clientid, int customerid, int index = 0)
        {
            List<billoflading> result = new List<billoflading>();
            try
            {
                using SqlConnection sqlConnection = new SqlConnection(getConnectionString(clientid));
                sqlConnection.Open();
                using SqlCommand sqlCommand = new SqlCommand("dbo.lst_billofladings", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@userid", userid));
                sqlCommand.Parameters.Add(new SqlParameter("@clientid", clientid));
                sqlCommand.Parameters.Add(new SqlParameter("@customerid", customerid));
                sqlCommand.Parameters.Add(new SqlParameter("@index", index));
                using DataTable dataTable = new DataTable();
                using (IDataReader dataReader = sqlCommand.ExecuteReader())
                {
                    if (dataReader != null)
                    {
                        dataTable.Load(dataReader);
                    }
                }

                result = dataTable.DataTableToList<billoflading>();
            }
            catch (Exception ex)
            {
                if (ex.Message == "Database not available at this time")
                {
                    throw;
                }

                data_logging.AddAppLogEntry(ex);
            }

            return result;
        }


        public billoflading billoflading_get(int userid, int clientid, int customerid, int billofladingid)
        {
            billoflading result = new billoflading();
            try
            {
                using SqlConnection sqlConnection = new SqlConnection(getConnectionString(clientid));
                sqlConnection.Open();
                using SqlCommand sqlCommand = new SqlCommand("dbo.get_billoflading", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@userid", userid));
                sqlCommand.Parameters.Add(new SqlParameter("@clientid", clientid));
                sqlCommand.Parameters.Add(new SqlParameter("@customerid", customerid));
                sqlCommand.Parameters.Add(new SqlParameter("@billofladingid", billofladingid));
                using DataTable dataTable = new DataTable();
                using (IDataReader dataReader = sqlCommand.ExecuteReader())
                {
                    if (dataReader != null)
                    {
                        dataTable.Load(dataReader);
                    }
                }

                result = dataTable.DataTableToList<billoflading>().Single();
            }
            catch (Exception ex)
            {
                if (ex.Message == "Database not available at this time")
                {
                    throw;
                }

                data_logging.AddAppLogEntry(ex);
            }

            return result;
        }

        public List<billoflading> bolsearch_list(int userid, int clientid, string bolnumber, int index = 0)
        {
            List<billoflading> result = new List<billoflading>();
            try
            {
                using SqlConnection sqlConnection = new SqlConnection(getConnectionString(clientid));
                sqlConnection.Open();
                using SqlCommand sqlCommand = new SqlCommand("dbo.lst_billofladings_bolsearch", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@userid", userid));
                sqlCommand.Parameters.Add(new SqlParameter("@clientid", clientid));
                sqlCommand.Parameters.Add(new SqlParameter("@bolnumber", bolnumber));
                sqlCommand.Parameters.Add(new SqlParameter("@index", index));
                using DataTable dataTable = new DataTable();
                using (IDataReader dataReader = sqlCommand.ExecuteReader())
                {
                    if (dataReader != null)
                    {
                        dataTable.Load(dataReader);
                    }
                }

                result = dataTable.DataTableToList<billoflading>();
            }
            catch (Exception ex)
            {
                if (ex.Message == "Database not available at this time")
                {
                    throw;
                }

                data_logging.AddAppLogEntry(ex);
            }

            return result;
        }

        public billoflading billoflading_next(int userid, int clientid, int customerid, string bolnumber)
        {
            billoflading result = new billoflading();
            try
            {
                using SqlConnection sqlConnection = new SqlConnection(getConnectionString(clientid));
                sqlConnection.Open();
                using SqlCommand sqlCommand = new SqlCommand("dbo.get_billofladingnext", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@userid", userid));
                sqlCommand.Parameters.Add(new SqlParameter("@clientid", clientid));
                sqlCommand.Parameters.Add(new SqlParameter("@customerid", customerid));
                sqlCommand.Parameters.Add(new SqlParameter("@bolnumber", bolnumber));
                using DataTable dataTable = new DataTable();
                using (IDataReader dataReader = sqlCommand.ExecuteReader())
                {
                    if (dataReader != null)
                    {
                        dataTable.Load(dataReader);
                    }
                }

                result = dataTable.DataTableToList<billoflading>().Single();
            }
            catch (Exception ex)
            {
                if (ex.Message == "Database not available at this time")
                {
                    throw;
                }

                data_logging.AddAppLogEntry(ex);
            }

            return result;
        }

        public billoflading billoflading_prev(int userid, int clientid, int customerid, string bolnumber)
        {
            billoflading result = new billoflading();
            try
            {
                using SqlConnection sqlConnection = new SqlConnection(getConnectionString(clientid));
                sqlConnection.Open();
                using SqlCommand sqlCommand = new SqlCommand("dbo.get_billofladingprev", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@userid", userid));
                sqlCommand.Parameters.Add(new SqlParameter("@clientid", clientid));
                sqlCommand.Parameters.Add(new SqlParameter("@customerid", customerid));
                sqlCommand.Parameters.Add(new SqlParameter("@bolnumber", bolnumber));
                using DataTable dataTable = new DataTable();
                using (IDataReader dataReader = sqlCommand.ExecuteReader())
                {
                    if (dataReader != null)
                    {
                        dataTable.Load(dataReader);
                    }
                }

                result = dataTable.DataTableToList<billoflading>().Single();
            }
            catch (Exception ex)
            {
                if (ex.Message == "Database not available at this time")
                {
                    throw;
                }

                data_logging.AddAppLogEntry(ex);
            }

            return result;
        }
    }
}
