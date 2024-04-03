using Arg.DataModels;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arg.DataAccess
{
    public class ContainerEventHistoryImpl
    {
        public List<ContainerEventHistory> GetContEventHist(string bookingId, string bolNo)
        {
            string query = $"SELECT LEFT(h.ContainerID,10) , LEFT(h.ContainerID,10), CONCAT(h.ContainerID,' ',h.Size,' ',h.Type) AS Container,h.EventDateTime," +
                                 $"(SELECT CONCAT(h.FromLocationCode,' ',l.CityState) FROM Locations l WHERE LocationCode=h.FromLocationCode) AS FromCode," +
                                 $"(SELECT CONCAT(h.ToLocationCode,' ',l.CityState) FROM  Locations l WHERE LocationCode=h.ToLocationCode) AS ToCode," +
                                 $"CONCAT(h.EventType,' ',t.EventDescription) AS EventType,h.LoadStatus" +
                                 $"FROM ContainerEventHistory h" +
                                 $"INNER JOIN ContainerEventTypes t ON t.EventType=h.EventType" +
                                 $"WHERE (SELECT COUNT (*) FROM BOLHeader bh INNER JOIN BOLCommodity bc ON bc.bol#=bh.bol# WHERE  bh.bol#=h.bol# AND  bh.BOL#= '{bolNo}' AND LEFT (bc.ContainerID,10) = h.Container10Digit) > 0) OR" +
                                 $"(SELECT COUNT(*) FROM bolheader bb INNER JOIN BOLCommodity cc ON bb.BOL#= cc.BOL# WHERE bb.BOL#='{bolNo}' " +
                                 $"AND (h.BookingID LIKE '%' + bb.BookingID + '%' AND LEFT(cc.ContainerID,10) = h.Container10Digit)) > 0 AND h.BookingID LIKE '%{bookingId}%' OR " +
                                 $"h.BOL#='{bolNo}'" +
                                 $"ORDER BY h.EventDateTime;";

            using (var connection = Common.ClientDatabase)
            {
                var contEventHist = connection.Query<ContainerEventHistory>(query).ToList();
                return contEventHist;
            }
        }
    }
}
