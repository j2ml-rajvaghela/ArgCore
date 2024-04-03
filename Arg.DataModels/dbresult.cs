using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arg.DataModels
{
    public class dbresult
    {
        public int id { get; set; }

        public int newid { get; set; }

        public string action { get; set; }

        public string message { get; set; }

        public string messagetype { get; set; }

        public string messagedetails { get; set; }

        public bool issuccessful { get; set; }

        public int inserts { get; set; }

        public int updates { get; set; }

        public int deletes { get; set; }
    }
}
