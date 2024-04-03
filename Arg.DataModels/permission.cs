using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arg.DataModels
{
    public class permission
    {
        public int permissionid { get; set; }

        public int clientid { get; set; }

        public int groupobjectid { get; set; }

        public int groupid { get; set; }

        public string objectname { get; set; }

        public string groupname { get; set; }

        public int defaultlevel { get; set; }

        public int permissionlevel { get; set; }

        public bool changed { get; set; }

        public bool markfordelete { get; set; }

        public string comment { get; set; }
    }
}
