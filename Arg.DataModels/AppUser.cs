using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arg.DataModels
{
    public class AppUser
    {
        public int AppUserId { get; set; }
        public string AspnetUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string ClientCode { get; set; }
    }
}
