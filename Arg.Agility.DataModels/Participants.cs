using Dapper.Contrib.Extensions;

namespace Arg.Agility.DataModels
{
    [Table("Participants")]
    public class Participants
    {
        public string ParticipantID { get; set; }
        public string ParticipantName { get; set; }
        public string ParticipantType { get; set; }
        public string loaded { get; set; }
    }
}
