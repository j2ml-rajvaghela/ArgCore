using Arg.DataModels;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace Arg.DataAccess
{
    public class RegionsImpl
    {
        public List<Regions> GetRegions(int regionId, string q, int companyId)
        {
            var parameters = new DynamicParameters();
            if (regionId > 0)
            {
                parameters.Add("@RegionId", regionId, DbType.Int32);
            }
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }
            if (!string.IsNullOrWhiteSpace(q))
            {
                parameters.Add("@q", q, DbType.String);
            }

            using var connection = Common.Database;
            var regions = connection.Query<Regions>("GetRegions", parameters, commandType: CommandType.StoredProcedure).ToList();
            return regions;
        }

        public Regions GetRegion(int regionId, int companyId)
        {
            var parameters = new DynamicParameters();
            if (regionId > 0)
            {
                parameters.Add("@RegionId", regionId, DbType.Int32);
            }
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }

            using var connection = Common.Database;
            var region = connection.QueryFirstOrDefault<Regions>("GetRegion", parameters, commandType: CommandType.StoredProcedure);
            return region;
        }

        public List<Regions> GetRegionsMultiple(int regionId, string q, string companyId)
        {
            var parameters = new DynamicParameters();

            if (regionId > 0)
            {
                parameters.Add("@RegionId", regionId, DbType.Int32);
            }
            if (!string.IsNullOrWhiteSpace(companyId))
            {
                parameters.Add("@CompanyIds", companyId, DbType.Int32);
            }
            if (!string.IsNullOrWhiteSpace(q))
            {
                parameters.Add("@q", q, DbType.String);
            }

            using var connection = Common.Database;
            var regionsMultiple = connection.Query<Regions>("GetRegionsMultiple", parameters, commandType: CommandType.StoredProcedure).ToList();
            return regionsMultiple;
        }

        public List<Regions> GetBalanceDueRegions(int companyId)
        {
            var parameters = new DynamicParameters();
            if (companyId > 0)
            {
                parameters.Add("@CompanyId", companyId, DbType.Int32);
            }

            using var connection = Common.Database;
            var regionsMultiple = connection.Query<Regions>("GetBalanceDueRegionsByCompanyId", parameters, commandType: CommandType.StoredProcedure).ToList();
            return regionsMultiple;
        }

        public List<Regions> GetRegions()
        {
            using var connection = Common.Database;
            var regions = connection.Query<Regions>("GetAllRegions", commandType: CommandType.StoredProcedure).ToList();
            return regions;
        }

        public List<Regions> GetRegionClients()
        {
            using var connection = Common.Database;
            var regions = connection.Query<Regions>("GetRegionClients", commandType: CommandType.StoredProcedure).ToList();
            return regions;
        }

        public List<Regions> RegionExist(int companyId, string region, int regionId)
        {
            var parameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(region))
            {
                parameters.Add("@Region", region, DbType.String);
            }
            if (regionId > 0)
            {
                parameters.Add("@RegionId", regionId, DbType.Int32);
            }
            parameters.Add("@CompanyId", companyId, DbType.Int32);

            using var connection = Common.Database;
            var isRegionsExist = connection.Query<Regions>("RegionExist", parameters, commandType: CommandType.StoredProcedure).ToList();
            return isRegionsExist;
        }

        public List<Regions> GetDistictRegions()
        {
            using var connection = Common.Database;
            var distictRegions = connection.Query<Regions>("GetDistinctRegions", commandType: CommandType.StoredProcedure).ToList();
            return distictRegions;
        }

        public void SaveRegion(Regions region)
        {
            if (string.IsNullOrWhiteSpace(region.Region))
            {
                throw new Exception("Region can't be empty.");
            }

            using var connection = Common.Database;
            if (region.RegionId == 0)
            {
                connection.Insert(region);
            }
            else
            {
                connection.Update(region);
            }

        }

        public int DeleteRegion(int regionId)
        {
            const string query = @"DELETE FROM Regions 
                                   WHERE RegionId=@RegionId;";

            using var connection = Common.Database;
            var result = connection.Execute(query, new { regionId });
            return Convert.ToInt32(result);
        }
    }
}
