using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Arg.DataModels
{
    [Table("ClientSMTPAccounts")]
    public class ClientSMTPAccounts
    {
        [Dapper.Contrib.Extensions.Key]
        public int SMTPAccountId { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string SMTPClient { get; set; }

        [Required]
        public int Port { get; set; }

        [Required]
        public string FromName { get; set; }

        [Required]
        public string FromEmail { get; set; }

        [Computed]
        public string Company { get; set; }
    }
}
