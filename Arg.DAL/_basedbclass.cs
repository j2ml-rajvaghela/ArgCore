using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace Arg.DAL
{
    public class _basedbclass : IDisposable
    {
        public string errorMessage = "";

        private bool disposedValue = false;

        public string getConnectionString(int clientId = 0)
        {
            string cs = "";
            try
            {
                if (clientId == 0)
                {
                    cs = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                }
                else
                {
                    cs = ConfigurationManager.ConnectionStrings["ClientDBName" + clientId].ConnectionString;
                }
            }
            catch (Exception)
            {
                throw new Exception("Database not available at this time");
            }
            return cs;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(disposing: true);
        }
    }
}
