using Dapper.Contrib.Extensions;

namespace Arg.DataModels
{
    [Table("ActivityStats")]
    public class ActivityStats
    {
        [Key]
        public int StatsId { get; set; }
        public string UserId { get; set; }
        public string Note { get; set; }
        public DateTime AddedOn { get; set; }
        public string IpAddress { get; set; }
        public int ClientId { get; set; }
        public string WebPage { get; set; }
        public string BolNo { get; set; }
        public string EventType { get; set; }
        public string UserName { get; set; }
        public string ClientName { get; set; }
        public string RoleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EventCount { get; set; }
        public DateTime ViewDate { get; set; }
        public string ParticipantName { get; set; }
        public string Name
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        public string Color { get; set; }
        public string GraphAnalyst
        {
            get
            {
                return ViewDate.ToString("d") + '\n' + ClientName;
            }
        }
    }
}
