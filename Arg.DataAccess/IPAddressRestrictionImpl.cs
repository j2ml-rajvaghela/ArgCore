using Arg.DataModels;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;
using System.Net;

namespace Arg.DataAccess
{
    public class IPAddressRestrictionImpl
    {
        public bool IsInRange(int companyId, string address)
        {
            bool result = true;
            var IpAdds = GetIPAddresses(companyId);

            foreach (var item in IpAdds)
            {
                long ipStart = 0;
                IPAddress ipAddress;
                if (IPAddress.TryParse(item.BeginningIp.Trim(), out ipAddress))
                {
                    byte[] bytes = ipAddress.GetAddressBytes();
                    Array.Reverse(bytes);
                    ipStart = BitConverter.ToInt32(bytes, 0);
                }

                long ipEnd = 0;
                if (IPAddress.TryParse(item.EndingIp.Trim(), out ipAddress))
                {
                    byte[] bytes = ipAddress.GetAddressBytes();
                    Array.Reverse(bytes);
                    ipEnd = BitConverter.ToInt32(bytes, 0);
                }

                long ip = 0;
                if (IPAddress.TryParse(address.Trim(), out ipAddress))
                {
                    byte[] bytes = ipAddress.GetAddressBytes();
                    Array.Reverse(bytes);
                    ip = BitConverter.ToInt32(bytes, 0);
                }

                if (!(ip >= ipStart && ip <= ipEnd))
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            return result;
        }

        public List<IPAddressRestriction> GetIPAddresses(int companyId, string beginningIP = null, string endingIP = null, int? iPAddressRestrictionId = 0)
        {
            var parameters = new DynamicParameters();
            if (companyId != 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }
            if (iPAddressRestrictionId != 0)
            {
                parameters.Add("@IPAddressRestrictionId", iPAddressRestrictionId, DbType.Int32);
            }
            if (!string.IsNullOrEmpty(beginningIP))
            {
                parameters.Add("@BeginningIp", beginningIP, DbType.String);
            }
            if (!string.IsNullOrEmpty(endingIP))
            {
                parameters.Add("@EndingIP", endingIP, DbType.String);
            }
            using (var connection = Common.Database)
            { 
                var iPAddresses = connection.Query<IPAddressRestriction>("GetIPAddress", parameters, commandType: CommandType.StoredProcedure).ToList();
                return iPAddresses;
            }
        }

        public void SaveIPAddress(IPAddressRestriction iPAddressRestriction)
        {
            using (var connection = Common.Database)
            {
                if (iPAddressRestriction.IPAddressRestrictionId == 0)
                {
                    connection.Insert(iPAddressRestriction);
                }
                else
                {
                    connection.Update(iPAddressRestriction);
                }
            }
        }

        public int DeleteIPAddress(int iPAddressRestrictionId)
        {
            const string query = "DELETE FROM IPAddressRestriction WHERE IPAddressRestrictionId=@IPAddressRestrictionId;";
            using (var connection = Common.Database)
            {
                var result = connection.Execute(query, new { @IPAddressRestrictionId = iPAddressRestrictionId });
                return result;
            }
        }
    }
}
