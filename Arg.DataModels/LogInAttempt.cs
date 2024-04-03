using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arg.DataModels
{

    public class LogInAttempt
    {
        public int LoginAttemptId { get; set; }
        public string UserName { get; set; }
        public string IPAddress { get; set; }
        public string ErrorMessage { get; set; }
        public bool Autheticated { get; set; }
    }
}
