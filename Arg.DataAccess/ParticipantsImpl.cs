using Dapper;
using System.Data;
namespace Arg.DataAccess
{
    public class ParticipantsImpl
    {
        public List<DataModels.Participants> GetParticipants()
        {
            const string query = @"SELECT * FROM Participants;";

            using (var connection = Common.ClientDatabase)
            {
                var allParticipants = connection.Query<DataModels.Participants>(query, commandType: CommandType.Text).ToList();
                return allParticipants;
            }
        }

        public DataModels.Participants GetParticipant(string customerId)
        {
            const string query = @"SELECT * FROM Participants
                                   WHERE (ParticipantID=@ParticipantId OR @ParticipantId = 0);";

            using (var connection = Common.ClientDatabase)
            {
                var participants = connection.QueryFirstOrDefault<DataModels.Participants>(query, new { ParticipantId  = customerId });
                return participants;
            }
        }
    }
}
