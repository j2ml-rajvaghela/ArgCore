using Arg.Agility.DataModels;
using Arg.DataAccess;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Arg.Agility.DataAccess
{
    public class BOLContainerDetailsImpl
    {
        private readonly SqlConnection _clientDbConnection;
        public BOLContainerDetailsImpl() => _clientDbConnection = Common.ClientDatabase;

        public List<BOLContainerDetails> GetDistinctType()
        {
            const string query = @"SELECT DISTINCT b.UnitType+ ' ' + UnitTypeDescription AS UnitType, b.UnitType AS UnitTypeCode FROM bolcontainerdetails b
                                   WHERE b.unittype IS NOT NULL
                                   ORDER BY UnitType;";

            var distinctTypes = _clientDbConnection.Query<DataModels.BOLContainerDetails>(query, commandType: CommandType.Text).ToList();
            return distinctTypes;
        }

        public List<BOLContainerDetails> GetBOLItemDetail(string jobNumber)
        {
            const string query = @"SELECT * FROM BOLContainerDetails C
                                   WHERE c.JobNumber=@JobNumber
                                   ORDER BY UnitSeqNumber;";

            var bOLItemDetail = _clientDbConnection.Query<BOLContainerDetails>(query, new { JobNumber = jobNumber }).ToList();
            return bOLItemDetail;
        }

        public List<BOLContainerDetails> GetUnitType()
        {
            const string query = @"SELECT DISTINCT b.UnitType+ ' ' + UnitTypeDescription AS UnitType, b.UnitType AS UnitTypeCode FROM bolcontainerdetails b
                                   WHERE b.unittype IS NOT NULL
                                   ORDER BY UnitType;";

            var unitTypes = _clientDbConnection.Query<BOLContainerDetails>(query, commandType: CommandType.Text).ToList();
            return unitTypes;
        }

        public List<BOLContainerDetails> GetHazMatFlag()
        {
            const string query = @"SELECT DISTINCT b.HazMatFlag , b.HazMatFlag as HazMatFlagCode FROM bolcontainerdetails b
                                   WHERE b.HazMatFlag IS NOT NULL
                                   ORDER BY HazMatFlag;";

            var hazMatFlag = _clientDbConnection.Query<BOLContainerDetails>(query, commandType: CommandType.Text).ToList();
            return hazMatFlag;
        }
    }
}
