using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arg.Core
{
    public class Utility
    {
        public static string GenerateRandomInvoiceNo(string prepend = "99")
        {
            Random generator = new Random();
            int randomNo = generator.Next(1, 1000000);
            string randomNoString = randomNo.ToString().PadLeft(6, '0');
            var invoiceNo = prepend + randomNoString;
            return invoiceNo;
        }

        public static string JoinStrings(string[] vals, string seperator = ",", string encloseIn = "'")
        {
            var txt = "";
            foreach (var val in vals)
            {
                txt += encloseIn + val + encloseIn + seperator;
            }
            if (txt.Substring(txt.Length - 1, 1) == seperator)
            {
                txt = txt.Remove(txt.Length - 1, 1);
            }
            return txt;
        }
    }
}
