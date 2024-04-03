using Arg.DataModels;
using ArgCore.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArgCore.Models
{
    public class ClientSMTPAccounts
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        public SelectList Companies { get; set; }

        public SearchOptions SearchOptions { get; set; }

        public List<Arg.DataModels.ClientSMTPAccounts> SmtpAccountsList { get; set; }

        public Arg.DataModels.ClientSMTPAccounts ClientSMTPAccountDetail { get; set; }
    }
}
