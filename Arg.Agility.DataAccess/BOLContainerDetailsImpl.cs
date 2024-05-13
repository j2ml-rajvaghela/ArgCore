using Arg.Agility.DataModels;
using Arg.DataAccess;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;


namespace Arg.Agility.DataAccess
{
    public class BOLContainerDetailsImpl
    {
        private readonly SqlConnection _connection;

        public BOLContainerDetailsImpl()
        {
            _connection = Common.ClientDatabase;
        }
        public List<BOLContainerDetails> GetDistinctType()
        {
            const string query = @"SELECT DISTINCT b.UnitType+ ' ' + UnitTypeDescription AS UnitType, b.UnitType AS UnitTypeCode 
                                   FROM bolcontainerdetails b
                                   WHERE b.unittype IS NOT NULL
                                   ORDER BY UnitType;";

            return _connection.Query<BOLContainerDetails>(query, commandType: CommandType.Text).ToList();
        }

        public List<BOLContainerDetails> GetBOLItemDetail(string jobNumber)
        {
            const string query = @"SELECT * FROM BOLContainerDetails C
                                   WHERE c.JobNumber=@JobNumber
                                   ORDER BY UnitSeqNumber;";

            return _connection.Query<BOLContainerDetails>(query, new { JobNumber = jobNumber }).ToList();
        }

        public List<BOLContainerDetails> GetUnitType()
        {
            const string query = @"SELECT DISTINCT b.UnitType+ ' ' + UnitTypeDescription AS UnitType, b.UnitType AS UnitTypeCode 
                                   FROM bolcontainerdetails b
                                   WHERE b.unittype IS NOT NULL
                                   ORDER BY UnitType;";

            return _connection.Query<BOLContainerDetails>(query, commandType: CommandType.Text).ToList();
        }

        public List<BOLContainerDetails> GetHazMatFlag()
        {
            const string query = @"SELECT DISTINCT b.HazMatFlag , b.HazMatFlag as HazMatFlagCode 
                                   FROM bolcontainerdetails b
                                   WHERE b.HazMatFlag IS NOT NULL
                                   ORDER BY HazMatFlag;";

            return _connection.Query<BOLContainerDetails>(query, commandType: CommandType.Text).ToList();
        }
    }
}
