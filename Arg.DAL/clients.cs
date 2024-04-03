using Arg.DataModels;
using Arg.Terms;
using System.Data;
using System.Data.SqlClient;

namespace Arg.DAL
{
    public class clients : _basedbclass
    {
        public List<Client> list_adminclients(int userid)
        {
            List<Client> result = new List<Client>();
            try
            {
                using SqlConnection sqlConnection = new SqlConnection(getConnectionString());
                sqlConnection.Open();
                using SqlCommand sqlCommand = new SqlCommand("arg.lst_adminclients", sqlConnection);
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

                result = dataTable.DataTableToList<Client>();
            }
            catch (Exception x)
            {
                data_logging.AddAppLogEntry(x);
            }

            return result;
        }

        public List<Client> list_clients(int userid)
        {
            List<Client> result = new List<Client>();
            try
            {
                using SqlConnection sqlConnection = new SqlConnection(getConnectionString());
                sqlConnection.Open();
                using SqlCommand sqlCommand = new SqlCommand("arg.lst_clients", sqlConnection);
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

                result = dataTable.DataTableToList<Client>();
            }
            catch (Exception x)
            {
                data_logging.AddAppLogEntry(x);
            }

            return result;
        }

        public Client get_client(int userid, int clientid)
        {
            Client result = new Client();
            try
            {
                using SqlConnection sqlConnection = new SqlConnection(getConnectionString());
                sqlConnection.Open();
                using SqlCommand sqlCommand = new SqlCommand("arg.get_client", sqlConnection);
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

                result = dataTable.DataTableToList<Client>().Single();
            }
            catch (Exception x)
            {
                data_logging.AddAppLogEntry(x);
            }

            return result;
        }

        public List<selectlistitem> ddl_clients(int userid)
        {
            List<selectlistitem> result = new List<selectlistitem>();
            try
            {
                using SqlConnection sqlConnection = new SqlConnection(getConnectionString());
                sqlConnection.Open();
                using SqlCommand sqlCommand = new SqlCommand("arg.ddl_clients", sqlConnection);
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

                result = dataTable.DataTableToList<selectlistitem>();
            }
            catch (Exception x)
            {
                data_logging.AddAppLogEntry(x);
            }

            return result;
        }

        public dbresult client_update(int userid, Client REC)
        {
            dbresult dbresult = new dbresult();
            try
            {
                if (REC.clientid == 0)
                {
                    dbresult.action = "insert";
                }
                else
                {
                    dbresult.action = "update";
                }

                using SqlConnection sqlConnection = new SqlConnection(getConnectionString());
                sqlConnection.Open();
                using SqlCommand sqlCommand = new SqlCommand("dbo.upd_client", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@userid", userid));
                sqlCommand.Parameters.Add(new SqlParameter("@clientid", REC.clientid));
                sqlCommand.Parameters.Add(new SqlParameter("@clientname", REC.clientname));
                sqlCommand.Parameters.Add(new SqlParameter("@nickname", REC.nickname));
                sqlCommand.Parameters.Add(new SqlParameter("@contactname", REC.contactname));
                sqlCommand.Parameters.Add(new SqlParameter("@emailaddress", REC.emailaddress));
                sqlCommand.Parameters.Add(new SqlParameter("@phonenumber", REC.phonenumber));
                sqlCommand.Parameters.Add(new SqlParameter("@street1", REC.street1));
                sqlCommand.Parameters.Add(new SqlParameter("@street2", REC.street2));
                sqlCommand.Parameters.Add(new SqlParameter("@city", REC.city));
                sqlCommand.Parameters.Add(new SqlParameter("@statecode", REC.statecode));
                sqlCommand.Parameters.Add(new SqlParameter("@postalcode", REC.postalcode));
                sqlCommand.Parameters.Add(new SqlParameter("@countryid", REC.countryid));
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

        public List<selectlistitem> ddl_groups(int userid)
        {
            List<selectlistitem> result = new List<selectlistitem>();
            try
            {
                using SqlConnection sqlConnection = new SqlConnection(getConnectionString());
                sqlConnection.Open();
                using SqlCommand sqlCommand = new SqlCommand("arg.ddl_groups", sqlConnection);
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
