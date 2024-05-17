using Arg.DataModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;

namespace Arg.DataAccess
{
    public static class ActiveClient
    {
        public static ArgClientsImpl _argClients = new ArgClientsImpl();

        private static readonly IHttpContextAccessor _httpContextAccessor; 

        public static ArgClient Get()
        {
            var client = new ArgClient();
            if (_httpContextAccessor.HttpContext.Session.TryGetValue("ActiveClient", out var clientData))
            {
                client = System.Text.Json.JsonSerializer.Deserialize<ArgClient>(clientData);
            }
            return client;

        }

        public static ArgClient Set(int companyId)
        {
            var client = _argClients.GetArgClient(companyId, "");
            if (client != null)
            {
                _httpContextAccessor.HttpContext.Session.Set("ActiveClient", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(client).ToString()));
            }
            return client;
        }

        public static ArgClient Info
        {
            get
            {
                return Get();
            }
        }
    }
}
