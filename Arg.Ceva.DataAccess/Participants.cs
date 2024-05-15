using Arg.DataAccess;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Arg.Ceva.DataAccess
{
    public class Participants
    {
        private readonly SqlConnection _connection;

        public Participants()
        {
            _connection = Common.ClientDatabase;
        }

        [Table("Participants")]
        public class Participant
        {
            public string ParticipantID { get; set; }
            public string Region { get; set; }
            public string Type { get; set; }
            public string Name { get; set; }
            public string Zip { get; set; }
            public DateTime? AddedOn { get; set; }

            [Computed]
            public string Shipper { get; set; }

            [Computed]
            public string Consignee { get; set; }

            [Computed]
            public string Payor { get; set; }
        }

        public Participant GetShipper(string SHPRNO)
        {
            const string query = @"SELECT p.*,CONCAT(p.Name,' (',p.ParticipantID,')') AS Shipper 
                                   FROM Participants p
                                   WHERE p.ParticipantID=@ParticipantID AND p.Type = 'Shipper';";

            return _connection.QueryFirstOrDefault<Participant>(query, new { ParticipantID = SHPRNO });
        }

        public Participant GetPayor(string CSORNO)
        {
            const string query = @"SELECT p.*,concat(p.Name,' (',p.ParticipantID,')') AS Payor 
                                   FROM Participants p
                                   WHERE p.ParticipantID=@ParticipantID AND p.Type = 'Consignee';";

            return _connection.QueryFirstOrDefault<Participant>(query, new { ParticipantID = CSORNO });
        }

        public Participant GetParticipant(string customerId)
        {
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(customerId))
            {
                parameters.Add("@ParticipantID", customerId, DbType.String);
            }
            const string query = @"SELECT * 
                                   FROM Participants 
                                   WHERE ParticipantID=@ParticipantID AND Type = 'customer';";

            return _connection.QueryFirstOrDefault<Participant>(query, parameters);
        }

        public Participant GetConsignee(string CSEENO)
        {
            const string query = @"SELECT p.*,CONCAT(p.Name,' (',p.ParticipantID,')') AS Consignee 
                                   FROM Participants p
                                   WHERE p.ParticipantID=@ParticipantID AND p.Type = 'Consignee';";

            return _connection.QueryFirstOrDefault<Participant>(query, new { ParticipantID = CSEENO });
        }
    }
}
