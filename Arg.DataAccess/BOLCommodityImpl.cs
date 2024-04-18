using Arg.DataModels;
using Dapper;

namespace Arg.DataAccess
{
    public class BOLCommodityImpl
    {
        public List<BOLCommodity> GetBOLItemDetail(string bolNo)
        {
            const string query = @"SELECT CONCAT(c.ContainerID,' ',WeightKilos) AS Container,c.CBF,c.CommodityDescription,
                                   ISNULL(c.SetTemperature,0) AS SetTemperature,ISNULL(c.MinTemperature,0) AS MinTemperature,
                                   ISNULL(c.MaxTemperature,0) AS MaxTemperature,c.CelsiusOrFahrenheit,c.CelsiusOrFahrenheit,
                                   CONCAT(c.CommodityCode,' ',c.CommodityDescription) AS Commodity,c.SITFlag,c.CommodityCode,
                                   CONCAT(c.ContainerID,' ',b.Size,' ',b.Type,' ',b.Weight) AS ContainerDetails,c.ContainerID,b.Size,b.Type,b.Weight,
                                   ISNULL(h.UNHazmatCode,'') AS UNHazmatCode,ISNULL(h.PlacardNumber,'') AS PlacardNumber,
                                   CONCAT(h.CommodityDescription,' ',h.CommodityDescription2) AS CommodityDescriptions FROM BOLCommodity c
                                   AND CROSS APPLY (SELECT TOP(1) * FROM BOLContainers b WHERE b.BOL#=c.BOL#) AS b
                                   LEFT JOIN BOLHazardous h ON h.BOL#=c.BOL#
                                   WHERE c.BOL#=@BolNO;";

            using (var connection = Common.ClientDatabase)
            {
                var bOLItemDetail = connection.Query<BOLCommodity>(query, new { BolNO  = bolNo }).ToList();
                return bOLItemDetail;
            }
        }
    }
}
