using Arg.DataModels;
using Dapper;
using Dapper.Contrib.Extensions;

namespace Arg.DataAccess
{
    public class ClientsImpl
    {

        public Client GetClient(int clientId)
        {
            const string query = @"SELECT * FROM arg.clients 
                                   WHERE ClientId = @ClientId;";

            using var connection = Common.Database;
            var client = connection.QueryFirstOrDefault<Client>(query, new { clientId });
            return client;
        }

        public void SaveClient(Client client)
        {
            if (string.IsNullOrWhiteSpace(client.clientname))
            {
                throw new Exception("Client Name can't be empty.");
            }
            if (string.IsNullOrWhiteSpace(client.emailaddress))
            {
                throw new Exception("Email Address can't be empty.");
            }

            using var connection = Common.Database;
            connection.Insert(client);
        }
    }
}
