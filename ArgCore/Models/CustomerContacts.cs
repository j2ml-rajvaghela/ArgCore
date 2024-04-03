using ArgCore.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArgCore.Models
{
    public class CustomerContacts
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        public SelectList LocationCodes { get; set; }

        public Arg.DataModels.BalanceDues_Customers_Contacts CustomerContactDetail { get; set; }

        public string CompanyName { get; set; }
    }
}
