using Arg.DataModels;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;


namespace Arg.DataAccess
{
    public class ClientSMTPAccountsImpl
    {
        public List<ClientSMTPAccounts> GetSmtpAccounts(SearchOptions so)
        {
            var parameters = new DynamicParameters();
            if (so.CompanyId > 0)
            {
                parameters.Add("@CompanyId", so.CompanyId, DbType.Int32);
            }
            using (var connection = Common.Database)
            {
                var smtpAccounts = connection.Query<ClientSMTPAccounts>("GetSmtpAccounts", parameters,commandType: CommandType.StoredProcedure).ToList();
                return smtpAccounts;
            }
        }

        public List<ClientSMTPAccounts> GetSmtpAccounts(int companyId)
        {
            var parameters = new DynamicParameters();
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }

            using (var connection = Common.Database)
            {
                var smtpAccounts = connection.Query<ClientSMTPAccounts>("GetSmtpAccountsByCompanyId", parameters, commandType: CommandType.StoredProcedure).ToList();
                return smtpAccounts;
            }
        }

        public ClientSMTPAccounts GetSmtpAccount(int smtpAccountId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@SMTPAccountId", smtpAccountId, DbType.Int32);
            using (var connection = Common.Database)
            {
                var smtpAccount = connection.QueryFirstOrDefault<ClientSMTPAccounts>("GetSmtpAccountBySMTPAccountId", parameters, commandType: CommandType.StoredProcedure);
                if (smtpAccount != null)
                {
                    try
                    {
                        smtpAccount.Password = smtpAccount.Password.Decrypt(Common.EncryptKey);
                    }
                    catch
                    {
                        System.Diagnostics.Trace.TraceError("Password not encrypted");
                    }

                }
                return smtpAccount;
            }
        }

        public void SaveClientSMTPAccount(ClientSMTPAccounts clientSMTPAccount)
        {
            clientSMTPAccount.Password = clientSMTPAccount.Password.Encrypt(Common.EncryptKey);
            using (var connection = Common.Database)
            {
                if (clientSMTPAccount.SMTPAccountId == 0)
                {
                    connection.Insert(clientSMTPAccount);
                }
                else
                {
                    connection.Update(clientSMTPAccount);
                }
               
            }
        }

        public int DeleteClientSMTPAccount(int smtpAccountId)
        {
            const string query = "DELETE FROM ClientSMTPAccounts WHERE SMTPAccountId=@SMTPAccountId;";
            using (var connection = Common.Database)
            {
                var result = connection.Execute(query, new { @SMTPAccountId = smtpAccountId });
                return result;
            }
        }
    }
}
